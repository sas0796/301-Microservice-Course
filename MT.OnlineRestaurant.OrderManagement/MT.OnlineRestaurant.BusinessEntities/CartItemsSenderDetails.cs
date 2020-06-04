using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class CartItemsSenderDetails
    {
        public int RestaurantId { get; set; }
        public int? MenuId { get; set; }
        public int Quantity { get; set; }
    }
}
