using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace clickdeal.Products
{
    public class FilteredProductsRequestDTO : PagedAndSortedResultRequestDto
    {
        public int? PriceMin { get; set; }
        public int? PriceMax { get; set; }

        // single category for now
        public string? Category { get; set; }
        public string? Brand { get; set; }

        // expects something from:
        // 1. DATE-ASC
        // 2. DATE-DESC
        // 3. PRICE-ASC
        // 4. PRICE-DESC
        public string? OrderBy { get; set; }

    }
}
