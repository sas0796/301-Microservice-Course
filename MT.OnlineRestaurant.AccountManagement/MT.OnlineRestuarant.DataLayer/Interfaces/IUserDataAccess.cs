using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.Interfaces
{
    public interface IUserDataAccess
    { 

        TblCustomer UserLogin(string username, string password);
    }
}
