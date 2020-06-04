using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Linq;

namespace MT.OnlineRestaurant.DataLayer
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly IOptions<ApplicationString> _connectionStrings;

        private readonly CustomerManagementContext _context;
        public CustomerRepository(IOptions<ApplicationString> dbSettings)
        {
            _connectionStrings = dbSettings;
            //_context = new CustomerManagementContext(_connectionStrings.Value.DB);
            _context = new CustomerManagementContext("Server=tcp:devserver4.database.windows.net;Initial Catalog=CustomerManagement;User ID=M1043027;Password=Azuredb1@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
        public int UserRegisteration(TblCustomer user,byte[] passwordSalt)
        {
            _context.TblCustomer.Add(new TblCustomer
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                PasswordKey= passwordSalt.ToString(),
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Address = user.Address,
                UserCreated = 2,
                RecordTimeStamp=DateTime.Now,
                Active = true
            });
            return _context.SaveChanges();

        }
        public IQueryable<TblCustomer> GetCustomerDetails()
        {
            return _context.TblCustomer;
        }

        /// <summary>
        /// Update Customer Details
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns></returns>
        public int UpdateCustomerDetails(TblCustomer customerDetails)
        {
            var user = _context.TblCustomer.Find(customerDetails.Id);

            if (user == null)
                throw new Exception("User not found");

            if (customerDetails.Email != user.Email)
            {
                // username has changed so check if the new username is already taken
                if (_context.TblCustomer.Any(x => x.Email == customerDetails.Email))
                    throw new Exception("Username " + customerDetails.Email + " is already taken");
            }

            // update user properties
            user.FirstName = customerDetails.FirstName;
            user.LastName = customerDetails.LastName;
            user.Email = customerDetails.Email;
            user.Password = customerDetails.Password;
            user.Address = customerDetails.Address;
            user.MobileNumber = customerDetails.MobileNumber;
            user.RecordTimeStamp = DateTime.Now;

            // update password if it was entered
            _context.TblCustomer.Update(user);
            return _context.SaveChanges();
        }
        public int UpdateFromReceivedMessage(int CustomerID)
        {
          
                var user = _context.TblCustomer.Find(CustomerID);
                user.Totalorders = user.Totalorders + 1;
                _context.TblCustomer.Update(user);
                return _context.SaveChanges();
        }
        /// <summary>
        /// Update Customer Details
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns></returns>
        public int DeactivateCustomer(int customerId,bool isActive)
        {
            var user = _context.TblCustomer.Find(customerId);

            if (user == null)
                throw new Exception("User not found");

            // update user properties
            user.Active = isActive;
            user.RecordTimeStamp = DateTime.Now;
            // update password if it was entered
            _context.TblCustomer.Update(user);
           return _context.SaveChanges();         
        }
    }
}
