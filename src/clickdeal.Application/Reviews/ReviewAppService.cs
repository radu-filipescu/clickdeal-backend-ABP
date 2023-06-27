using clickdeal.Permissions;
using clickdeal.Products;
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
using Volo.Abp.Json.SystemTextJson.JsonConverters;
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
        private readonly IRepository<Product> _productRepository;

        public ReviewAppService(IRepository<Review, Guid> repository, IRepository<Product, Guid> productRepository)
        : base(repository)
        {
            _reviewsRepository = repository;
            _productRepository = productRepository;
        }

        [Authorize]
        [IgnoreAntiforgeryToken]
        public async override Task<ReviewDTO> CreateAsync(CreateUpdateReviewDTO input)
        {
            var exists = await _productRepository.FirstOrDefaultAsync(product => product.Id == input.ProductId);

            if (exists == null || input.NumberOfStars < 1 || input.NumberOfStars > 5)
            {
                return new ReviewDTO();
            }

            Review newReview = new Review();

            newReview.ReviewUsername = input.ReviewUsername;
            newReview.SmartbillId = exists.CodIdentificareSmartbill;
            newReview.Title = "";
            newReview.Content = input.Content;
            newReview.NumberOfStars = input.NumberOfStars;
            newReview.ProductId = input.ProductId;
            newReview.Approved = false;

            var result = await _reviewsRepository.InsertAsync(newReview);

            return new ReviewDTO();
        }

        public class ProductReviewsByIdDTO
        {
            public string productId { get; set; } = string.Empty;
        }

        public async Task<IEnumerable<ReviewDTO>?> GetReviewsForProduct(ProductReviewsByIdDTO input)
        {
            Guid productId;
            bool valid = Guid.TryParse(input.productId, out productId);

            if (!valid)
                return null;

            var result = await _reviewsRepository.GetListAsync(review => review.ProductId == productId && review.Approved == true);

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
        public async Task<List<Review>> GetPendingReviews()
        {
            var result = await _reviewsRepository.GetListAsync(review => review.Approved == false);

            return result;
        }

        public class ApproveOrDeleteReviewDTO
        {
            public string ReviewId { get; set; } = string.Empty;
            public bool Approve { get; set; }
            public bool Delete { get; set; }
        }

        [Authorize("clickdeal.Admin")]
        [IgnoreAntiforgeryToken]
        public async Task ApproveOrDeleteReview(ApproveOrDeleteReviewDTO input)
        {
            Guid reviewIdParsed;
            bool valid = Guid.TryParse(input.ReviewId, out reviewIdParsed);

            if (!valid)
                return;

            var result = await _reviewsRepository.FirstOrDefaultAsync(review => review.Id == reviewIdParsed);

            if(input.Approve == true)
            {
                result.Approved = true;
                await _reviewsRepository.UpdateAsync(result);
            }
            else 
                if(input.Delete == true)
                {
                    await _reviewsRepository.DeleteAsync(result);
                }
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
