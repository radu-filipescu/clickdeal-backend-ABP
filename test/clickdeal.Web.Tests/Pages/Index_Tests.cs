using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace clickdeal.Pages;

public class Index_Tests : clickdealWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
