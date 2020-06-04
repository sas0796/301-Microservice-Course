using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public interface IRestaurantBusiness
    {
        RestaurantInformation GetResturantDetails(int restaurantID);
        IQueryable<RestaurantMenu> GetRestaurantMenus(int restaurantID);
        IQueryable<RestaurantRating> GetRestaurantRating(int restaurantID);
        IQueryable<RestaurantTables> GetTableDetails(int restaunrantID);
        IQueryable<RestaurantInformation> SearchRestaurantByLocation(LocationDetails locationDetails);
        IQueryable<RestaurantInformation> GetRestaurantsBasedOnMenu(AdditionalFeatureForSearch additionalFeatureForSearch);
        IQueryable<RestaurantInformation> SearchForRestaurant(SearchForRestaurant searchDetails);
        IQueryable<RestaurantInformation> SearchForRestaurantWithHigherRatingsFirst();
        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name=""></param>
        void RestaurantRating(RestaurantRating restaurantRating);
        /// <summary>
        /// Gets the Menu Name, Price and if Item is out of stock
        /// </summary>
        /// <param name="restaurantID"></param>
        /// <param name="cartItems"></param>
        List<CartItems> CartDetailsCheck(int restaurantID, List<CartItemsReceiverDetails> cartItems);
        /// <summary>
        /// Checks for availability 
        /// </summary>
        /// <param name="restaurantID"></param>
        /// <param name="menuId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        CartItems InsertCartDetailsCheck(CartItemsReceiverDetails cartItem);

    }
}
