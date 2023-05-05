using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace clickdeal.Categories
{
    public interface ICategoriesAppService :
    ICrudAppService< //Defines CRUD methods
        CategoryDTO,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDTO>
    {

    }
}
