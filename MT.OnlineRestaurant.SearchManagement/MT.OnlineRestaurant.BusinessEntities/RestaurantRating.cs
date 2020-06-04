using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class RestaurantRating
    {
        public int RestaurantId { get; set; }
        public string rating { get; set; }
        public string user_Comments { get; set; }
        public int customerId { get; set; }

    }
}
