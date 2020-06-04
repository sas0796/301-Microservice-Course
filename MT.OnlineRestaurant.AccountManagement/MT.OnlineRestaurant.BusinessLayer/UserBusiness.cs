using MT.OnlineRestaurant.BusinessLayer.Interfaces;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.Interfaces;
using System;

namespace MT.OnlineRestaurant.BusinessLayer
{
   
        /// <summary>
        /// Data Access Layer User
        /// </summary>
        public class UserBusiness : IUserBusiness
        {
            private readonly IUserDataAccess _userDataAccess;

            public UserBusiness(IUserDataAccess userDataAccess)
            {
            _userDataAccess = userDataAccess;
            }



            public TblCustomer UserLogin(string username, string password)
            {
               return _userDataAccess.UserLogin(username, password);
            }
        }
}
