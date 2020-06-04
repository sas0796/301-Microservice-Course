using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.BusinessLayer.Interfaces;
using MT.OnlineRestaurant.Helper;

namespace MT.OnlineRestaurant.AccountManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SecurityController : Controller
    {
        private readonly IUserBusiness _userBusiness;
        private readonly AppSettings _appSettings;
        public SecurityController(IUserBusiness userBusiness, IOptions<AppSettings> appSettings)
        {
            _userBusiness = userBusiness;
            _appSettings = appSettings.Value;
        }
        

        // GET api/values/5
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            var user = _userBusiness.UserLogin(username, password);
            if (user == null)
                return BadRequest("Please enter valid username and password");
            string tokenString = TokenGenerator.CreateToken(_appSettings.SecretKey, _appSettings.IssuerKey, user.Id,Convert.ToInt32(_appSettings.ExpiryTime));
            return Ok(new
            {
                UserID =user.Id,
                User = user.FirstName + user.LastName,
                token = tokenString
            });

        }

    }
}
