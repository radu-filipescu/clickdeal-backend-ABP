using System;
using System.Collections.Generic;
using System.Text;

namespace clickdeal.Categories
{
    public class CreateUpdateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;

        public string PhotoBase64 { get; set; } = string.Empty;

        // will hold subcategories GUIDs separated by #
        public string Subcategories { get; set; } = string.Empty;

        public Guid? ParentGuid { get; set; } = null;
    }
}
