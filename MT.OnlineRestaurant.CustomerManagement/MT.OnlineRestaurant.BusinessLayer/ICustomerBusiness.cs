using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer
{
    /// <summary>
    /// Customer business related operations
    /// </summary>
    public interface ICustomerBusiness
    {
        int UserRegisteration(CustomerDetails userDetails);

        IQueryable<CustomerDetails> GetCustomerDetails();

        int UpdateCustomerDetails(CustomerDetails customerDetails);

        /// <summary>
        /// Deactive Customer Details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        int DeactivateCustomer(int customerId, bool isActive);
        int UpdateFromReceivedMessage(string CustomerID);
    }
}
