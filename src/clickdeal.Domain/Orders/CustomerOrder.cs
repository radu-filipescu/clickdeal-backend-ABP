using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace clickdeal.Orders
{
    public class CustomerOrder : AggregateRoot<Guid>
    {
        public Guid UserId { get; set; }

        public string Status { get; set; } = string.Empty;

        public ICollection<UserOrderEntry>? OrderEntries { get; set; }

        public double DeliveryCost { get; set; }

        public double TotalCost { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public string BillingAdress { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string TrackingNumber { get; set; } = string.Empty;

        public string ShippingMethod { get; set; } = string.Empty;

        public string CustomerNotes { get; set; } = string.Empty;

        public string PromoCode { get; set; } = string.Empty;
    }
}
