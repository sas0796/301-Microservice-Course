using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class RatingBusiness : IRatingBusiness
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingBusiness(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }
        public bool AddRestaurantRating(RestaurantRating restaurantRating, out RestaurantRating returnedRestaurantRating)
        {
            try
            {
                if (restaurantRating.RestaurantId < 1)
                    throw new Exception("Entity Error : Restaurant Id cannot be less than one");
                if (string.IsNullOrEmpty(restaurantRating.RestaurantId.ToString()))
                    throw new Exception("Entity Error : Restaurant Id cannot be empty");
                if (string.IsNullOrEmpty(restaurantRating.rating))
                    throw new Exception("Entity Error : Rating cannot be empty");

                DataLayer.DataEntity.RestaurantRating ratingForDataLayer = new DataLayer.DataEntity.RestaurantRating()
                {
                    RestaurantId = restaurantRating.RestaurantId,
                    rating = restaurantRating.rating,
                    user_Comments = restaurantRating.user_Comments
                };

                DataLayer.DataEntity.RestaurantRating returnedRating = new DataLayer.DataEntity.RestaurantRating();

                bool isSuccess = _ratingRepository.AddRestaurantRating(ratingForDataLayer, out returnedRating);

                returnedRestaurantRating = new RestaurantRating()
                {
                    RatingId = returnedRating.RatingId,
                    RestaurantId = returnedRating.RestaurantId,
                    rating = returnedRating.rating,
                    user_Comments = returnedRating.user_Comments
                };

                return isSuccess;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<RestaurantRating> GetRestaurantRatings(int restaurantId)
        {
            if (restaurantId < 1)
                throw new Exception("Restaurant Id cannot be less than one! Try again with input for Restaurant Id greater than or equals one");

            try
            {
                IQueryable<DataLayer.DataEntity.RestaurantRating> restaurantRatingQueryable = _ratingRepository.GetRestaurantRatings(restaurantId);
                List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
                if (restaurantRatingQueryable != null)
                {
                    foreach (var item in restaurantRatingQueryable)
                    {
                        RestaurantRating rating = new RestaurantRating()
                        {
                            RatingId = item.RatingId,
                            RestaurantId = item.RestaurantId,
                            rating = item.rating,
                            user_Comments = item.user_Comments
                        };
                        restaurantRatings.Add(rating);
                    }
                }
                return restaurantRatings;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RemoveRestaurantRating(int ratingId)
        {
            if (ratingId < 1)
                throw new Exception("Rating Id cannot be less than one! Try again with input for Rating Id greater than or equals one");

            try
            {
                bool isSuccess = _ratingRepository.RemoveRestaurantRating(ratingId);

                return isSuccess;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateRestaurantRating(int ratingId, RestaurantRating restaurantRating)
        {
            try
            {
                if (restaurantRating.RestaurantId < 1)
                    throw new Exception("Entity Error : Restaurant Id cannot be less than one");
                if (string.IsNullOrEmpty(restaurantRating.RestaurantId.ToString()))
                    throw new Exception("Entity Error : Restaurant Id cannot be empty");
                if (ratingId < 1)
                    throw new Exception("Entity Error : Rating Id cannot be less than one");
                if (string.IsNullOrEmpty(restaurantRating.rating))
                    throw new Exception("Entity Error : Rating cannot be empty");


                DataLayer.DataEntity.RestaurantRating ratingForDataLayer = new DataLayer.DataEntity.RestaurantRating()
                {
                    RatingId = ratingId,
                    RestaurantId = restaurantRating.RestaurantId,
                    rating = restaurantRating.rating,
                    user_Comments = restaurantRating.user_Comments
                };

                bool isSuccess = _ratingRepository.UpdateRestaurantRating(ratingId, ratingForDataLayer);

                return isSuccess;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
