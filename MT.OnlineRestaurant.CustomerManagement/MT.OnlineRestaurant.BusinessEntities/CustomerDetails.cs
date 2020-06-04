using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MT.OnlineRestaurant.BusinessEntities
{
    /// <summary>
    /// Customer Details
    /// </summary>
    public class CustomerDetails 
    {
        public int Id { get; set; }
  
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }


    }
}
