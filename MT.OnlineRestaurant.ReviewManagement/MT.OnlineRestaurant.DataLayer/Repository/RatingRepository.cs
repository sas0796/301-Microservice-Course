using MT.OnlineRestaurant.DataLayer.DataEntity;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ReviewManagementContext _dbContext;

        public RatingRepository(ReviewManagementContext context)
        {
            _dbContext = context;
        }

        public bool AddRestaurantRating(RestaurantRating restaurantRating, out RestaurantRating returnedRestaurantRating)
        {
            try
            {
                if (!DoesRestaurantExists(restaurantRating.RestaurantId))
                    throw new Exception("Restaurant doesn't exist! Try sending a valid Restaurant Id");

                TblRating tblRating = new TblRating()
                {
                    Id = restaurantRating.RatingId,
                    TblRestaurantId = restaurantRating.RestaurantId,
                    Rating = restaurantRating.rating,
                    Comments = restaurantRating.user_Comments,
                    RecordTimeStampCreated = DateTime.Now,
                    RecordTimeStampUpdated = DateTime.Now
                };
                _dbContext.Add(tblRating);

                SaveChanges();

                returnedRestaurantRating = new RestaurantRating()
                {
                    RatingId = tblRating.Id,
                    RestaurantId = tblRating.TblRestaurantId,
                    rating = tblRating.Rating,
                    user_Comments = tblRating.Comments
                };

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<RestaurantRating> GetRestaurantRatings(int restaurantId)
        {
            try
            {
                var restaurantRatings = (from ratings in _dbContext.TblRating
                                         join restaurant in _dbContext.TblRestaurant on ratings.TblRestaurantId equals restaurant.Id
                                         where restaurant.Id == restaurantId
                                         select new RestaurantRating
                                         {
                                             RatingId = ratings.Id,
                                             RestaurantId = ratings.TblRestaurantId,
                                             rating = ratings.Rating,
                                             user_Comments = ratings.Comments
                                         }).Distinct().ToList();
                return restaurantRatings.AsQueryable();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RemoveRestaurantRating(int ratingId)
        {
            try
            {
                var rating = (_dbContext.TblRating.Find(ratingId));
                
                if(rating == null)
                    throw new Exception("Rating doesn't exist! Try creating the rating first");
                
                _dbContext.Remove(rating);

                return SaveChanges();
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
                if (!DoesRatingForRestaurantExists(restaurantRating.RestaurantId, ratingId))
                    throw new Exception($"Rating doesn't exist for the Restaurant {restaurantRating.RestaurantId}! " +
                        $"Try creating the same rating using HttpPost method.");

                TblRating tblRating = new TblRating()
                {
                    Id = ratingId,
                    TblRestaurantId = restaurantRating.RestaurantId,
                    Rating = restaurantRating.rating,
                    Comments = restaurantRating.user_Comments,
                    RecordTimeStampCreated = DateTime.Now.AddDays(-1),
                    RecordTimeStampUpdated = DateTime.Now
                };
                _dbContext.Update(tblRating);

                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool DoesRatingForRestaurantExists(int restaurantId, int ratingId)
        {
            try
            {
                var isRatingPresentForRestaurant = (from ratings in _dbContext.TblRating
                                       join restaurant in _dbContext.TblRestaurant on ratings.TblRestaurantId equals restaurant.Id
                                       where ratings.Id == ratingId && restaurant.Id == restaurantId
                                       select new RestaurantRating
                                       {
                                           RatingId = ratings.Id,
                                           RestaurantId = ratings.TblRestaurantId,
                                           rating = ratings.Rating,
                                           user_Comments = ratings.Comments
                                       }).Count() > 0;
                return isRatingPresentForRestaurant;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool DoesRestaurantExists(int restaurantId)
        {
            try
            {
                var restaurant = (_dbContext.TblRestaurant.Find(restaurantId));
                if (restaurant != null)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
