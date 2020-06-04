using MT.OnlineRestaurant.DataLayer.DataEntity;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public interface ISearchRepository
    {
        TblRestaurant GetResturantDetails(int restaurantID);
        IQueryable<TblRating> GetRestaurantRating(int restaurantID);

        IQueryable<MenuDetails> GetRestaurantMenu(int restaurantID);

        IQueryable<TblRestaurantDetails> GetTableDetails(int restaurantID);
        IQueryable<RestaurantSearchDetails> GetRestaurantsBasedOnLocation(LocationDetails location_Details);
        IQueryable<RestaurantSearchDetails> GetRestaurantsBasedOnMenu(AddtitionalFeatureForSearch searchDetails);
        IQueryable<RestaurantSearchDetails> SearchForRestaurant(SearchForRestautrant searchDetails);
        IQueryable<RestaurantSearchDetails> SearchForRestaurantWithHigherRatingsFirst();

        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name="tblRating"></param>
        void RestaurantRating(TblRating tblRating);
        /// <summary>
        /// Returns a Menu Item based on restaurantId & menuId
        /// </summary>
        /// <param name="restaurantID"></param>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        MenuItems CartDetailsCheck(int restaurantID,int MenuID);
        /// <summary>
        /// Updates the quantities of menus after every booking
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        bool UpdateQuantitiesForMenuOfRestaurant(int restaurantId, int menuId, int quantity, out int quantityDifference);

    }
}
