using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.DataEntity
{
    public class RestaurantSearchDetails
    {
        public int restauran_ID { get; set; }
        public string restaurant_Name { get; set; }
        public string restaurant_Address { get; set; }
        public string restaurant_PhoneNumber { get; set; }
        public string restraurant_Website { get; set; }
        public string opening_Time { get; set; }
        public string closing_Time { get; set; }
        public double xaxis { get; set; }
        public double yaxis { get; set; }

    }
}
