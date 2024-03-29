﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace clickdeal.Products
{
    public class ProductDTO : AuditedEntityDto<Guid>
    {
        public string CodIdentificareSmartbill { get; set; } = string.Empty;

        public string SmartbillName { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public double PriceDiscount { get; set; }

        public string ProductId { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string DescriptionShort { get; set; } = string.Empty;

        public string DescriptionLong { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        // all the product's categories, separated by #
        public string Categories { get; set; } = string.Empty;

        // serialized JSON holding different characteristics of the product
        public string Specs { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public bool IsVisible { get; set; }
    }
}
