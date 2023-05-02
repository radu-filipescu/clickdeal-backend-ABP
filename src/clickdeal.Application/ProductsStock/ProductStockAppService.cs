using clickdeal.Products;
using clickdeal.ProductStocks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
        public ProductStockAppService(IRepository<ProductStock, Guid> repository)
            : base(repository)
        {
            _productStockRepository = repository;
        }

        [Authorize("clickdeal.Admin")]
        public async override Task<ProductStockDTO> CreateAsync(CreateUpdateProductStockDTO input)
        {
            return await base.CreateAsync(input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<PagedResultDto<ProductStockDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ProductStockDTO> UpdateAsync(Guid id, CreateUpdateProductStockDTO input)
        {
            return await base.UpdateAsync(id, input);
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
    }
}
