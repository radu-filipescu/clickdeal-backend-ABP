using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace clickdeal.Products
{
    public interface IProductAppService :
    ICrudAppService< //Defines CRUD methods
        ProductDTO, //Used to show products
        Guid, //Primary key of the product entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProductDTO> //Used to create/update a product
    {

    }
}
