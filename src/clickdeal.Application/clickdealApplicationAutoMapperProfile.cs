using AutoMapper;
using clickdeal.Reviews;

namespace clickdeal;

public class clickdealApplicationAutoMapperProfile : Profile
{
    public clickdealApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Review, ReviewDTO>();
        CreateMap<CreateUpdateReviewDTO, Review>();
    }
}
