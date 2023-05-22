using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace clickdeal.Reviews
{
    public class CreateUpdateReviewDTO
    {
        public string ReviewUsername { get; set; } = string.Empty;
        
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int NumberOfStars { get; set; }

        public Guid ProductId { get; set; }
    }
}
