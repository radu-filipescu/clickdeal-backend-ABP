using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace clickdeal.Reviews
{
    public class CreateUpdateReviewDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int NumberOfStars { get; set; }

        public Guid ProductId { get; set; }
    }
}
