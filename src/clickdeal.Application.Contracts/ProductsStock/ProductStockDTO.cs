using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace clickdeal.ProductsStock
{
    public class ProductStockDTO : AuditedEntityDto<Guid>
    {
        public string ProductId { get; set; }
        public string ProductSpecs { get; set; } = string.Empty;
        public int TotalUnits { get; set; }
    }
}
