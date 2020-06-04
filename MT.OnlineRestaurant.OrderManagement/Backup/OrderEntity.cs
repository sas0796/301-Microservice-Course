using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class OrderEntity
    {
        public int Restaurant_Id { get; set; }
        public int Customer_Id { get; set; }
        public List<OrderMenus> OrderMenuDetails { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
