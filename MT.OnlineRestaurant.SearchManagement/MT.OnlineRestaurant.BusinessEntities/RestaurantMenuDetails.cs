using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class RestaurantMenuDetails
    {
        public string cuisine { get; set; }
        public List<RestaurantMenu> menuList { get; set; }
    }
}
