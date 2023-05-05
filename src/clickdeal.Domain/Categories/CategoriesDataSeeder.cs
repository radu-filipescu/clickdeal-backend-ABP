using clickdeal.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace clickdeal.Categories
{
    public class CategoriesDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Category, Guid> _categoriesRepository;

        public CategoriesDataSeeder(IRepository<Category, Guid> categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _categoriesRepository.GetCountAsync() <= 0)
            {
                await _categoriesRepository.InsertAsync(
                    new Category
                    {
                        Name = "Telefoane",
                        ProductsNumber = 0,
                        PhotoBase64 = "",
                        Subcategories = "",
                        ParentGuid = null,
                        Visible = true
                    },
                    autoSave: true
                );

                await _categoriesRepository.InsertAsync(
                    new Category
                    {
                        Name = "Electronice",
                        ProductsNumber = 0,
                        PhotoBase64 = "",
                        Subcategories = "",
                        ParentGuid = null,
                        Visible = true
                    },
                    autoSave: true
                );

                await _categoriesRepository.InsertAsync(
                    new Category
                    {
                        Name = "Accesorii Auto",
                        ProductsNumber = 0,
                        PhotoBase64 = "",
                        Subcategories = "",
                        ParentGuid = null,
                        Visible = true
                    },
                    autoSave: true
                );

                await _categoriesRepository.InsertAsync(
                    new Category
                    {
                        Name = "Home",
                        ProductsNumber = 0,
                        PhotoBase64 = "",
                        Subcategories = "",
                        ParentGuid = null,
                        Visible = true
                    },
                    autoSave: true
                );

                await _categoriesRepository.InsertAsync(
                    new Category
                    {
                        Name = "Sport",
                        ProductsNumber = 0,
                        PhotoBase64 = "",
                        Subcategories = "",
                        ParentGuid = null,
                        Visible = true
                    },
                    autoSave: true
                );
            }
        }
    }
}
