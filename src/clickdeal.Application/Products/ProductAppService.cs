using clickdeal.Categories;
using clickdeal.Reviews;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IBlobContainer _blobContainer;

        public ProductAppService(IRepository<Product, Guid> repository, IRepository<Category, Guid> categoriesRepository, IBlobContainer blobContainer)
            : base(repository)
        {
            _productsRepository = repository;
            _categoriesRepository = categoriesRepository;
            _blobContainer = blobContainer;
        }

        [Authorize("clickdeal.Admin")]
        public async override Task<ProductDTO> CreateAsync(CreateUpdateProductDTO input)
        {
            Product newProduct = new Product();

            newProduct.Name = input.Name;
            newProduct.Description = input.Description;
            newProduct.Price = input.Price;
            newProduct.Brand = input.Brand;
            
            // handle categories  (input is assumed to be  #category1#category2#category3#
            string[] categories = input.Categories.Split("#");
            List<string> goodCategories = new List<string>();

            for(int i = 0; i < categories.Length; i++)
            {
                if (categories[i].IsNullOrEmpty())
                    continue;
                
                var categoryResult = await _categoriesRepository.FirstOrDefaultAsync(cat => cat.Name == categories[i]);

                if (categoryResult == null)
                    continue;

                // add new product to category
                categoryResult.ProductsNumber++;

                await _categoriesRepository.UpdateAsync(categoryResult);
                goodCategories.Add(categories[i]);
            }

            if(goodCategories.Count > 0)
            {
                string categoryField = "#" + String.Join("#", goodCategories) + "#";

                newProduct.Categories = categoryField;
            }

            // add to database to get newProductId
            var result = await _productsRepository.InsertAsync(newProduct);

            string newProductId = result.Id.ToString();

            try
            {
                byte[] imageBytes = System.Text.Encoding.UTF8.GetBytes(input.Image);

                // save to file storage
                await _blobContainer.SaveAsync(newProductId, imageBytes);
            }
            catch(Exception ex)
            {
                var errorEx = ex;
            }
           
            ProductDTO response = new ProductDTO();

            response.Id = result.Id;
            response.Name = result.Name;
            response.Description = result.Description;
            response.Price = result.Price;
            response.Brand = result.Brand;

            return response;
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<PagedResultDto<ProductDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // THIS IS DISABLED
            
            return await base.GetListAsync(input);
        }

        [Authorize("clickdeal.User")]
        public async Task<List<ProductDTO>> GetProductsFiltered(FilteredProductsRequestDTO input)
        {
            // filtering products
            
            var entitiesQuery = await _productsRepository.GetQueryableAsync();

            if(input.PriceMin != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price >= input.PriceMin);

            if (input.PriceMax != null)
                entitiesQuery = entitiesQuery.Where(product => product.Price <= input.PriceMax);

            if(input.Brand != null)
                entitiesQuery = entitiesQuery.Where(product => product.Brand ==  input.Brand);

            if(input.Category != null && input.Category.Length > 0)
                entitiesQuery = entitiesQuery.Where(product => product.Categories.Contains(input.Category));

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

            var mappedResult = ObjectMapper.Map<List<Product>, List<ProductDTO>>(result);

            foreach(var product in mappedResult)
            {
                var productImage = await _blobContainer.GetAllBytesOrNullAsync(product.Id.ToString());

                if (productImage == null)
                    continue;

                product.Image = System.Text.Encoding.UTF8.GetString(productImage);
            }

            return mappedResult;
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductDTO> UpdateAsync(Guid id, CreateUpdateProductDTO input)
        {
            return await base.UpdateAsync(id, input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductDTO> GetAsync(Guid id)
        {
            return await base.GetAsync(id);
        }
    }


}
