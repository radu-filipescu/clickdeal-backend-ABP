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
        public bool AreSpecsEqual(string specs1, string specs2)
        {
            if (specs1 == string.Empty && specs2 == string.Empty)
                return true;

            var obj1 = JsonConvert.DeserializeObject<Dictionary<string, string>>(specs1);
            var obj2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(specs2);


            if (obj1 == null || obj2 == null)
                return false;

            if (obj1.Equals(obj2)) return true;

            return false;
        }
    }
}
