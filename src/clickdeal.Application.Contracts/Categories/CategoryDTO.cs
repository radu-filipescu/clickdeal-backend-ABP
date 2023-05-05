using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace clickdeal.Categories
{
    public class CategoryDTO : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public int ProductsNumber { get; set; }

        public string PhotoBase64 { get; set; } = string.Empty;

        // will hold actual categories
        public List<CategoryDTO> Subcategories { get; set; } = new List<CategoryDTO>();
    }
}
