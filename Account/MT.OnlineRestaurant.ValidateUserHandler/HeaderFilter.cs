using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Reflection;

namespace MT.OnlineRestaurant.ValidateUserHandler
{
    public class HeaderFilter : IOperationFilter
    {
        /// <summary>
        /// confuguration for header in swagger testing
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (!SkipAuthorization(controllerActionDescriptor.MethodInfo))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "AuthToken",
                    In = "header",
                    Type = "string",
                    Required = false // set to false if this is optional
                });

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "CustomerId",
                    In = "header",
                    Type = "int",
                    Required = false // set to false if this is optional
                });
            }
        }

        private bool SkipAuthorization(MethodInfo methodList)
        {
            string AllowAnonymous = "Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute";
            if (methodList != null)
            {
                var actionAttributes = methodList.GetCustomAttributes(inherit: true);
                foreach (var attribute in actionAttributes)
                {
                    string type = attribute.GetType().ToString();
                    if (type == AllowAnonymous)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
