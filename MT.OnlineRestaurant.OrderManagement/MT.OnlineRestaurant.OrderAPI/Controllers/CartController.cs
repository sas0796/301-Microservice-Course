using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LoggingManagement;
using Microsoft.AspNetCore.Mvc;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer.interfaces;

namespace MT.OnlineRestaurant.OrderAPI.Controllers
{
    [Produces("application/json")]
    public class CartController : Controller
    {
        private readonly ICartBusiness _cartBusiness;
        private readonly ILogService _logService;
        /// <summary>
        /// Inject buisiness layer dependency
        /// </summary>
        /// <param name="cartBusiness">Instance of this interface is injected in startup</param>
        public CartController(ICartBusiness cartBusiness, ILogService logService)
        {
            _cartBusiness = cartBusiness;
            _logService = logService;
        }

        /// <summary>
        /// GET api/GetCart
        /// To get cart details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetCart/{customerId}", Name = "GetCartDetails")]
        public async Task<IActionResult> Get(int customerId)
        {
            try
            {
                _logService.LogMessage($"Customer Id received at endpoint : api/GetCart, Customer ID : {customerId}");
                Task<CartDetails> cartDetails = _cartBusiness.GetCartDetails(customerId);

                if (cartDetails.Result != null)
                    return Ok(cartDetails.Result);

                return this.StatusCode((int)HttpStatusCode.InternalServerError, $"Error getting cart details for customer : {customerId}");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/InsertCart/{customerId}")]
        public ActionResult Insert(int customerId, [FromBody]InsertCartDetails insertCartDetails)
        {
            try
            {
                _logService.LogMessage($"Customer Id received at endpoint : api/InsertCart, Customer ID : {customerId}");
                Task<bool> result = _cartBusiness.InsertCartDetails(customerId, insertCartDetails);

                if (result.Result)
                {
                    _logService.LogMessage($"Cart details added for customer : {customerId}");
                    return Created("GetCartDetails", new { customerId });
                }

                _logService.LogMessage($"Error inserting cart details for customer : {customerId}");
                return this.StatusCode((int)HttpStatusCode.InternalServerError, $"Error inserting cart details for customer : {customerId}");
            }
            catch (Exception ex)
            {
                _logService.LogMessage(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// PUT api/UpdateCart
        /// To update cart item
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="updateCartDetails"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UpdateCart/{customerId}")]
        public ActionResult Update(int customerId, [FromBody]InsertCartDetails updateCartDetails)
        {
            try
            {
                _logService.LogMessage($"Customer Id received at endpoint : api/UpdateCart, Customer ID : {customerId}");
                Task<bool> result = _cartBusiness.UpdateCartDetails(customerId, updateCartDetails);

                if (result.Result)
                    return Accepted($"Cart details updated for customer : {customerId}");

                return this.StatusCode((int)HttpStatusCode.InternalServerError, $"Error updating cart details for customer : {customerId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/RemoveCart/{customerId}/{restaurantId}/{menuId}")]
        public ActionResult Remove(int customerId, int restaurantId, int menuId)
        {
            try
            {
                _logService.LogMessage($"Customer Id received at endpoint : api/RemoveCart, Customer ID : {customerId}");
                bool result = _cartBusiness.RemoveCartDetails(customerId, restaurantId, menuId);

                if (result)
                    return NoContent();

                return this.StatusCode((int)HttpStatusCode.InternalServerError, $"Error removing cart details for customer : {customerId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}