using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class PaymentEntity
    {
        public int Order_Id { get; set; }
        public int PaymentType_Id { get; set; }
        public string Remarks { get; set; }
        public int Customer_Id { get; set; }
    }
}
