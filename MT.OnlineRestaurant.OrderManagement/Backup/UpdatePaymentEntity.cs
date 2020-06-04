using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class UpdatePaymentEntity : PaymentEntity
    {
        public int Payment_Id { get; set; }
        public string Transaction_Id { get; set; }
        public int PaymentStatus_Id { get; set; }
    }
}
