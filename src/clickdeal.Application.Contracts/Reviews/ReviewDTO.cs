using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace clickdeal.Reviews
{
    public class ReviewDTO : AuditedEntityDto<Guid>
    {
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public string Date { get; set;} = string.Empty;
        public int NumberOfStars { get; set; }
    }
}
