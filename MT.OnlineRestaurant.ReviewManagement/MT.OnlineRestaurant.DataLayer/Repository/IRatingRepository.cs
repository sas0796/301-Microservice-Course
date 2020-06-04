using MT.OnlineRestaurant.DataLayer.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public interface IRatingRepository
    {
        IQueryable<RestaurantRating> GetRestaurantRatings(int restaurantId);
        bool AddRestaurantRating(RestaurantRating restaurantRating, out RestaurantRating returnedRestaurantRating);
        bool UpdateRestaurantRating(int ratingId, RestaurantRating restaurantRating);
        bool RemoveRestaurantRating(int ratingId);
    }
}
