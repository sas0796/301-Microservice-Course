using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class CartItems
    {
        public string MenuName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsItemAvailable { get; set; }
    }
}
