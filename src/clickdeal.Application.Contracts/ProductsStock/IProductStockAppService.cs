using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace clickdeal.ProductsStock
{
    public interface IProductStockAppService :
        ICrudAppService< //Defines CRUD methods
            ProductStockDTO, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateProductStockDTO> //Used to create/update a book
    {

    }
}
