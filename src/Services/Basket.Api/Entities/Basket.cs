using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Entities
{
    public class Basket
    {
        public string Username { get; set; }
        public IEnumerable<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
        public Basket()
        {

        }
        public Basket(string username)
        {
            Username = username;
        }

        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach(var item in Items)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }
        }
    }
}
