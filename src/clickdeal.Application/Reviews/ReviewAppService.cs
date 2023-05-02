using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

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

        [Authorize]
        public async override Task<ReviewDTO> CreateAsync(CreateUpdateReviewDTO input)
        {
            return await base.CreateAsync(input);
        }
    }
}
