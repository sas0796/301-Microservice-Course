using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Registering User
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns>Success or not</returns>
        int UserRegisteration(TblCustomer customerDetails, byte[] passwordSalt);

        /// <summary>
        /// Get Customer Details
        /// </summary>
        /// <returns>List of Customer Details</returns>
        IQueryable<TblCustomer> GetCustomerDetails();
        /// <summary>
        /// Update Customer Details
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns>Success or not</returns>
        int UpdateCustomerDetails(TblCustomer customerDetails);

        /// <summary>
        /// Deactive Customer Details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        int DeactivateCustomer(int customerId, bool isActive);

        int UpdateFromReceivedMessage(int CustomerId);
    }
}
