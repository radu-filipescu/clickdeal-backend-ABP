using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace clickdeal.ProductStocks;

public class BookStoreDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<ProductStock, Guid> _productStockRepository;

    public BookStoreDataSeederContributor(IRepository<ProductStock, Guid> productStockRepository)
    {
        _productStockRepository = productStockRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _productStockRepository.GetCountAsync() <= 0)
        {
            await _productStockRepository.InsertAsync(
                new ProductStock
                {
                    ProductId = Guid.NewGuid(),
                    TotalUnits = 500
                },
                autoSave: true
            );
        }
    }
}
