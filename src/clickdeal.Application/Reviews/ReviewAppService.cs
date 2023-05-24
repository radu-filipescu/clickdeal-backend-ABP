using clickdeal.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        [IgnoreAntiforgeryToken]
        public async override Task<ReviewDTO> CreateAsync(CreateUpdateReviewDTO input)
        {
            Review newReview = new Review();

            newReview.ReviewUsername = input.ReviewUsername;
            newReview.Title = "";
            newReview.Content = input.Content;
            newReview.NumberOfStars = input.NumberOfStars;
            newReview.ProductId = input.ProductId;

            var result = await _reviewsRepository.InsertAsync(newReview);

            return new ReviewDTO();
        }

        public class ProductReviewsByIdDTO
        {
            public string productId { get; set; } = string.Empty;
        }

        public async Task<IEnumerable<ReviewDTO>?> GetReviewsForProduct(ProductReviewsByIdDTO input)
        {
            // TODO: only return approved reviews for the product

            Guid productId;
            bool valid = Guid.TryParse(input.productId, out productId);

            if (!valid)
                return null;

            var result = await _reviewsRepository.GetListAsync(review => review.ProductId == productId);


            List<ReviewDTO> resultDTOs = new List<ReviewDTO>();

            foreach(var review in result)
            {
                resultDTOs.Add(new ReviewDTO
                {
                    Username = review.ReviewUsername,
                    Content = review.Content,
                    NumberOfStars = review.NumberOfStars,
                    Date = review.CreationTime.ToShortDateString(),
                });
            }

            return resultDTOs;
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

        // TODO: Get reviews of certain User (to show in his profile)
    }
}
