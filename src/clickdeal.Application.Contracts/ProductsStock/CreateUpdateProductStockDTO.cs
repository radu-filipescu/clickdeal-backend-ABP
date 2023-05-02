using System;
using System.Collections.Generic;
using System.Text;

namespace clickdeal.ProductsStock
{
    public class CreateUpdateProductStockDTO 
    {
        public Guid ProductId { get; set; }
        public int ReservedUnits { get; set; }
        public int TotalUnits { get; set; }
    }
}
