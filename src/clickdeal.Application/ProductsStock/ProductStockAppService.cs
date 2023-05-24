using clickdeal.Orders;
using clickdeal.Products;
using clickdeal.ProductStocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace clickdeal.ProductsStock
{
    public class ProductStockAppService :
        CrudAppService<
            ProductStock, //The ProductStock entity
            ProductStockDTO, //Used to show ProductStock
            Guid, //Primary key of the ProductStock entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateProductStockDTO>, //Used to create/update a ProductStock
        IProductStockAppService //implement the IProductStockAppService
    {
        private readonly IRepository<ProductStock> _productStockRepository;
        private readonly IRepository<PendingOrder> _pendingOrdersRepository;
        private readonly IRepository<CustomerOrder> _ordersHistoryRepository;
        private readonly IRepository<Product> _productsRepository;
        private readonly ICurrentUser _currentUser;

        public ProductStockAppService(IRepository<ProductStock, Guid> productStockRepository, 
            IRepository<PendingOrder, Guid> pendingRepository, 
            IRepository<CustomerOrder, Guid> ordersHistoryRepository, 
            IRepository<Product, Guid> productsRepository,
            ICurrentUser currentUser)
            : base(productStockRepository)
        {
            _productStockRepository = productStockRepository;
            _pendingOrdersRepository = pendingRepository;
            _ordersHistoryRepository = ordersHistoryRepository;
            _productsRepository = productsRepository;
            _currentUser = currentUser;
        }

        [Authorize("clickdeal.Admin")]
        public async override Task<ProductStockDTO> CreateAsync(CreateUpdateProductStockDTO input)
        {
            ProductStock newStock = new ProductStock();

            newStock.ProductId = input.ProductId;
            newStock.ProductSpecs = input.ProductSpecs;
            newStock.TotalUnits = input.TotalUnits;
            
            var result = await _productStockRepository.InsertAsync(newStock);

            return new ProductStockDTO
            {
                Id = result.Id,
                ProductId = input.ProductId.ToString(),
                ProductSpecs = input.ProductSpecs,
                TotalUnits = input.TotalUnits
            };
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<PagedResultDto<ProductStockDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductStockDTO> UpdateAsync(Guid id, CreateUpdateProductStockDTO input)
        {
            // DISABLED
            return await base.UpdateAsync(id, input);
        }

        public class DeliveryCostDTO
        {
            public double Cost { get; set; }
        }

        public async Task<DeliveryCostDTO> GetDeliveryCostAsync()
        {
            // TODO: get delivery cost dynamically

            return new DeliveryCostDTO
            {
                Cost = 20
            };
        }


        public class AddToStockDTO
        {
            public string StockId { get; set; } = string.Empty;
            public string ProductId { get; set; } = string.Empty;
            public string Specs { get; set;} = string.Empty;
            public int NumberToAdd { get; set; }
        }

        [Authorize("clickdeal.Admin")]
        public async Task<ProductStockDTO> AddItemsToStockAsync(AddToStockDTO input)
        {
            var result = await AddToStockInternalAsync(input);

            if (!result)
                return new ProductStockDTO();

            return new ProductStockDTO
            {
                ProductId = input.ProductId,
                ProductSpecs = input.Specs,
                TotalUnits = input.NumberToAdd
            };
        }

        public class CheckoutCartResponse
        {
            public bool Success { get; set; }
            public List<string> ProductIdsOutOfStock { get; set; } = new List<string>();

            public bool UserAlreadyHasPendingOrder;
        }

        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<CheckoutCartResponse> CheckoutCart(ShoppingCartDTO input)
        {
            // 1. Some sanity checks for input 

            CheckoutCartResponse badResponse = new CheckoutCartResponse { Success = false };

            if (input.Entries.Count == 0)
                return badResponse;
            else
            {
                foreach (var entry in input.Entries)
                {
                    if (entry.Quantity < 0)
                        return badResponse;

                    Guid productId;
                    bool productIdValid = Guid.TryParse(entry.ProductId, out productId);

                    if (!productIdValid)
                        return badResponse;

                    var foundProduct = await _productsRepository.FirstOrDefaultAsync(product => product.Id == productId);

                    // if there is a productId in the cart that doesn't exist
                    if (foundProduct == null)
                        return badResponse;
                }
            }

            if (_currentUser.IsAuthenticated)
            {
                // if username is empty there's something sketchy
                if(input.Username.IsNullOrEmpty())
                    return badResponse;

                // if user is authenticated and DTO with userId is empty or doesn't match there's something sketchy
                if (input.Username != _currentUser.UserName)
                    return badResponse;
            }

            // now we check if the client's products are available

            // 2. First check if are there any pending orders which expired 
            // (those products should be placed back in their stocks)
            await RefreshPendingOrdersInternal();

            // 3. After checking the pending orders and updating products stocks
            // we can now check if the current shopping cart is valid
            bool notEnough = false;
            List<(ProductStock, int)> cartStocks = new List<(ProductStock, int)>(); 

            foreach (var cartEntry in input.Entries)
            {
                if (cartEntry == null)
                    return badResponse;

                // check if the product with those specs is available
                var isEnoughAvailable = await IsProductAvailableInternal(cartEntry);

                if (isEnoughAvailable == null)
                {
                    badResponse.ProductIdsOutOfStock.Add(cartEntry.ProductId);
                    notEnough = true;
                }
                else
                {
                    cartStocks.Add(((ProductStock, int))(isEnoughAvailable));
                }
            }
            
            if (notEnough)
                return badResponse;

            // 4. if the wanted products are available
            // we move this order into pending

            PendingOrder newPendingOrder = new PendingOrder();

            newPendingOrder.CustomerEmail = input.OrderEmail;
            
            if (input.Username != null)
                newPendingOrder.UserId = _currentUser.GetId();
            
            // add pending products to pending order and 
            foreach(var pendingStock in cartStocks)
            {
                var newPendingEntry = new UserOrderEntry();
                newPendingEntry.StockId = pendingStock.Item1.Id;
                newPendingEntry.ProductId = pendingStock.Item1.ProductId;
                newPendingEntry.ProductSpecs = pendingStock.Item1.ProductSpecs;
                newPendingEntry.Quantity = pendingStock.Item2;

                // find pricePerUnit of that product
                var foundProduct = await _productsRepository.FirstOrDefaultAsync(product => product.Id == newPendingEntry.ProductId && 
                                                                            product.AreSpecsEqual(product.Specs, newPendingEntry.ProductSpecs));
                if (foundProduct == null)
                    return badResponse;
                else
                    newPendingEntry.PricePerUnit = foundProduct.Price;

                newPendingOrder.OrderEntries.Add(newPendingEntry);
                newPendingOrder.TotalCost += newPendingEntry.PricePerUnit * newPendingEntry.Quantity;
            }

            // add delivery fee
            double deliveryCost = (await GetDeliveryCostAsync()).Cost;

            newPendingOrder.DeliveryCost = deliveryCost;
            newPendingOrder.TotalCost = deliveryCost;

            // add this order to pending ones
            var result = await _pendingOrdersRepository.InsertAsync(newPendingOrder);

            // remove pending products from stock
            foreach (var pendingStock in cartStocks)
            {
                pendingStock.Item1.TotalUnits -= pendingStock.Item2;

                await _productStockRepository.UpdateAsync(pendingStock.Item1);
            }

            return new CheckoutCartResponse
            {
                Success = true
            };
        }

        // returns null if it is not available or the stock if it's ok
        private async Task<(ProductStock, int)?> IsProductAvailableInternal(ShoppingCartEntry input)
        {
            var productsFind = await _productsRepository.GetListAsync(product => product.Id == Guid.Parse(input.ProductId));

            // there is no product with that Id
            if (productsFind == null || productsFind.Count == 0)
                return null;

            productsFind = await _productsRepository.GetListAsync(product => product.Id == Guid.Parse(input.ProductId));
            productsFind = productsFind.Where(product => product.AreSpecsEqual(product.Specs, input.Specs)).ToList();
            
            // there is no product with that Id and Specs
            if (productsFind == null || productsFind.Count == 0)
                return null;

            // get all stocks with that productId
            var stockFindAll = await _productStockRepository.GetListAsync(stock => stock.ProductId == Guid.Parse(input.ProductId) && stock.IsDeleted == false);

            // there is no stock with that productId
            if (stockFindAll == null || stockFindAll.Count == 0)
                return null;

            // get all stocks with those specs
            var stockFindWithSpecs = stockFindAll.FirstOrDefault(stock => stock.AreSpecsEqual(stock.ProductSpecs, input.Specs));

            // there is no stock with that productId and those specs
            if (stockFindWithSpecs == null)
                return null;

            // at this point, we found a stock with that productId and those specs

            int availableQuantitiy = stockFindWithSpecs.TotalUnits;

            if (availableQuantitiy < input.Quantity) 
                return null;

            return (stockFindWithSpecs, input.Quantity);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductStockDTO> GetAsync(Guid id)
        {
            return await base.GetAsync(id);
        }

        private async Task<bool> AddToStockInternalAsync(AddToStockDTO input)
        {
            Guid stockId = Guid.Parse(input.StockId);

            // we find the stock that we want to add
            var result = await _productStockRepository.FirstOrDefaultAsync(stock => stock.Id == stockId);

            // we still check for productId and specs for redundancy
            if (result.ProductId.ToString() != input.ProductId)
                return false;

            if (result.AreSpecsEqual(input.Specs, result.ProductSpecs) == false)
                return false;

            // everything was ok
            result.TotalUnits += input.NumberToAdd;

            var updateResult = await _productStockRepository.UpdateAsync(result);

            return true;
        }

        // this functions goes through pending orders
        // and checks it they timed out 
        // if a pending order timed out,
        // all the products from there are returned to their stocks
        private async Task RefreshPendingOrdersInternal()
        {
            const int PENDING_TIME_MINUTES = 30;

            var result = await _pendingOrdersRepository.GetListAsync(pendingOrder => true);

            HashSet<Guid> markForRemove = new HashSet<Guid>();

            // if something goes wrong when matching products to stocks
            // we don't remove that order from pending,
            // we only remove and add back to stock the products that match
            bool pendingOrderRemoveError = false;

            for (int i = 0; i < result.Count; i++)
            {
                TimeSpan timeDifference = DateTime.Now - result[i].CreationTime;

                // pending order expired, so we remove all products from
                // there and back into the stocks, and we remove the order from pending
                if (timeDifference.TotalMinutes > PENDING_TIME_MINUTES)
                {
                    foreach (var orderEntry in result[i].OrderEntries)
                    {
                        Guid currentStockId = orderEntry.StockId;
                        Guid currentProductId = orderEntry.ProductId;
                        string currentSpecs = orderEntry.ProductSpecs;
                        int currentQuantity = orderEntry.Quantity;
                        int currentNumToAdd = orderEntry.Quantity;

                        // checking that all matches
                        var correspondingStock = await _productStockRepository.FirstOrDefaultAsync(stock => stock.Id == currentStockId);

                        // the pending order entry should point to a valid stock
                        if (correspondingStock == null)
                        {
                            pendingOrderRemoveError = true;
                            continue;
                        }
                            

                        // even if stockId matches, we still check for product Id and Specs for safety and redundancy 
                        if (correspondingStock.ProductId != currentProductId)
                        {
                            pendingOrderRemoveError = true;
                            continue;
                        }
                           

                        if (correspondingStock.AreSpecsEqual(correspondingStock.ProductSpecs, currentSpecs) == false)
                        {
                            pendingOrderRemoveError = true;
                            continue;
                        }
        

                        if (currentNumToAdd < 1)
                        {
                            pendingOrderRemoveError = true;
                            continue;
                        }

                        // everything seems ok, we move those products back into stock
                        var addItemsInput = new AddToStockDTO();
                        addItemsInput.StockId = currentStockId.ToString();
                        addItemsInput.ProductId = currentProductId.ToString();
                        addItemsInput.Specs = currentSpecs;
                        addItemsInput.NumberToAdd = currentNumToAdd;

                        var addBackResult = await AddToStockInternalAsync(addItemsInput);

                        if (addBackResult == false)
                        {
                            pendingOrderRemoveError = true;
                        }
                    }

                    // after we push all the pending products back in their stocks,
                    // we mark this pending order to be removed (if everything went ok)

                    if(!pendingOrderRemoveError)
                        markForRemove.Add(result[i].Id);
                }
            }

            // after checking all pending orders,
            // we remove the ones which expired
            foreach (Guid markedId in markForRemove)
            {
                await _pendingOrdersRepository.DeleteAsync(pendingOrder => pendingOrder.Id == markedId);
            }
        }
    }
}
