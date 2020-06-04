#region References
using MessengerManagement;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
#endregion

#region namespace
namespace MT.OnlineRestaurant.BusinessLayer
{
    #region Class Definition
    /// <summary>
    /// Implements the interface methods for booking the table for a restaurant
    /// </summary>
    public class CartBusiness : ICartBusiness
    {
        #region privateVariables
        private readonly ICartRepository _cartRepository;
        private readonly IMessenger _messenger;

        private List<CartItems> lstCartItems = new List<CartItems>();
        #endregion

        public CartBusiness(ICartRepository cartRepository, IMessenger messenger)
        {
            _cartRepository = cartRepository;
            _messenger = messenger;
        }

        #region Interface Methods
        /// <summary>
        /// Gets the cart items based on the customer Id and returns a cart details model
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CartDetails> GetCartDetails(int customerId)
        {
            try
            {
                if (customerId < 1)
                    throw new Exception("Customer Id cannot be less than one");

                IList<TblCartItems> tblCartItems = _cartRepository.GetCartDetails(customerId, out int restaurantId);

                IList<CartItemsSenderDetails> cartSenderItems = new List<CartItemsSenderDetails>();
                foreach(var cartItem in tblCartItems)
                {
                    cartSenderItems.Add(new CartItemsSenderDetails()
                    { 
                        RestaurantId = 0,
                        MenuId = cartItem.TblMenuId,
                        Quantity = cartItem.Quantity
                    });
                }


                string serializedCartSenderItems = JsonConvert.SerializeObject(cartSenderItems);

                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(serializedCartSenderItems, Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"http://localhost:10601/api/CartDetailsCheck/{restaurantId}", stringContent);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        lstCartItems = JsonConvert.DeserializeObject<List<CartItems>>(json);

                    }
                }

                double totalPrice = 0.0;
                foreach (var item in lstCartItems)
                {
                    if(item.IsItemAvailable)
                        totalPrice += item.Price;
                }

                CartDetails cartDetails = new CartDetails()
                {
                    CustomerId = customerId,
                    RestaurantId = restaurantId,
                    CartItems = lstCartItems,
                    TotalPrice = totalPrice
                };

                return cartDetails;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Insert the cart item based on the customerId & menuId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="insertCartDetails"></param>
        /// <returns></returns>
        public async Task<bool> InsertCartDetails(int customerId, InsertCartDetails insertCartDetails)
        {
            try
            {
                if (customerId < 1)
                    throw new Exception("Customer Id cannot be less than one");

                if (customerId != insertCartDetails.CustomerId)
                    throw new Exception("Customer Id in URI doesn't match with the one in Body");

                if (insertCartDetails.RestaurantId < 0)
                    throw new Exception("Restaurant Id cannot be less than one");

                if (insertCartDetails.MenuId < 1)
                    throw new Exception("Menu Id cannot be less than one");

                if (insertCartDetails.Quantity < 1)
                    throw new Exception("Quantity cannot be less than one");


                CartItemsSenderDetails cartSenderItems = new CartItemsSenderDetails()
                {
                    RestaurantId = insertCartDetails.RestaurantId,
                    MenuId = insertCartDetails.MenuId,
                    Quantity = insertCartDetails.Quantity
                };
                string serializedCartSenderItems = JsonConvert.SerializeObject(cartSenderItems);

                CartItems cartItem;
                using (HttpClient httpClient = new HttpClient())
                {
                    
                    StringContent stringContent = new StringContent(serializedCartSenderItems, Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"http://localhost:10601/api/InsertDetailsCheck/", stringContent);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        cartItem = JsonConvert.DeserializeObject<CartItems>(json);
                    }
                    else
                        throw new Exception($"No menu present for Restaurant {insertCartDetails.RestaurantId} and Menu {insertCartDetails.MenuId}");
                }

                if (!cartItem.IsItemAvailable)
                    throw new Exception($"Cannot add {insertCartDetails.Quantity} for Menu {insertCartDetails.MenuId}. Atmost {cartItem.Quantity} can be added.");

                bool result = _cartRepository.InsertCartMaster(insertCartDetails.CustomerId, insertCartDetails.RestaurantId, out int cartMasterId);

                result = _cartRepository.InsertCartItems(cartMasterId, insertCartDetails);                    

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Removing the cart item based on the customerId & menu Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool RemoveCartDetails(int customerId, int restaurantId, int menuId)
        {
            try
            {
                if (customerId < 1)
                    throw new Exception("Customer Id cannot be less than one");

                if (menuId < 1)
                    throw new Exception("Menu Id cannot be less than one");

                bool result = _cartRepository.RemoveCartItems(customerId, restaurantId, menuId);

                if (!result)
                    throw new Exception("Error in removing Menu");

                _cartRepository.RemoveCartMaster(customerId, restaurantId);

                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updates the cart item based on the customerId & menu Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="updateCartDetails"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCartDetails(int customerId, InsertCartDetails updateCartDetails)
        {
            try
            {
                if (customerId < 1)
                    throw new Exception("Customer Id cannot be less than one");

                if (customerId != updateCartDetails.CustomerId)
                    throw new Exception("Customer Id in URI doesn't match with the one in Body");
                
                if (updateCartDetails.RestaurantId < 0)
                    throw new Exception("Restaurant Id cannot be less than one");

                if (updateCartDetails.MenuId < 1)
                    throw new Exception("Menu Id cannot be less than one");

                if (updateCartDetails.Quantity < 1)
                    throw new Exception("Quantity cannot be less than one");

                CartItemsSenderDetails cartSenderItems = new CartItemsSenderDetails()
                {
                    RestaurantId = updateCartDetails.RestaurantId,
                    MenuId = updateCartDetails.MenuId,
                    Quantity = updateCartDetails.Quantity
                };
                string serializedCartSenderItems = JsonConvert.SerializeObject(cartSenderItems);

                CartItems cartItem;
                using (HttpClient httpClient = new HttpClient())
                {

                    StringContent stringContent = new StringContent(serializedCartSenderItems, Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"http://localhost:10601/api/InsertDetailsCheck/", stringContent);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        cartItem = JsonConvert.DeserializeObject<CartItems>(json);
                    }
                    else
                        throw new Exception($"No menu present for Restaurant {updateCartDetails.RestaurantId} and Menu {updateCartDetails.MenuId}");
                }

                if (!cartItem.IsItemAvailable)
                    throw new Exception($"Cannot add {updateCartDetails.Quantity} for Menu {updateCartDetails.MenuId}. Atmost {cartItem.Quantity} can be added.");

                bool result = _cartRepository.UpdateCartItems(updateCartDetails);

                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
    #endregion
}
#endregion
