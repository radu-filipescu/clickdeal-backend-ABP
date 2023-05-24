using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clickdeal
{
    public class ShoppingCartDTO
    {
        public List<ShoppingCartEntry> Entries { get; set; } = new List<ShoppingCartEntry>();

        public string Coupon { get; set; } = string.Empty;

        // mandatory for both guests and logged users
        public string OrderEmail { get; set; } = string.Empty;

        // if logged in
        public string? Username { get; set; } = string.Empty;
    }


    public class ShoppingCartEntry
    {
        public string ProductId { get; set; } = string.Empty;

        public string Specs { get; set; } = string.Empty;

        public int Quantity { get; set; }

    }
}
