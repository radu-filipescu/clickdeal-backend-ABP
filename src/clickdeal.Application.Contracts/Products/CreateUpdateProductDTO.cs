﻿using System;
using System.Collections.Generic;
using System.Text;

namespace clickdeal.Products
{
    public class CreateUpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public string Image { get; set; } = string.Empty;

        public string DescriptionShort { get; set; } = string.Empty;

        public string DescriptionLong { get; set; } = string.Empty;

        // information table with this convention  key:value  title*
        public string Information { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        // all the product's categories names, starting and ending with #, and separated by #
        public string Categories { get; set; } = string.Empty;

        // serialized JSON holding different characteristics of the product
        public string Specs { get; set; } = string.Empty;
    }
}
