using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class InsertCartDetails
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }
    }
}
