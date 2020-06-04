using AutoMapper;
using MessengerManagement;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessEntities.ServiceModels;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.interfaces;
using MT.OnlineRestaurant.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class PlaceOrderActions : IPlaceOrderActions
    {
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly IPlaceOrderDbAccess _placeOrderDbAccess;
        private readonly ICartRepository _cartRepository;
        private readonly IMessenger _messenger;
        private readonly IOptions<ConnectionStrings> _connectionStrings;

        public PlaceOrderActions()
        {
            
        }

        public PlaceOrderActions(IPlaceOrderDbAccess placeOrderDbAccess)
        {
            _placeOrderDbAccess = placeOrderDbAccess;
        }

        public PlaceOrderActions(IPlaceOrderDbAccess placeOrderDbAccess, IMapper mapper, ICartRepository cartRepository, IMessenger messenger)
        {
            _placeOrderDbAccess = placeOrderDbAccess;
            _mapper = mapper;
            _cartRepository = cartRepository;
            _messenger = messenger;
        }

        /// <summary>
        /// Place order
        /// </summary>
        /// <param name="orderEntity">Order details</param>
        /// <returns>order id</returns>
        public async Task<int> PlaceOrder(OrderEntity orderEntity)
        {
            try
            {
                DataLayer.Context.TblFoodOrder tblFoodOrder = _mapper.Map<DataLayer.Context.TblFoodOrder>(orderEntity);

                IList<DataLayer.Context.TblFoodOrderMapping> tblFoodOrderMappings = new List<DataLayer.Context.TblFoodOrderMapping>();
                List<CartItemsSenderDetails> orderPlacedItems = new List<CartItemsSenderDetails>();

                foreach (OrderMenus orderMenu in orderEntity.OrderMenuDetails)
                {
                    //Mapping to DbContext model
                    tblFoodOrderMappings.Add(new DataLayer.Context.TblFoodOrderMapping()
                    {
                        TblFoodOrderId = 0,
                        TblMenuId = orderMenu.MenuId,
                        Price = orderMenu.Price,
                        Active = true,
                        UserCreated = 0,
                        RecordTimeStampCreated = DateTime.Now
                    });
                    //Mapping to business model to use for pre-validation
                    orderPlacedItems.Add(new CartItemsSenderDetails()
                    {
                        RestaurantId = orderEntity.RestaurantId,
                        MenuId = orderMenu.MenuId,
                        Quantity = orderMenu.Quantity
                    });
                }

                //Validation for Restaurant Id & Menu Id && item's quantity available or not
                foreach(var item in orderPlacedItems)
                {
                    CartItems cartItem;
                    string serializedPlacedOrderItems = JsonConvert.SerializeObject(item);
                    using (HttpClient httpClient = new HttpClient())
                    {
                        StringContent stringContent = new StringContent(serializedPlacedOrderItems, Encoding.UTF8, "application/json");
                        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"http://localhost:10601/api/InsertDetailsCheck/", stringContent);
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string json = await httpResponseMessage.Content.ReadAsStringAsync();
                            cartItem = JsonConvert.DeserializeObject<CartItems>(json);
                        }
                        else
                            throw new Exception($"No menu present for Restaurant {orderEntity.RestaurantId} and Menu {item.MenuId}");
                    }

                    if (!cartItem.IsItemAvailable)
                        throw new Exception($"Cannot add {item.Quantity} for Menu {item.MenuId}. Atmost {cartItem.Quantity} can be added.");
                }

                //Saving data for Food Order master table
                int tblFoodOrderId = _placeOrderDbAccess.PlaceOrder(tblFoodOrder);

                if (tblFoodOrderId < 1)
                    throw new Exception("Error in placing an order");

                foreach (var tblFoodOrderMapping in tblFoodOrderMappings)
                    tblFoodOrderMapping.TblFoodOrderId = tblFoodOrderId;

                //Saving Food order item details
                bool result = _placeOrderDbAccess.PlaceOrderMapping(tblFoodOrderMappings);

                if (!result)
                    throw new Exception("Error in placing order for food items");

                //Using service bus to update database in Search service's table
                foreach (var item in orderPlacedItems)
                {
                    string json = JsonConvert.SerializeObject(item);
                    await _messenger.SendMessageAsync(json);
                }

                //Checking & dis-activating the cartMaster Id, if there's any
                if (orderEntity.CartMasterId > 0)
                    _cartRepository.UpdateCartMaster(1);

                return tblFoodOrderId;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Cancel Order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        public int CancelOrder(int orderId)
        {
            return (orderId > 0 ? _placeOrderDbAccess.CancelOrder(orderId) : 0);
        }

        /// <summary>
        /// gets the customer placed order details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable<CustomerOrderReport> GetReports(int customerId)
        {
            var foodOrders = _placeOrderDbAccess.GetReports(customerId);
            if (foodOrders.Any())
            {
                return foodOrders.Select(x => new CustomerOrderReport
                {
                    OrderedDate = x.RecordTimeStampCreated,
                    OrderStatus = x.TblOrderStatus.Status,
                    OrderId = x.Id,
                    PaymentStatus = x.TblOrderPayment.Any() ? x.TblOrderPayment.FirstOrDefault().TblPaymentStatus.Status : string.Empty,
                    price = x.TotalPrice
                }).AsQueryable();
            }

            return null;
        }

        public async Task<bool> IsValidRestaurantAsync(OrderEntity orderEntity, int UserId, string UserToken)
        {
            using (HttpClient httpClient = WebAPIClient.GetClient(UserToken, UserId, _connectionStrings.Value.RestaurantApiUrl))
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("api/ResturantDetail?RestaurantID=" + orderEntity.RestaurantId);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    RestaurantInformation restaurantInformation = JsonConvert.DeserializeObject<RestaurantInformation>(json);
                    if(restaurantInformation != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<bool> IsOrderItemInStock(OrderEntity orderEntity, int UserId, string UserToken)
        {
            //using (HttpClient httpClient = WebAPIClient.GetClient(UserToken, UserId, _connectionStrings.Value.RestaurantApiUrl))
            using(HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("http://localhost:10601/api/OrderDetail?RestaurantID=" + orderEntity.RestaurantId);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    RestaurantInformation restaurantInformation = JsonConvert.DeserializeObject<RestaurantInformation>(json);
                    if (restaurantInformation != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public async Task<bool> IsValidRestaurant(OrderEntity orderEntity)
        //{
        //    using (HttpClient httpClient = WebAPIClient.GetClient(orderEntity.UserToken, orderEntity.UserId, "http://localhost:10601/"))
        //    {
        //        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("api/ResturantMenuDetail?RestaurantID=" + orderEntity.RestaurantId);
        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            string json = await httpResponseMessage.Content.ReadAsStringAsync();
        //            IQueryable<RestaurantMenu> restaurantInformation = JsonConvert.DeserializeObject<IQueryable<RestaurantMenu>>(json);
        //            if (restaurantInformation != null)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
