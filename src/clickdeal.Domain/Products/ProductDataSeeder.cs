using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using clickdeal.Reviews;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace clickdeal.Products;
public class ProductDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductDataSeeder(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        //if (await _productRepository.GetCountAsync() <= 0)
        //{
        //    await _productRepository.InsertAsync(
        //        new Product
        //        {
        //            Name = "suport auto telefon",
        //            Price = 50,
        //            DescriptionShort = "suport de lipit pe parbriz pentru telefonul tau",
        //            DescriptionLong = "",
        //            Categories = "auto#electronice#telefoane",
        //            Specs = "to-do-next"
        //        },
        //        autoSave: true
        //    );
        //}
    }
}