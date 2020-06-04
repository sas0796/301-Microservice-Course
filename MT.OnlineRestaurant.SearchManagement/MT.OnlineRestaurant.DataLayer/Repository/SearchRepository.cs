using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Linq;
using MT.OnlineRestaurant.DataLayer.DataEntity;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly RestaurantManagementContext db;
        public SearchRepository(RestaurantManagementContext connection)
        {
            db = connection;
        }

        #region Interface Methods
        public IQueryable<MenuDetails> GetRestaurantMenu(int restaurantID)
        {
            List<MenuDetails> menudetails = new List<MenuDetails>();
            try
            {
                if (db != null)
                {
                    var menudetail = (from offer in db.TblOffer
                                      join menu in db.TblMenu
                                      on offer.TblMenuId equals menu.Id into TableMenu
                                      from menu in TableMenu.ToList()
                                      join cuisine in db.TblCuisine on menu.TblCuisineId equals cuisine.Id
                                      where offer.TblRestaurantId == restaurantID
                                      select new MenuDetails
                                      {
                                          tbl_Offer = offer,
                                          tbl_Cuisine = cuisine,
                                          tbl_Menu = menu

                                      }).ToList();
                    foreach (var item in menudetail)
                    {
                        MenuDetails menuitem = new MenuDetails
                        {
                            tbl_Cuisine = item.tbl_Cuisine,
                            tbl_Menu = item.tbl_Menu,
                            tbl_Offer = item.tbl_Offer
                        };
                        menudetails.Add(menuitem);
                    }
                }
                return menudetails.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<TblRating> GetRestaurantRating(int restaurantID)
        {
            // List<TblRating> restaurant_Rating = new List<TblRating>();
            try
            {
                if (db != null)
                {
                    return (from rating in db.TblRating
                            join restaurant in db.TblRestaurant on
                            rating.TblRestaurantId equals restaurant.Id
                            where rating.TblRestaurantId == restaurantID
                            select new TblRating
                            {
                                Rating = rating.Rating,
                                Comments = rating.Comments,
                                TblRestaurant = restaurant,
                            }).AsQueryable();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TblRestaurant GetResturantDetails(int restaurantID)
        {
            TblRestaurant resturantInformation = new TblRestaurant();

            try
            {
                if (db != null)
                {
                    resturantInformation = (from restaurant in db.TblRestaurant
                                            join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                            where restaurant.Id == restaurantID
                                            select new TblRestaurant
                                            {
                                                Id = restaurant.Id,
                                                Name = restaurant.Name,
                                                Address = restaurant.Address,
                                                ContactNo = restaurant.ContactNo,
                                                TblLocation = location,
                                                CloseTime = restaurant.CloseTime,
                                                OpeningTime = restaurant.OpeningTime,
                                                Website = restaurant.Website
                                            }).FirstOrDefault();

                }

                return resturantInformation;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public IQueryable<TblRestaurantDetails> GetTableDetails(int restaurantID)
        {
            try
            {
                if (db != null)
                {
                    return (from restaurantDetails in db.TblRestaurantDetails
                            join restaurant in db.TblRestaurant
                            on restaurantDetails.TblRestaurantId equals restaurant.Id
                            where restaurantDetails.TblRestaurantId == restaurantID
                            select new TblRestaurantDetails
                            {
                                TableCapacity = restaurantDetails.TableCapacity,
                                TableCount = restaurantDetails.TableCount,
                                TblRestaurant = restaurant
                            }).AsQueryable();

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantSearchDetails> GetRestaurantsBasedOnLocation(LocationDetails location_Details)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {
                restaurants = GetRetaurantBasedOnLocationAndName(location_Details);
                return restaurants.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IQueryable<RestaurantSearchDetails> GetRestaurantsBasedOnMenu(AddtitionalFeatureForSearch searchDetails)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {
                restaurants = GetRestaurantDetailsBasedOnRating(searchDetails);
                return restaurants.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IQueryable<RestaurantSearchDetails> SearchForRestaurant(SearchForRestautrant searchDetails)
        {
            try
            {
                List<RestaurantSearchDetails> searchedRestaurantBasedOnRating = new List<RestaurantSearchDetails>();
                if(!string.IsNullOrEmpty(searchDetails.search.cuisine) || !string.IsNullOrEmpty(searchDetails.search.Menu)
                    || searchDetails.search.rating > 0)
                    searchedRestaurantBasedOnRating = GetRestaurantDetailsBasedOnRating(searchDetails.search);

                List<RestaurantSearchDetails> restaurantsBasedOnLocation = new List<RestaurantSearchDetails>();
                if(!string.IsNullOrEmpty(searchDetails.location.restaurant_Name) || searchDetails.location.xaxis != 0
                    || searchDetails.location.yaxis != 0)
                    restaurantsBasedOnLocation = GetRetaurantBasedOnLocationAndName(searchDetails.location);

                List<RestaurantSearchDetails> restaurantInfo = new List<RestaurantSearchDetails>();

                if ((!string.IsNullOrEmpty(searchDetails.search.cuisine) || !string.IsNullOrEmpty(searchDetails.search.Menu)
                    || searchDetails.search.rating > 0) && restaurantsBasedOnLocation.Count == 0)
                {
                    restaurantInfo = restaurantsBasedOnLocation;
                }
                else if ((!string.IsNullOrEmpty(searchDetails.location.restaurant_Name) || searchDetails.location.xaxis != 0
                || searchDetails.location.yaxis != 0) && searchedRestaurantBasedOnRating.Count == 0)
                {
                    restaurantInfo = searchedRestaurantBasedOnRating;
                }
                else if (searchedRestaurantBasedOnRating.Any() && restaurantsBasedOnLocation.Any())
                {
                    restaurantInfo = searchedRestaurantBasedOnRating.Intersect
                        (restaurantsBasedOnLocation, new RestaurantSearchDetailsComparer()).ToList();
                }

                return restaurantInfo.AsQueryable();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantSearchDetails> SearchForRestaurantWithHigherRatingsFirst()
        {
            try
            {
                if (db != null)
                {
                    var restaurantFilter = (from restaurant in db.TblRestaurant
                                            join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                            select new { TblRestaurant = restaurant, TblLocation = location });
                    
                    restaurantFilter = (from restaurant in restaurantFilter
                                        join rating in db.TblRating on restaurant.TblRestaurant.Id equals rating.TblRestaurantId
                                        orderby rating.Rating descending
                                        select restaurant);

                    List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
                    foreach (var item in restaurantFilter)
                    {
                        RestaurantSearchDetails restaurant = new RestaurantSearchDetails
                        {
                            restauran_ID = item.TblRestaurant.Id,
                            restaurant_Name = item.TblRestaurant.Name,
                            restaurant_Address = item.TblRestaurant.Address,
                            restaurant_PhoneNumber = item.TblRestaurant.ContactNo,
                            restraurant_Website = item.TblRestaurant.Website,
                            closing_Time = item.TblRestaurant.CloseTime,
                            opening_Time = item.TblRestaurant.OpeningTime,
                            xaxis = Convert.ToDouble(string.IsNullOrEmpty(item.TblLocation.X.ToString()) ? "0.00" : item.TblLocation.X.ToString()),
                            yaxis = Convert.ToDouble(string.IsNullOrEmpty(item.TblLocation.Y.ToString()) ? "0.00" : item.TblLocation.Y.ToString())
                    };
                        restaurants.Add(restaurant);
                    }
                    return restaurants.AsQueryable();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary> 
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name="tblRating"></param>
        public void RestaurantRating(TblRating tblRating)
        {
            //tblRating.UserCreated = ,
            //tblRating.UserModified=,
            tblRating.RecordTimeStampCreated = DateTime.Now;

            db.Set<TblRating>().Add(tblRating);
            db.SaveChanges();

        }
        /// <summary>
        /// Returns a Menu Item based on restaurantId & menuId
        /// </summary>
        /// <param name="restaurantID"></param>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public MenuItems CartDetailsCheck(int restaurantId, int menuId)
        {
            try
            {
                MenuItems menuObj = new MenuItems();
                if (db != null)
                {
                    menuObj = (from menu in db.TblMenu
                                join restMenuPrice in db.TblRestaurantMenuPrice on menu.Id equals restMenuPrice.tblMenuId
                                join rest in db.TblRestaurant on restMenuPrice.tblRestaurantId equals rest.Id
                                where restMenuPrice.tblMenuId == menuId && restMenuPrice.tblRestaurantId == restaurantId
                                select new MenuItems
                                {
                                    Price = Convert.ToDouble(restMenuPrice.Price),
                                    Item = menu.Item,
                                    Quantity = restMenuPrice.Quantity
                                }).FirstOrDefault();

                    if (menuObj == null)
                        throw new Exception("Menu not present");

                    double offerPrice = (from menu in db.TblMenu
                                         join offer in db.TblOffer on menu.Id equals offer.TblMenuId
                                         join rest in db.TblRestaurant on offer.TblRestaurantId equals rest.Id
                                         where menu.Id == menuId && rest.Id == restaurantId &&
                                         offer.FromDate < DateTime.Now && offer.ToDate > DateTime.Now
                                         select offer.Discount).FirstOrDefault();
                    if (offerPrice != 0)
                        menuObj.Price -= offerPrice;
                }
                    return menuObj;
            }            
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Updates the quantities of menus after every booking
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool UpdateQuantitiesForMenuOfRestaurant(int restaurantId, int menuId, int quantity, out int quantityDifference)
        {
            var tblRestaurantMenuPrice = (from menu in db.TblMenu
                                          join rmp in db.TblRestaurantMenuPrice on menu.Id equals rmp.tblMenuId
                                          join rest in db.TblRestaurant on rmp.tblRestaurantId equals rest.Id
                                          where rmp.tblRestaurantId == restaurantId && rmp.tblMenuId == menuId
                                          select rmp).FirstOrDefault();

            if (tblRestaurantMenuPrice == null)
                throw new Exception($"Couldn't find Menu {menuId} for Restaurant {restaurantId}");

            quantityDifference = tblRestaurantMenuPrice.Quantity - quantity;

            tblRestaurantMenuPrice.Quantity = quantityDifference;
            return db.SaveChanges() > 0 ? true : false;
        }
        #endregion

        #region private methods
        private List<RestaurantSearchDetails> GetRestaurantDetailsBasedOnRating(AddtitionalFeatureForSearch searchList)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {
                var restaurantFilter = (from restaurant in db.TblRestaurant
                                      join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                      select new { TblRestaurant = restaurant, TblLocation = location });

                if(!string.IsNullOrEmpty(searchList.cuisine))
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        join offer in db.TblOffer on filteredRestaurant.TblRestaurant.Id equals offer.TblRestaurantId
                                        join menu in db.TblMenu on offer.TblMenuId equals menu.Id
                                        join cuisine in db.TblCuisine on menu.TblCuisineId equals cuisine.Id
                                        where cuisine.Cuisine.Contains(searchList.cuisine)
                                        select filteredRestaurant).Distinct();
                }
                if(!string.IsNullOrEmpty(searchList.Menu))
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        join offer in db.TblOffer on filteredRestaurant.TblRestaurant.Id equals offer.TblRestaurantId
                                        join menu in db.TblMenu on offer.TblMenuId equals menu.Id
                                        where menu.Item.Contains(searchList.Menu)
                                        select filteredRestaurant).Distinct();
                }

                if(searchList.rating > 0)
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        join rating in db.TblRating on filteredRestaurant.TblRestaurant.Id equals rating.TblRestaurantId
                                        where rating.Rating.Contains(searchList.rating.ToString())
                                        select filteredRestaurant).Distinct();
                }
                foreach (var item in restaurantFilter)
                {
                    RestaurantSearchDetails restaurant = new RestaurantSearchDetails
                    {
                        restauran_ID = item.TblRestaurant.Id,
                        restaurant_Name = item.TblRestaurant.Name,
                        restaurant_Address = item.TblRestaurant.Address,
                        restaurant_PhoneNumber = item.TblRestaurant.ContactNo,
                        restraurant_Website = item.TblRestaurant.Website,
                        closing_Time = item.TblRestaurant.CloseTime,
                        opening_Time = item.TblRestaurant.OpeningTime,
                        xaxis = Convert.ToDouble(string.IsNullOrEmpty(item.TblLocation.X.ToString()) ? "0.00" : item.TblLocation.X.ToString()),
                        yaxis = Convert.ToDouble(string.IsNullOrEmpty(item.TblLocation.Y.ToString()) ? "0.00" : item.TblLocation.Y.ToString())
                    };
                    restaurants.Add(restaurant);
                }
                return restaurants;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<RestaurantSearchDetails> GetRetaurantBasedOnLocationAndName(LocationDetails location_Details)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {

                 var restaurantInfo = (from restaurant in db.TblRestaurant
                                          join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                          select new { TblRestaurant = restaurant, TblLocation = location });

                if(!string.IsNullOrEmpty(location_Details.restaurant_Name))
                {
                    restaurantInfo = restaurantInfo.Where(a => a.TblRestaurant.Name.Contains(location_Details.restaurant_Name));

                }

                if(!(double.IsNaN(location_Details.xaxis)) && !(double.IsNaN(location_Details.yaxis)))
                {
                    foreach (var place in restaurantInfo)
                    {
                        place.TblLocation.X = Convert.ToDecimal(string.IsNullOrEmpty(place.TblLocation.X.ToString()) ? "0.00" : place.TblLocation.X.ToString());
                        place.TblLocation.Y = Convert.ToDecimal(string.IsNullOrEmpty(place.TblLocation.Y.ToString()) ? "0.00" : place.TblLocation.Y.ToString());
                        
                        double distance = Distance(location_Details.xaxis, location_Details.yaxis, (double)place.TblLocation.X, (double)place.TblLocation.Y);
                        if (distance <= int.Parse(location_Details.distance.ToString()))
                        {
                            RestaurantSearchDetails tblRestaurant = new RestaurantSearchDetails
                            {
                                restauran_ID = place.TblRestaurant.Id,
                                restaurant_Name = string.IsNullOrEmpty(place.TblRestaurant.Name) ? string.Empty : place.TblRestaurant.Name,
                                restaurant_Address = string.IsNullOrEmpty(place.TblRestaurant.Address) ? string.Empty : place.TblRestaurant.Address,
                                restaurant_PhoneNumber = string.IsNullOrEmpty(place.TblRestaurant.ContactNo) ? string.Empty : place.TblRestaurant.ContactNo,
                                restraurant_Website = string.IsNullOrEmpty(place.TblRestaurant.Website) ? string.Empty : place.TblRestaurant.Website,
                                closing_Time = string.IsNullOrEmpty(place.TblRestaurant.CloseTime) ? string.Empty : place.TblRestaurant.CloseTime,
                                opening_Time = string.IsNullOrEmpty(place.TblRestaurant.OpeningTime) ? string.Empty : place.TblRestaurant.OpeningTime,
                                xaxis = Convert.ToDouble(place.TblLocation.X),
                                yaxis = Convert.ToDouble(place.TblLocation.Y)
                        };
                             restaurants.Add(tblRestaurant);
                        }
                    }
                }
                return restaurants;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double Distance(double currentLatitude, double currentLongitude, double latitude, double longitude)
        {
            if (currentLatitude == latitude && currentLongitude == longitude)
                return 0.0;
            double theta = currentLatitude - latitude;
            double dist = Math.Sin(GetRadius(currentLatitude)) * Math.Sin(GetRadius(longitude)) + Math.Cos(GetRadius(currentLatitude)) * Math.Cos(GetRadius(latitude)) * Math.Cos(GetRadius(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = (dist * 60 * 1.1515) / 0.6213711922;          //miles to kms
            return (dist);
        }

        private double rad2deg(double dist)
        {
            return (dist * Math.PI / 180.0);
        }

        private double GetRadius(double Latitude)
        {
            return (Latitude * 180.0 / Math.PI);
        }
        #endregion
    }
    public class RestaurantSearchDetailsComparer : IEqualityComparer<RestaurantSearchDetails>
    {
        public bool Equals(RestaurantSearchDetails x, RestaurantSearchDetails y)
        {
            if (x == y) // same instance or both null
                return true;
            if (x == null || y == null) // either one is null but not both
                return false;

            return x.restauran_ID == y.restauran_ID;
        }


        public int GetHashCode(RestaurantSearchDetails restaurantSearchDetails)
        {
            return restaurantSearchDetails != null ? restaurantSearchDetails.restauran_ID : 0;
        }
    }
}
