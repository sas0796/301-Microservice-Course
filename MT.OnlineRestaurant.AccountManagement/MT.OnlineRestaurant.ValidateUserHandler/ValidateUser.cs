using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Net.Http;
using System.Net;
using System;
using System.IdentityModel.Tokens.Jwt;
using MT.OnlineRestaurant.DataLayer.Context;

namespace MT.OnlineRestaurant.ValidateUserHandler
{
    /// <summary>
    /// Validate JWT Token
    /// </summary>
    public class ValidateUser:DelegatingHandler
    {
        public readonly string _secretKey = string.Empty;
        public readonly string _issuerKey = string.Empty;
        private readonly string  _connectionStrings;
        public CustomerManagementContext _context { get; set; }

        public ValidateUser()
        {
            /*to read app strings value*/
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            _secretKey = root.GetSection("AppSettings").GetSection("SecretKey").Value;
            _issuerKey = root.GetSection("AppSettings").GetSection("IssuerKey").Value;
            //_connectionStrings = connectionString;
          //  _context = new CustomerManagementContext(_connectionStrings);
        }
    
        public string SecretKey
        {
            get => _secretKey;
        }
        public string IssuerKey
        {
            get => _issuerKey;
        }
        public static bool TryRetrieveToken(IEnumerable<string> request, out string token)
        {
            token = null;
            if (!(request.Count()>0))
            {
                return false;
            }
            var bearerToken = request.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

         public  HttpStatusCode  ValidateToken(IEnumerable<string> requestHeader)
        {
            HttpStatusCode statusCode;
            string token;
            //determine whether a jwt exists or not
            if (!TryRetrieveToken(requestHeader, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                //allow requests with no token - whether a action method needs an authentication can be set with the claimsauthorization attribute
                return statusCode;
            }
            try
            {
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(SecretKey));
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {                 
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    LifetimeValidator = this.LifetimeValidator,
                    ValidIssuer = IssuerKey,
                    IssuerSigningKey = securityKey
                };
                //extract and assign the user of the jwt
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                statusCode = HttpStatusCode.OK;      
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return statusCode;
        }
    
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}
