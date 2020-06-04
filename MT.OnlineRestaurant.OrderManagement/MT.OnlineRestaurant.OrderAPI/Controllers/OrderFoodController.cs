#region References
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.OrderAPI.ModelValidators;
using System;
using System.Linq;
using System.Threading.Tasks;

using LoggingManagement;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
#endregion

#region Namespace
namespace MT.OnlineRestaurant.OrderAPI.Controllers
{
    /// <summary>
    /// Food ordering controller
    /// </summary>
    [Produces("application/json")]
    public class OrderFoodController : Controller
    {
        private readonly IPlaceOrderActions _placeOrderActions;
        private readonly ILogService _logService;
        /// <summary>
        /// Inject buisiness layer dependency
        /// </summary>
        /// <param name="placeOrderActions">Instance of this interface is injected in startup</param>
        public OrderFoodController(IPlaceOrderActions placeOrderActions, ILogService logService)
        {
            _placeOrderActions = placeOrderActions;
            _logService = logService;
        }       
        /// <summary>
        /// POST api/OrderFood
        /// To order food
        /// </summary>
        /// <param name="orderEntity">Order entity</param>
        /// <returns>Status of order</returns>
        [HttpPost]
        [Route("api/OrderFood")]
        public async Task<IActionResult> Post([FromBody]OrderEntity orderEntity)
        {
            try
            {
                _logService.LogMessage("Order Entity received at endpoint : api/OrderFood, User ID : " + orderEntity.CustomerId);
                int UserId = 1;//(Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
                string UserToken = "1407";//(Request.Headers.ContainsKey("AuthToken") ? Convert.ToString(HttpContext.Request.Headers["AuthToken"]) : "");

                OrderEntityValidator orderEntityValidator = new OrderEntityValidator(UserId, UserToken, _placeOrderActions);
                ValidationResult validationResult = orderEntityValidator.Validate(orderEntity);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToString("; "));
                }
                else
                {
                    var result = await Task<int>.Run(() => _placeOrderActions.PlaceOrder(orderEntity));
                    if (result == 0)
                    {
                        return BadRequest("Failed to place order, Please try again later");
                    }
                }
                return Ok("Order placed successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// DELETE api/CancelOrder
        /// Cancel the order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Status of order</returns>
        [HttpDelete]
        [Route("api/CancelOrder")]
        public IActionResult Delete(int id)
        {
            var result = _placeOrderActions.CancelOrder(id);
            if (result > 0)
            {
                return Ok("Order cancelled successfully");
            }

            return BadRequest("Failed to cancel order, Please try again later");
        }      
        //[HttpGet]
        //[Route("api/Reports")]
        //public IActionResult Reports(int customerId)
        //{
        //    IQueryable<CustomerOrderReport> result = _placeOrderActions.GetReports(customerId);
        //    if(result.Any())
        //    {
        //        return Ok(result.ToList());
        //    }

        //    return BadRequest("Failed to get the reports");
        //}     
    }
}
#endregion