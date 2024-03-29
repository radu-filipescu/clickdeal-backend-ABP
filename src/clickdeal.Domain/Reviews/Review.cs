﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace clickdeal.Reviews
{
    public class Review : AuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public string SmartbillId { get; set; }
        public string ReviewUsername { get; set; } = string.Empty;
        public int NumberOfStars { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Approved { get; set; }

        public Review() {
            Title = "";
            Content = "";
        }

    }
}
