using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace clickdeal.ProductsStock
{
    public class ProductStockDTO : AuditedEntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public int TotalUnits { get; set; }
        public int ReservedUnits { get; set; }
    }
}
