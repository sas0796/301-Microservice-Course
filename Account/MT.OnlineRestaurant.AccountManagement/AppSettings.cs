using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT.OnlineRestaurant.AccountManagement
{
    public class AppSettings
    {
        public string SecretKey { get; set; }

        public string IssuerKey { get; set; }

        public string ExpiryTime { get; set; }

    }
}
