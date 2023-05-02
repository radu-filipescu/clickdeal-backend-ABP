using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace clickdeal.ProductStocks
{
    public class ProductStock : AuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        public int TotalUnits { get; set; }
        public int ReservedUnits { get; set; }
    }
}
