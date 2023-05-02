using clickdeal.Reviews;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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

        public ProductAppService(IRepository<Product, Guid> repository)
            : base(repository)
        {
            _productsRepository = repository;
        }

        [Authorize("clickdeal.Admin")]
        public async override Task<ProductDTO> CreateAsync(CreateUpdateProductDTO input)
        {
            return await base.CreateAsync(input);
        }

        [Authorize("clickdeal.User")]
        public override async Task<PagedResultDto<ProductDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
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
