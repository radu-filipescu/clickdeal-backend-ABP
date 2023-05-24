using clickdeal.Orders;
using clickdeal.Products;
using clickdeal.ProductStocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clickdeal.ProductsStock.ProductStockAppService;
using Volo.Abp.Domain.Repositories;

namespace clickdeal.Utils
{
    public class ProductStockUtils
    {
        private readonly IRepository<ProductStock> _productStockRepository;
        private readonly IRepository<PendingOrder> _pendingOrdersRepository;
        private readonly IRepository<CustomerOrder> _ordersHistoryRepository;
        private readonly IRepository<Product> _productsRepository;

        public ProductStockUtils(IRepository<ProductStock, Guid> productStockRepository,
            IRepository<PendingOrder, Guid> pendingRepository,
            IRepository<CustomerOrder, Guid> ordersHistoryRepository,
            IRepository<Product, Guid> productsRepository)
        {
            _productStockRepository = productStockRepository;
            _pendingOrdersRepository = pendingRepository;
            _ordersHistoryRepository = ordersHistoryRepository;
            _productsRepository = productsRepository;
        }

        
    }
}
