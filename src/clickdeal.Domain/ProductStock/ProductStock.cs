using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace clickdeal.ProductStocks
{
    public class ProductStock : AuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        public string ProductSpecs { get; set; } = string.Empty;
        public int TotalUnits { get; set; }
        public bool IsDeleted { get; set; }
    }
}
