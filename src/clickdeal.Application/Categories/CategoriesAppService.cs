using clickdeal.Products;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace clickdeal.Categories
{
    public class CategoriesAppService :
    CrudAppService<
        Category, //The Book entity
        CategoryDTO, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateCategoryDTO>, //Used to create/update a book
    ICategoriesAppService //implement the IBookAppService
    {

        private readonly IRepository<Category> _categoriesRepository;
        public CategoriesAppService(IRepository<Category, Guid> repository)
        : base(repository)
        {
            _categoriesRepository = repository;
        }

        [Authorize("clickdeal.Admin")]
        public async override Task<CategoryDTO> CreateAsync(CreateUpdateCategoryDTO input)
        {
            // TODO: implement creating category with subcategories

            return await base.CreateAsync(input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<PagedResultDto<CategoryDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // SHOULD BE DISABLED

            return await base.GetListAsync(input);
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            List<Category> primaryCategories = await _categoriesRepository.GetListAsync();
            List<CategoryDTO> result = new List<CategoryDTO>();

            for (int i = 0; i < primaryCategories.Count; i++)
            {
                CategoryDTO newCat = new CategoryDTO
                {
                    Name = primaryCategories[i].Name,
                    ProductsNumber = primaryCategories[i].ProductsNumber,
                    PhotoBase64 = primaryCategories[i].PhotoBase64,
                    Subcategories = new List<CategoryDTO>()
                };

                // get subcategories
                string subcategoryString = primaryCategories[i].Subcategories;

                if(subcategoryString.Length > 0)
                {
                    string[] subcategoryStringSplit = subcategoryString.Split('#');

                    for (int j = 0; j < subcategoryStringSplit.Length; j++)
                    {
                        // some safety I guess
                        if (subcategoryStringSplit[j].Length == 0)
                            continue;
                        
                        Guid childGUID = Guid.Parse(subcategoryStringSplit[j]);

                        Category? childCategory = await _categoriesRepository.FirstOrDefaultAsync(cat => cat.Id == childGUID);

                        if (childCategory != null)
                        {
                            CategoryDTO childCategoryDTO = new CategoryDTO
                            {
                                Name = childCategory.Name,
                                ProductsNumber = childCategory.ProductsNumber,
                                PhotoBase64 = childCategory.PhotoBase64,
                                Subcategories = new List<CategoryDTO>()
                            };

                            newCat.Subcategories.Add(childCategoryDTO);
                        }
                    }
                }

                result.Add(newCat);
            }

            return result;
        }


        [Authorize("clickdeal.Admin")]
        public override async Task<CategoryDTO> UpdateAsync(Guid id, CreateUpdateCategoryDTO input)
        {
            return await base.UpdateAsync(id, input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<CategoryDTO> GetAsync(Guid id)
        {
            return await base.GetAsync(id);
        }
    }
}
