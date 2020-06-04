using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class CartDetails
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public double TotalPrice { get; set; }
        public IList<CartItems> CartItems { get; set; }
    }
}
