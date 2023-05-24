using System;
using System.Collections.Generic;
using System.Text;

namespace clickdeal.ProductsStock
{
    public class CreateUpdateProductStockDTO 
    {
        public Guid ProductId { get; set; }
        public string ProductSpecs { get; set; } = string.Empty;
        public int TotalUnits { get; set; }
    }
}
