using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using MT.Online.Restaurant.MessagesManagement.Services;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.BusinessLayer.ModelValidator;

namespace MT.OnlineRestaurant.CustomerManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CustomerController : Controller
    {        
        private readonly IMessages _msgObj;
        public ICustomerBusiness _customerBusiness { get; set; }
        public CustomerController(ICustomerBusiness customerBusiness, IMessages msgObj)
        {
            _customerBusiness = customerBusiness;
            _msgObj = msgObj;
        }

        // GET api/values
        /// <summary>
        /// User Registeration 
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister([Required][FromBody]CustomerDetails userDetails)
        {
            CustomerValidator customerValidator = new CustomerValidator();
            FluentValidation.Results.ValidationResult validationResult = customerValidator.Validate(userDetails);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString("; "));
            }           
                var result = await Task<int>.Run(() => _customerBusiness.UserRegisteration(userDetails));
                if (result > 0)
                {
                    return Ok("User Added Successfully");
                }
                else
                    return BadRequest("User is not added");
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetCustomerDetails(PaginationModel pagingparametermodel)
        {
            var customerList = await Task<IQueryable<CustomerDetails>>.Run(() => _customerBusiness.GetCustomerDetails());
            int count = customerList.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = customerList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };
            // Returing List of Customers Collections  
            if (customerList.Count() == 0)
                return NotFound("UserList is empty");
            return Ok(items);
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateCustomerDetails([Required][FromBody]CustomerDetails customerDetails)
        {
            //_msgObj.RegisterOnMessageHandlerAndReceiveMessages();
            CustomerValidator customerValidator = new CustomerValidator();
            FluentValidation.Results.ValidationResult validationResult = customerValidator.Validate(customerDetails);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString("; "));
            }
            var result = await Task<int>.Run(() => _customerBusiness.UpdateCustomerDetails(customerDetails));
            if (result > 0)
                return Ok("Customer data updated successfully");
            else
                return BadRequest("Customer data cannot be updated.Please try again later");
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> DeactivateCustomer([Required] bool CustomerStatus)
        {
            int customerID = 0;
                if (Request.Headers.ContainsKey("CustomerId"))
                {
                    var userId = HttpContext.Request.Headers["CustomerId"].ToString();
                     customerID = int.Parse(userId);
                }
                var result = await Task<int>.Run(() => _customerBusiness.DeactivateCustomer(customerID, CustomerStatus));
                if (result > 0)
                    return Ok("Customer deactivated successfully");
                else
                    return BadRequest("Customer data cannot be updated.Please try again later");
        }      
    }
}
