using System;
using System.Threading.Tasks;
using clickdeal.Reviews;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore;
public class ReviewDataSeeder
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Review, Guid> _reviewRepository;

    public ReviewDataSeeder(IRepository<Review, Guid> reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _reviewRepository.GetCountAsync() <= 0)
        {
            await _reviewRepository.InsertAsync(
                new Review
                {
                    UserId = new Guid(),
                    ProductId = new Guid(),
                    Title = "test review",
                    Content = "this is a seed review, for testing",
                },
                autoSave: true
            );
        }
    }
}