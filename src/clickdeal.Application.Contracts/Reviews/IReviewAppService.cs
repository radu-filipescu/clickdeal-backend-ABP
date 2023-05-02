using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace clickdeal.Reviews
{
    public interface IReviewAppService :
        ICrudAppService< //Defines CRUD methods
        ReviewDTO, //Used to show reviews
        Guid, //Primary key of the review entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateReviewDTO> //Used to create/update a review
    {
    }
}
