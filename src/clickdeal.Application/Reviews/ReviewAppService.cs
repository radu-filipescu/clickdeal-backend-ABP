using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace clickdeal.Reviews
{
    public class ReviewAppService :
        CrudAppService<
        Review, //The review entity
        ReviewDTO, //Used to show reviews
        Guid, //Primary key of the review entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateReviewDTO>, //Used to create/update a review
    IReviewAppService //implement the IReviewAppService
    {
        public ReviewAppService(IRepository<Review, Guid> repository)
        : base(repository)
        {

        }
    }
}
