using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;

namespace MT.OnlineRestaurant.ReviewManagement.Controllers
{
    [Route("api/RestaurantRating")]
    [ApiController]
    [Produces("application/json")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingBusiness _ratingBusiness;

        public RatingController(IRatingBusiness ratingBusiness)
        {
            _ratingBusiness = ratingBusiness;
        }
        /// <summary>
        /// Get All Ratings for a Restaurant
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{restaurantID:int}", Name = "GetId")]
        public ActionResult GetResturantRating(int restaurantID)
        {
            try
            {
                List<RestaurantRating> resturantRatings;
                resturantRatings = _ratingBusiness.GetRestaurantRatings(restaurantID);

                if (resturantRatings != null && resturantRatings.Count > 0)
                    return Ok(resturantRatings);

                return BadRequest($"No ratings found for Restaurant Id : {restaurantID} ! " +
                    $"Try again with other Restaurant Id");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Restaurant Id"))
                    return BadRequest(ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    $"Error in getting restaurant for Id : {restaurantID}! Try again after some time");
            }
        }
        /// <summary>
        /// Create a Rating for a Restaurant
        /// </summary>
        /// <param name="restaurantRating"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRestaurantRating([FromBody] RestaurantRating restaurantRating)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide proper input");

            try
            {
                bool isRatingAdded = _ratingBusiness.AddRestaurantRating(restaurantRating, out RestaurantRating returnedRestaurantRating);

                if (isRatingAdded)
                    return CreatedAtRoute("GetId", new { restaurantID = returnedRestaurantRating.RestaurantId }, returnedRestaurantRating);
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Entity Error") || ex.Message.Contains("Restaurant doesn't exist"))
                    return BadRequest(ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    $"Error in creating restaurant! Try again after some time.");
            }
        }
        /// <summary>
        /// Updating a Rating for a Restaurant
        /// </summary>
        /// <param name="restaurantRating"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{ratingId}")]
        public ActionResult UpdateRestaurantRating(int ratingId, [FromBody] RestaurantRating restaurantRating)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide proper input");
            try
            {
                bool isRatingUpdated = _ratingBusiness.UpdateRestaurantRating(ratingId, restaurantRating);

                if (isRatingUpdated)
                    return AcceptedAtRoute("GetId", new { restaurantID = restaurantRating.RestaurantId });
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Entity Error") || (ex.Message.Contains("HttpPost method")))
                    return BadRequest(ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    $"Error in updating restaurant! Try again after some time.");
            }
        }
        /// <summary>
        /// Removing a Rating for a Restaurant
        /// </summary>
        /// <param name="restaurantID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{ratingID}")]
        public ActionResult RemoveRestaurantRating(int ratingID)
        {
            try
            {
                bool isRatingDeleted = _ratingBusiness.RemoveRestaurantRating(ratingID);

                if (isRatingDeleted)
                    return NoContent();

                throw new Exception();
            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("Rating Id") || ex.Message.Contains("Rating doesn't exist"))
                    return BadRequest(ex.Message);
                
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    $"Error in deleting rating for Id : {ratingID}! Try again after some time");
            }
        }
    }
}