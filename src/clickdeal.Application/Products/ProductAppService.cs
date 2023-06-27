using clickdeal.Categories;
using clickdeal.ProductStocks;
using clickdeal.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Polly.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace clickdeal.Products
{
    public class ProductAppService :
        CrudAppService<
            Product, //The product entity
            ProductDTO, //Used to show products
            Guid, //Primary key of the product entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateProductDTO>, //Used to create/update a product
        IProductAppService //implement the IProductAppService
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<ProductStock> _productsStockRepository;
        private readonly IBlobContainer _blobContainer;

        public ProductAppService(IRepository<Product, Guid> repository, IRepository<Category, Guid> categoriesRepository, IRepository<ProductStock, Guid> productStockRepository, IBlobContainer blobContainer)
            : base(repository)
        {
            _productsRepository = repository;
            _categoriesRepository = categoriesRepository;
            _blobContainer = blobContainer;
            _productsStockRepository = productStockRepository;

            sharedClient = new()
            {
                BaseAddress = new Uri("https://ws.smartbill.ro/SBORO/api/"),
            };

            //sharedClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            sharedClient.DefaultRequestHeaders.Add("authorization", "Basic bmFybGV4Y29uY2VwdEBnbWFpbC5jb206MDAyfGM0MjQ3MzYyZjg4MzEyZDQ3NDhlZjI1NTQ4MjQ5ZDcw");

        }

        private static HttpClient sharedClient;


        [Authorize("clickdeal.Admin")]
        public async override Task<ProductDTO> CreateAsync(CreateUpdateProductDTO input)
        {
            //Product newProduct = new Product();

            //newProduct.Name = input.Name;
            //newProduct.DescriptionShort = input.DescriptionShort;
            //newProduct.DescriptionLong = input.DescriptionLong;
            //newProduct.Information = input.Information;
            //newProduct.Price = input.Price;
            //newProduct.Brand = input.Brand;

            //// handle categories  (input is assumed to be  #category1#category2#category3#
            //string[] categories = input.Categories.Split("#");
            //List<string> goodCategories = new List<string>();

            //for(int i = 0; i < categories.Length; i++)
            //{
            //    if (categories[i].IsNullOrEmpty())
            //        continue;

            //    var categoryResult = await _categoriesRepository.FirstOrDefaultAsync(cat => cat.Name == categories[i]);

            //    if (categoryResult == null)
            //        continue;

            //    // add new product to category
            //    categoryResult.ProductsNumber++;

            //    await _categoriesRepository.UpdateAsync(categoryResult);
            //    goodCategories.Add(categories[i]);
            //}

            //if(goodCategories.Count > 0)
            //{
            //    string categoryField = "#" + String.Join("#", goodCategories) + "#";

            //    newProduct.Categories = categoryField;
            //}

            //// add to database to get newProductId
            //var result = await _productsRepository.InsertAsync(newProduct);

            //string newProductId = result.Id.ToString();

            //try
            //{
            //    byte[] imageBytes = System.Text.Encoding.UTF8.GetBytes(input.Image);

            //    // save to file storage
            //    await _blobContainer.SaveAsync(newProductId, imageBytes);
            //}
            //catch(Exception ex)
            //{
            //    var errorEx = ex;
            //}

            //ProductDTO response = new ProductDTO();

            //response.Id = result.Id;
            //response.Name = result.Name;
            //response.DescriptionShort = result.DescriptionShort;
            //response.Price = result.Price;
            //response.Brand = result.Brand;

            //return response;

            // THIS IS DISABLED

            return new ProductDTO();
        }

        [Authorize("clickdeal.Admin")]
        public async Task SyncProductsWithSmartbill(CreateUpdateProductDTO input)
        {
            // THIS SHOULD SYNC PRODUCTS WITH SMARTBILL
            var todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            using HttpResponseMessage response = await sharedClient.GetAsync("stocks?cif=47561891&date=" + todayDate + "&warehouseName=" + "Gestiune valorica");

            //response.EnsureSuccessStatusCode()
            //    .WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(jsonResponse);
            var productList = json.GetOrDefault("list")[0];
            var actualList = productList["products"];

            for (int i = 0; i < actualList.Count(); i++)
            {
                if (actualList[i]["productCode"] == null)
                    continue;
                
                // if there's no smartbill Id, continue
                string smartbillId = actualList[i]["productCode"].ToString();

                if (smartbillId.Length == 0)
                    continue;

                string smartbillName = "";

                if (actualList[i]["productName"] != null)
                    smartbillName = actualList[i]["productName"].ToString();

                int smarbillQuantity = Int32.Parse(actualList[i]["quantity"].ToString());

                Product? result = null;

                try
                {
                    result = await _productsRepository.FirstOrDefaultAsync(product => product.CodIdentificareSmartbill == smartbillId);
                }
                catch(Exception ex)
                {
                    var mzg = ex;
                }

                // if that product is not yet in our database, we add it
                if(result == null)
                {
                    Product newProduct = new Product();

                    newProduct.CodIdentificareSmartbill = smartbillId;
                    newProduct.SmartbillProductName = smartbillName;
                    newProduct.Quantity = smarbillQuantity;

                    await _productsRepository.InsertAsync(newProduct);
                }
                // else we just update the quantity and the smartbill name
                else
                {
                    result.SmartbillProductName = smartbillName;
                    result.Quantity = smarbillQuantity;

                    await _productsRepository.UpdateAsync(result);
                }
            }

            //Console.WriteLine($"{jsonResponse}\n");
        }


        [Authorize("clickdeal.Admin")]
        public override async Task<PagedResultDto<ProductDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // THIS IS DISABLED
            
            return await base.GetListAsync(input);
        }

        public class ProductsCountResponse
        {
            public int Value { get; set; }
        }

        public async Task<ProductsCountResponse> GetProductsCountWithFilters(FilteredProductsRequestDTO input)
        {
            // filtering products

            var entitiesQuery = await _productsRepository.GetQueryableAsync();

            if (input.PriceMin != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price >= input.PriceMin);

            if (input.PriceMax != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price <= input.PriceMax);

            if (input.Brand != null)
                entitiesQuery = entitiesQuery.Where(product => product.Brand == input.Brand);

            if (input.Category != null && input.Category.Length > 0)
                entitiesQuery = entitiesQuery.Where(product => product.Categories.Contains(input.Category));

            if (input.OrderBy != null && input.OrderBy.Length > 0)
            {
                if (input.OrderBy == "DATE-ASC")
                    entitiesQuery = entitiesQuery.OrderBy(product => product.CreationTime);

                if (input.OrderBy == "DATE-DESC")
                    entitiesQuery = entitiesQuery.OrderByDescending(product => product.CreationTime);

                if (input.OrderBy == "PRICE-ASC")
                    entitiesQuery = entitiesQuery.OrderBy(product => product.Price);

                if (input.OrderBy == "PRICE-DESC")
                    entitiesQuery = entitiesQuery.OrderByDescending(product => product.Price);
            }

            if (input.SkipCount > 0)
                entitiesQuery = entitiesQuery.Skip(input.SkipCount);

            if (input.MaxResultCount > 0)
                entitiesQuery = entitiesQuery.Take(input.MaxResultCount);

            var result = entitiesQuery.ToList();

            var mappedResult = ObjectMapper.Map<List<Product>, List<ProductDTO>>(result);

            return new ProductsCountResponse
            {
                Value = mappedResult.Count
            };
        }

        public async Task<List<ProductDTO>> GetProductsFiltered(FilteredProductsRequestDTO input)
        {
            // filtering products
            
            var entitiesQuery = await _productsRepository.GetQueryableAsync();

            entitiesQuery = entitiesQuery.Where(product => product.VisibleOnWebsite);

            if (input.PriceMin != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price >= input.PriceMin);

            if (input.PriceMax != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price <= input.PriceMax);

            if(input.Brand != null)
                entitiesQuery = entitiesQuery.Where(product => product.Brand ==  input.Brand);

            if(input.Category != null && input.Category.Length > 0)
                entitiesQuery = entitiesQuery.Where(product => product.Categories.Contains(input.Category));

            if (input.MinDiscount != null && input.MinDiscount > 0.5)
                entitiesQuery = entitiesQuery.Where(product => product.PriceDiscount >= input.MinDiscount);


            if(input.OrderBy != null && input.OrderBy.Length > 0)
            {
                if (input.OrderBy == "DATE-ASC")
                    entitiesQuery = entitiesQuery.OrderBy(product => product.CreationTime);

                if (input.OrderBy == "DATE-DESC")
                    entitiesQuery = entitiesQuery.OrderByDescending(product => product.CreationTime);

                if (input.OrderBy == "PRICE-ASC")
                    entitiesQuery = entitiesQuery.OrderBy(product => product.Price);

                if (input.OrderBy == "PRICE-DESC")
                    entitiesQuery = entitiesQuery.OrderByDescending(product => product.Price);
            }

            if(input.SkipCount > 0)
                entitiesQuery = entitiesQuery.Skip(input.SkipCount);

            if(input.MaxResultCount > 0)
                entitiesQuery = entitiesQuery.Take(input.MaxResultCount);

            var result = entitiesQuery.ToList();
            List<ProductDTO> mappedResult = new List<ProductDTO>();

            foreach (var prod in result)
            {
                ProductDTO newProductDTO = new ProductDTO();

                newProductDTO.Name = prod.Name;
                newProductDTO.Price = prod.Price;
                newProductDTO.PriceDiscount = prod.PriceDiscount;
                newProductDTO.ProductId = prod.Id.ToString();
                newProductDTO.DescriptionShort = prod.DescriptionShort;
                newProductDTO.DescriptionLong = prod.DescriptionLong;
                newProductDTO.Brand = prod.Brand;
                newProductDTO.Categories = prod.Categories;
                newProductDTO.Specs = prod.Specs;
                newProductDTO.Image = prod.PhotoPaths;

                mappedResult.Add(newProductDTO);
            }


            foreach(var product in mappedResult)
            {
                var productImage = await _blobContainer.GetAllBytesOrNullAsync(product.ProductId.ToString());
                if (productImage == null)
                    continue;

                product.Image = System.Text.Encoding.UTF8.GetString(productImage);
            }

            return mappedResult;
        }

        [Authorize("clickdeal.Admin")]
        public async Task<List<ProductDTO>> GetProductsFilteredAdmin(FilteredProductsRequestDTO input)
        {
            await RefreshCategories();
            
            // filtering products
            var entitiesQuery = await _productsRepository.GetQueryableAsync();

            if (input.PriceMin != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price >= input.PriceMin);

            if (input.PriceMax != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price <= input.PriceMax);

            if (input.Brand != null)
                entitiesQuery = entitiesQuery.Where(product => product.Brand == input.Brand);

            if (input.Category != null && input.Category.Length > 0)
                entitiesQuery = entitiesQuery.Where(product => product.Categories.Contains(input.Category));

            if (input.MinDiscount != null && input.MinDiscount > 0.5)
                entitiesQuery = entitiesQuery.Where(product => product.PriceDiscount >= input.MinDiscount);


            if (input.OrderBy != null && input.OrderBy.Length > 0)
            {
                if (input.OrderBy == "DATE-ASC")
                    entitiesQuery = entitiesQuery.OrderBy(product => product.CreationTime);

                if (input.OrderBy == "DATE-DESC")
                    entitiesQuery = entitiesQuery.OrderByDescending(product => product.CreationTime);

                if (input.OrderBy == "PRICE-ASC")
                    entitiesQuery = entitiesQuery.OrderBy(product => product.Price);

                if (input.OrderBy == "PRICE-DESC")
                    entitiesQuery = entitiesQuery.OrderByDescending(product => product.Price);
            }

            if (input.SkipCount > 0)
                entitiesQuery = entitiesQuery.Skip(input.SkipCount);

            if (input.MaxResultCount > 0)
                entitiesQuery = entitiesQuery.Take(input.MaxResultCount);

            var result = entitiesQuery.ToList();
            List<ProductDTO> mappedResult = new List<ProductDTO>();

            foreach (var prod in result)
            {
                ProductDTO newProductDTO = new ProductDTO();

                newProductDTO.CodIdentificareSmartbill = prod.CodIdentificareSmartbill;
                newProductDTO.SmartbillName = prod.SmartbillProductName;
                newProductDTO.Name = prod.Name;
                newProductDTO.Price = prod.Price;
                newProductDTO.PriceDiscount = prod.PriceDiscount;
                newProductDTO.ProductId = prod.Id.ToString();
                newProductDTO.DescriptionShort = prod.DescriptionShort;
                newProductDTO.DescriptionLong = prod.DescriptionLong;
                newProductDTO.Brand = prod.Brand;
                newProductDTO.Categories = prod.Categories;
                newProductDTO.Specs = prod.Specs;
                newProductDTO.Quantity = prod.Quantity;
                newProductDTO.IsVisible = prod.VisibleOnWebsite;
                newProductDTO.Image = prod.PhotoPaths;

                mappedResult.Add(newProductDTO);
            }


            foreach (var product in mappedResult)
            {
                var productImage = await _blobContainer.GetAllBytesOrNullAsync(product.ProductId.ToString());
                if (productImage == null)
                    continue;

                product.Image = System.Text.Encoding.UTF8.GetString(productImage);
            }

            return mappedResult;
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductDTO> UpdateAsync(Guid id, CreateUpdateProductDTO input)
        {
            // DISABLED

            return new ProductDTO();
        }

        [Authorize("clickdeal.Admin")]
        [HttpPut("api/app/edit-product-working")]
        [IgnoreAntiforgeryToken]
        public async Task<ProductDTO> UpdateProductWorkingAsync(CreateUpdateProductDTO input)
        {
            var product = await _productsRepository.FirstOrDefaultAsync(product => product.Id == Guid.Parse(input.ProductId));

            // something's wrong
            if (product == null || product.CodIdentificareSmartbill != input.CodIdentificareSmartBill)
                return new ProductDTO();

            product.Name = input.Name;
            product.Price = input.Price;
            product.PhotoPaths = input.Image;
            product.DescriptionLong = input.DescriptionLong;
            product.DescriptionShort = input.DescriptionShort;
            product.Brand = input.Brand;
            product.Categories = input.Categories;
            product.VisibleOnWebsite = input.IsVisible;

            var result2 = await _productsRepository.UpdateAsync(product);

            await RefreshCategories();

            var success = new ProductDTO();
            success.Name = "Editing succesful!";

            return success;
        }

        private async Task RefreshCategories()
        {
            List<Category> allCategories = (await _categoriesRepository.GetQueryableAsync()).Where(category => true).ToList();
            List<Product> allProducts = (await _productsRepository.GetQueryableAsync()).Where(product => product.VisibleOnWebsite).ToList();

            for(int i = 0; i < allCategories.Count(); i++)
            {
                allCategories[i].ProductsNumber = 0;

                await _categoriesRepository.UpdateAsync(allCategories[i]);
            }

            for(int i = 0; i < allProducts.Count(); i++)
            {
                if (allProducts[i].Categories.IsNullOrEmpty())
                    continue;
                
                string[] categories = allProducts[i].Categories.Split("#");
                List<string> goodCategories = new List<string>();

                for (int j = 0; j < categories.Length; j++)
                {
                    if (categories[j].IsNullOrEmpty())
                        continue;

                    var categoryResult = await _categoriesRepository.FirstOrDefaultAsync(cat => cat.Name == categories[j]);

                    if (categoryResult == null)
                        continue;

                    // add new product to category
                    categoryResult.ProductsNumber++;

                    await _categoriesRepository.UpdateAsync(categoryResult);
                    goodCategories.Add(categories[j]);
                }

                if (goodCategories.Count > 0)
                {
                    string categoryField = "#" + String.Join("#", goodCategories) + "#";

                    allProducts[i].Categories = categoryField;
                    await _productsRepository.UpdateAsync(allProducts[i]);
                }
            }
        }


        [Authorize("clickdeal.Admin")]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductDTO> GetAsync(Guid id)
        {
            // DISABLED
            return await base.GetAsync(id);
        }

        public class ProductsDetailInput
        {
            public string ProductId { get; set; } = string.Empty;
        }

        public async Task<ProductDTO?> GetProductDetailsAsync(ProductsDetailInput input)
        {
            Guid productGuid;

            bool valid = Guid.TryParse(input.ProductId, out productGuid);

            if (!valid)
                return null;

            var result = await _productsRepository.FirstOrDefaultAsync(product => product.Id == productGuid);

            if (result == null || result.VisibleOnWebsite == false)
                return new ProductDTO();

            var resultDTO = new ProductDTO();

            resultDTO.Brand = result.Brand;
            resultDTO.Categories = result.Categories;
            resultDTO.DescriptionShort = result.DescriptionShort;
            resultDTO.DescriptionLong = result.DescriptionLong;
            resultDTO.Image = result.PhotoPaths;
            resultDTO.Name = result.Name;
            resultDTO.Price = result.Price;
            resultDTO.Specs = result.Specs;
            resultDTO.ProductId = result.Id.ToString();
            resultDTO.PriceDiscount = result.PriceDiscount;

            if (result.Quantity > 0)
                resultDTO.Quantity = 1;

            return resultDTO;
        }

        [Authorize("clickdeal.Admin")]
        public async Task<Product> GetProductDetailsSmartbillCodeAsync(ProductsDetailInput input)
        {
            var result = await _productsRepository.FirstOrDefaultAsync(product => product.CodIdentificareSmartbill == input.ProductId);

            if (result == null)
                return new Product();

            if (result != null)
            {
                var productImage = await _blobContainer.GetAllBytesOrNullAsync(input.ProductId);
                //if (productImage != null)
                //    result.PhotoPaths = System.Text.Encoding.UTF8.GetString(productImage);
            }

            return result;
        }

        public class InStockResponseDTO
        {
            public bool InStock { get; set;}
        }

        public class InStockRequestDTO
        {
            public string ProductId { get; set; } = string.Empty;

            public string Specs { get; set; } = string.Empty;
        }

        [IgnoreAntiforgeryToken]
        public async Task<InStockResponseDTO> IsProductInStock(InStockRequestDTO input)
        {
            InStockResponseDTO nope = new InStockResponseDTO
            {
                InStock = false,
            };
            
            Guid productId;

            var convertResult = Guid.TryParse(input.ProductId, out productId);

            if (!convertResult)
                return nope;

            var result = await _productsStockRepository.GetListAsync(stock => stock.ProductId == productId);
            var result2 = result.FirstOrDefault(stock => Product.AreSpecsEqual(stock.ProductSpecs, input.Specs));

            if (result2 == null)
                return nope;

            if (result2.TotalUnits >= 1)
                return new InStockResponseDTO { InStock = true };

            return nope;
        }
    }


}
