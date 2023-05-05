using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace clickdeal.Categories
{
    public class Category : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public int ProductsNumber { get; set; }

        public string PhotoBase64 { get; set; } = string.Empty;

        // will hold subcategories GUIDs separated by #
        public string Subcategories { get; set; } = string.Empty;

        public Guid? ParentGuid { get; set; } = null;

        public bool Visible { get; set; } = true;
    }
}
