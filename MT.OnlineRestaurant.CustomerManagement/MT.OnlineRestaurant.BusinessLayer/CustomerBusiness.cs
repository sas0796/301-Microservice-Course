using AutoMapper;
using Microsoft.Extensions.Configuration;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class CustomerBusiness : ICustomerBusiness
    {
        public ICustomerRepository _customerRepository { get; set; }
        private IConfiguration _configuration { get; set; }

        private const string CustomerCacheKey = "customers";

        ICache _cacheManager;

        public IMapper _Mapper { get; set; }
        public CustomerBusiness(IMapper Mapper, ICustomerRepository customerRepository, ICache cacheManager)
        {
            _customerRepository = customerRepository;
            _cacheManager = cacheManager;
            _Mapper = Mapper;
        }
        public int UserRegisteration(CustomerDetails user)
        {
            var userDetails = _Mapper.Map<TblCustomer>(user);
            int result = -1;
            {
                byte[] passwordHash, passwordSalt;
                Helper.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
                userDetails.Password = passwordHash.ToString();//change ada
                result = _customerRepository.UserRegisteration(userDetails, passwordSalt);
            }
            return result;
        }

        public int DeactivateCustomer(int customerId, bool isActive)
        {
            return _customerRepository.DeactivateCustomer(customerId, isActive);

        }
        public int UpdateFromReceivedMessage(string msg)
        {
           return _customerRepository.UpdateFromReceivedMessage(Convert.ToInt32(msg));
        }
        private List<CustomerDetails> GetCustomerFromCache()
        {
            List<CustomerDetails> customerList;

            try
            {
                customerList = _cacheManager.GetFromCache<List<CustomerDetails>>(CustomerCacheKey);
                if (customerList == null)
                {
                    customerList = GetCustomerDetailsFromDb().ToList();
                    _cacheManager.SetInCache<List<CustomerDetails>>(CustomerCacheKey, customerList);
                }

            }
            catch (Exception ex)
            {
                customerList = GetCustomerDetailsFromDb().ToList();
            }
            return customerList;

        }

        public IQueryable<CustomerDetails> GetCustomerDetailsFromDb()
        {
            List<CustomerDetails> customerDetails = new List<CustomerDetails>();
            var details = _customerRepository.GetCustomerDetails();
            foreach (var item in details)
            {
                var customer = _Mapper.Map<CustomerDetails>(item);
                customerDetails.Add(customer);
            }
            return customerDetails.AsQueryable();
        }
        public IQueryable<CustomerDetails> GetCustomerDetails()
        {
            return GetCustomerFromCache().AsQueryable();

            /*
            List<CustomerDetails> customerDetails = new List<CustomerDetails>();
            var details = _customerRepository.GetCustomerDetails();
            foreach (var item in details)
            {
                var customer = _Mapper.Map<CustomerDetails>(item);
                customerDetails.Add(customer);
            }
            return customerDetails.AsQueryable();
            */
        }

        public CustomerDetails GetCustomer(int customerId)
        {
            CustomerDetails customer = null;
            var details = GetCustomerFromCache().FirstOrDefault(c => c.Id == customerId);

            //var details = _customerRepository.GetCustomerDetails().FirstOrDefault(c => c.Id == customerId);
            if (details != null)
            {
                customer = _Mapper.Map<CustomerDetails>(details);
            }

            return customer;
        }
        public int UpdateCustomerDetails(CustomerDetails customerDetails)
        {

            var userDetails = _Mapper.Map<TblCustomer>(customerDetails);
            /*update user PAssword*/
            if (!string.IsNullOrWhiteSpace(customerDetails.Password))
            {
                byte[] passwordHash, passwordSalt;
                Helper.CreatePasswordHash(customerDetails.Password, out passwordHash, out passwordSalt);
                userDetails.Password = passwordHash.ToString();
            }

            _cacheManager.InValidateCache(CustomerCacheKey);

            return _customerRepository.UpdateCustomerDetails(userDetails);

        }
    }
}


