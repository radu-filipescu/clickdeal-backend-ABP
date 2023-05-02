using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace clickdeal.Reviews
{
    public class ReviewDTO : AuditedEntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NumberOfStars { get; set; }

        public ReviewDTO() { 
            Title = string.Empty;
            Content = string.Empty;
        }
    }
}
