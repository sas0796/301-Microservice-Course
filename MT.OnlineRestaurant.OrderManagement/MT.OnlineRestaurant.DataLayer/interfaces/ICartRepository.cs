#region References
using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Context;
#endregion

#region namespaces
namespace MT.OnlineRestaurant.DataLayer.interfaces
{
    #region Interface Definition
    /// <summary>
    /// Defines data actions for booking the table
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Gets the cart details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IList<TblCartItems> GetCartDetails(int customerId, out int restaurantId);

        /// <summary>
        /// Inserts cart master information
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        bool InsertCartMaster(int customerId, int restaurantId, out int cartMasterId);

        /// <summary>
        /// Updates cart master information
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        bool UpdateCartMaster(int cartMasterId);

        /// <summary>
        /// Removes cart master information
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        bool RemoveCartMaster(int customerId, int restaurantId);

        /// <summary>
        /// Inserts cart items information
        /// </summary>
        /// <param name="tblCartItems"></param>
        /// <returns></returns>
        bool InsertCartItems(int cartMasterId, InsertCartDetails cartItem);

        /// <summary>
        /// Updates cart items information
        /// </summary>
        /// <param name="tblCartItems"></param>
        /// <returns></returns>
        bool UpdateCartItems(InsertCartDetails updateCartDetails);

        /// <summary>
        /// Removes cart items information
        /// </summary>
        /// <param name="tblCartItems"></param>
        /// <returns></returns>
        bool RemoveCartItems(int customerId, int restaurantId, int menuId);
        /// <summary>
        /// Updates IsItemsAvailable to false for all rows in cart items having the provided restaurantId & menuId
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        bool UpdateCartItemsForOutOfStock(int restaurantId, int? menuId);
    }
    #endregion
}
#endregion
