using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public interface IRatingBusiness
    {
        List<RestaurantRating> GetRestaurantRatings(int restaurantId);
        bool AddRestaurantRating(RestaurantRating restaurantRating, out RestaurantRating returnedRestaurantRating);
        bool UpdateRestaurantRating(int ratingId, RestaurantRating restaurantRating);
        bool RemoveRestaurantRating(int ratingId);
    }
}
