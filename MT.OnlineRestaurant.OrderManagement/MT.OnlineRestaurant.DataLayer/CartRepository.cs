using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.interfaces;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.DataLayer
{
    public class CartRepository : ICartRepository
    {
        #region Private Variables
        private readonly OrderManagementContext _orderManagementContext;
        private readonly IOptions<ConnectionStrings> _connectionStrings;
        #endregion

        #region Constructor
        public CartRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings;
            _orderManagementContext = new OrderManagementContext(_connectionStrings.Value.DatabaseConnectionString);
        }
        #endregion

        #region Interface Methods
        /// <summary>
        /// Gets the cart items based on the customerId provided
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IList<TblCartItems> GetCartDetails(int customerId, out int restaurantId)
        {
            try
            {
                if (!DoesCartMasterExists(customerId))
                    throw new Exception("No items are there in your cart. Continue shopping");
                int tblCartMasterId = (from cartMaster in _orderManagementContext.TblCartMaster
                                       where cartMaster.TblCustomerId == customerId
                                       && cartMaster.IsActive
                                       select cartMaster.Id).FirstOrDefault();

                restaurantId = (from cartMaster in _orderManagementContext.TblCartMaster
                                       where cartMaster.TblCustomerId == customerId
                                       && cartMaster.IsActive
                                       select cartMaster.TblRestaurantId).FirstOrDefault();

                if (!DoesCartItemsExists(tblCartMasterId))
                    throw new Exception("Error in fetching cart items");

                IList<TblCartItems> tblCartItems = (from cartItems in _orderManagementContext.TblCartItems
                                             where cartItems.TblCartMasterId == tblCartMasterId
                                             select cartItems).ToList();

                return tblCartItems;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Insert Cart Items data
        /// </summary>
        /// <param name="cartMasterId"></param>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        public bool InsertCartItems(int cartMasterId, InsertCartDetails cartItem)
        {
            try
            {
                if (cartMasterId == 0)
                    cartMasterId = (from cartMaster in _orderManagementContext.TblCartMaster
                                    where cartMaster.TblCustomerId == cartItem.CustomerId &&
                                    cartMaster.TblRestaurantId == cartItem.RestaurantId && cartMaster.IsActive
                                    select cartMaster.Id).FirstOrDefault();

                if (cartMasterId == 0)
                    throw new Exception("Error adding cart item. Try again later");

                bool duplicateMenuCheck = (from cartItems in _orderManagementContext.TblCartItems
                                           where cartItems.TblCartMasterId == cartMasterId &&
                                           cartItems.TblMenuId == cartItem.MenuId
                                           select cartItems.Id).Count() > 0;
                if (duplicateMenuCheck)
                    throw new Exception($"Menu {cartItem.MenuId} for Restaurant {cartItem.RestaurantId} already exists! Try updating the menu using the " +
                        $"update endpoint.");

                TblCartItems tblCartItems = new TblCartItems()
                {
                    TblCartMasterId = cartMasterId,
                    TblMenuId = cartItem.MenuId,
                    Quantity = cartItem.Quantity,
                    IsItemAvailable = true,
                    RecordTimeStampCreated = DateTime.Now
                };

                _orderManagementContext.Add(tblCartItems);

                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Insert Cart Master data
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        public bool InsertCartMaster(int customerId, int restaurantId, out int cartMasterId)
        {
            try
            {
                bool cartMasterExists = DoesCartMasterExists(customerId);
                cartMasterId = 0;

                var tblCartMaster = (from cartMaster in _orderManagementContext.TblCartMaster
                                     where cartMaster.TblCustomerId == customerId && 
                                     cartMaster.TblRestaurantId == restaurantId && cartMaster.IsActive
                                     select cartMaster).FirstOrDefault();
                if (tblCartMaster != null)
                {
                    cartMasterId = tblCartMaster.Id;
                    return false;
                }

                if (tblCartMaster == null && cartMasterExists)
                    throw new Exception($"Cannot add menu for restaurant {restaurantId}! Cart contains items from different restaurant");

                TblCartMaster newCartMaster = new TblCartMaster()
                {
                    TblCustomerId = customerId,
                    TblRestaurantId = restaurantId,
                    IsActive = true,
                    RecordTimeStampCreated = DateTime.Now
                };

                _orderManagementContext.Add(newCartMaster);

                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Removing the cart item
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool RemoveCartItems(int customerId, int restaurantId, int menuId)
        {
            try
            {
                if (!DoesCartMasterExists(customerId))
                    throw new Exception("No items are there in your cart. Continue shopping");

                int tblCartMasterId = (from cartMaster in _orderManagementContext.TblCartMaster
                                       where cartMaster.TblCustomerId == customerId
                                       && cartMaster.TblRestaurantId == restaurantId && cartMaster.IsActive
                                       select cartMaster.Id).FirstOrDefault();

                if(tblCartMasterId == 0)
                    throw new Exception($"Cannot remove menu for restaurant {restaurantId}! Cart contains items from different restaurant");

                if (!DoesCartItemsExists(tblCartMasterId))
                    throw new Exception("Error in fetching cart items");

                TblCartItems tblCartItem = (from cartItems in _orderManagementContext.TblCartItems
                                            where cartItems.TblCartMasterId == tblCartMasterId
                                            && cartItems.TblMenuId == menuId
                                            select cartItems).FirstOrDefault();

                if (tblCartItem != null)
                    throw new Exception($"Menu with id {menuId} not found for the customer id {customerId}");

                _orderManagementContext.Remove(tblCartItem);
                return SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Removing the cart master details when the cart is empty
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool RemoveCartMaster(int customerId, int restaurantId)
        {
            try
            {
                if (!DoesCartMasterExists(customerId))
                    throw new Exception("No items are there in your cart. Continue shopping");

                var tblCartMaster = (from cartMaster in _orderManagementContext.TblCartMaster
                                       where cartMaster.TblCustomerId == customerId
                                       && cartMaster.TblRestaurantId == restaurantId && cartMaster.IsActive
                                       select cartMaster).FirstOrDefault();

                if (tblCartMaster == null)
                    throw new Exception($"Cannot remove menu for restaurant {restaurantId}! Cart contains items from different restaurant");

                if (!DoesCartItemsExists(tblCartMaster.Id))
                    _orderManagementContext.Remove(tblCartMaster);

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updating cart items details
        /// </summary>
        /// <param name="tblCartItems"></param>
        /// <returns></returns>
        public bool UpdateCartItems(InsertCartDetails updateCartDetails)
        {
            try
            {
                if (!DoesCartMasterExists(updateCartDetails.CustomerId))
                    throw new Exception("No items are there in your cart. Continue shopping");

                int tblCartMasterId = (from cartMaster in _orderManagementContext.TblCartMaster
                                       where cartMaster.TblCustomerId == updateCartDetails.CustomerId
                                       && cartMaster.TblRestaurantId == updateCartDetails.RestaurantId && cartMaster.IsActive
                                       select cartMaster.Id).FirstOrDefault();

                if (tblCartMasterId == 0)
                    throw new Exception($"Cannot update menu for restaurant {updateCartDetails.RestaurantId}! Cart contains items from different restaurant");

                if (!DoesCartItemsExists(tblCartMasterId))
                    throw new Exception("Error in fetching cart items");

                TblCartItems tblCartItem = (from cartItems in _orderManagementContext.TblCartItems
                                            where cartItems.TblCartMasterId == tblCartMasterId
                                            && cartItems.TblMenuId == updateCartDetails.MenuId
                                            select cartItems).FirstOrDefault();

                if (tblCartItem == null)
                    throw new Exception($"Menu with id {updateCartDetails.MenuId} not found for the customer id {updateCartDetails.CustomerId}");

                tblCartItem.Quantity = updateCartDetails.Quantity;

                return SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updating cart master details
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        public bool UpdateCartMaster(int cartMasterId)
        {
            try
            {
                var tblCartMaster = (from cartMaster in _orderManagementContext.TblCartMaster
                                       where cartMaster.Id == cartMasterId
                                       && cartMaster.IsActive
                                       select cartMaster).FirstOrDefault();
                if (tblCartMaster == null)
                    throw new Exception("Cart Master Id is not valid");

                tblCartMaster.IsActive = false;

                return SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updates IsItemsAvailable to false for all rows in cart items having the provided restaurantId & menuId
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool UpdateCartItemsForOutOfStock(int restaurantId, int? menuId)
        {
            try
            {
                List<TblCartItems> tblCartItems = (from cartMaster in _orderManagementContext.TblCartMaster
                                                   join cartItems in _orderManagementContext.TblCartItems on cartMaster.Id equals cartItems.TblCartMasterId
                                                   where cartMaster.TblRestaurantId == restaurantId && cartMaster.IsActive && cartItems.TblMenuId == menuId
                                                   select cartItems).ToList();

                foreach (var item in tblCartItems)
                {
                    item.IsItemAvailable = false;
                    _orderManagementContext.SaveChanges();
                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Private Methods
        private bool SaveChanges()
        {
            try
            {
                _orderManagementContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool DoesCartMasterExists(int customerId)
        {
            bool IsAvailable = (from cartMaster in _orderManagementContext.TblCartMaster
                                where cartMaster.TblCustomerId == customerId
                                && cartMaster.IsActive
                                select cartMaster).Count() > 0;
            return IsAvailable;
        }

        private bool DoesCartItemsExists(int cartMasterId)
        {
            bool IsAvailable = (from cartItems in _orderManagementContext.TblCartItems
                                where cartItems.TblCartMasterId == cartMasterId
                                select cartItems).Count() > 0;
            return IsAvailable;
        }
        #endregion
    }
}
