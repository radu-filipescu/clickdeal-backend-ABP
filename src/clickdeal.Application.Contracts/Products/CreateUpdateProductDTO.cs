﻿using System;
using System.Collections.Generic;
using System.Text;

namespace clickdeal.Products
{
    public class CreateUpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; } = string.Empty;

        // all the product's categories, separated by #
        public string Categories { get; set; } = string.Empty;

        // serialized JSON holding different characteristics of the product
        public string Specs { get; set; } = string.Empty;
    }
}