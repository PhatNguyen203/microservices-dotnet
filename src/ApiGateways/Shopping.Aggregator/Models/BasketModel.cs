using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }
        public List<BasketItemsModel> Items { get; set; } = new List<BasketItemsModel>();
        public decimal TotalPrice { get; set; }
    }
}
