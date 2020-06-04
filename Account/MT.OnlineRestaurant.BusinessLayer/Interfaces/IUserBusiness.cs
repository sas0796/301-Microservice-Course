using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
       
            TblCustomer UserLogin(string username, string password);
    }
}
