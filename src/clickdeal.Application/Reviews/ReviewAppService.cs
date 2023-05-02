using clickdeal.Permissions;
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
using Volo.Abp.Threading;
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
        private readonly IRepository<Review> _reviewsRepository;

        public ReviewAppService(IRepository<Review, Guid> repository)
        : base(repository)
        {
            _reviewsRepository = repository;
        }

        [Authorize("clickdeal.User")]
        public async override Task<ReviewDTO> CreateAsync(CreateUpdateReviewDTO input)
        {
            return await base.CreateAsync(input);
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsForProduct(Guid productId)
        {
            var result = await _reviewsRepository.GetListAsync(review => review.ProductId == productId);

            var mappedResult = ObjectMapper.Map<List<Review>, List<ReviewDTO>>(result);

            return mappedResult;
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<PagedResultDto<ReviewDTO>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ReviewDTO> UpdateAsync(Guid id, CreateUpdateReviewDTO input)
        {
            return await base.UpdateAsync(id, input);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }

        [Authorize("clickdeal.Admin")]
        public override async Task<ReviewDTO> GetAsync(Guid id)
        {
            return await base.GetAsync(id);
        }
    }
}
