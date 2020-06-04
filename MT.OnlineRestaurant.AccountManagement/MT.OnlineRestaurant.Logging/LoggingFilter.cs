#region References
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using MT.OnlineRestaurant.Logging.Context;
using MT.OnlineRestaurant.Logging.DAL;
using System;
using System.Collections.Generic;
#endregion

namespace MT.OnlineRestaurant.Logging
{
    #region class defintion
    /// <summary>
    /// captures the loggging for each action / request
    /// </summary>
    public class LoggingFilter : Attribute, IActionFilter
    {
        private readonly ILogging _logging;

        #region Constructior
        public LoggingFilter(string dbConnectionString)
        {
            _logging = new LoggingRespository(dbConnectionString);
        }
        #endregion

        #region ActionExecuted
        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

                LoggingInfo info = new LoggingInfo()
                {
                    ActionName = actionName,
                    ControllerName = controllerName,
                    Description = "action executed at:" + DateTime.Now.ToString(),
                    RecordTimeStamp = DateTime.Now,
                };

                foreach (ParameterDescriptor item
                    in ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.Parameters)
                {
                    //print item.Key
                    //print item.Value
                }
                this._logging.CaptureLogs(info);



                //code to insert into the log table
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ActionExecuting
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
                var controller = context.Controller as Controller;
                LoggingInfo info = new LoggingInfo()
                {
                    ActionName = actionName,
                    ControllerName = controllerName,
                    Description = "action executed at:" + DateTime.Now.ToString(),
                    RecordTimeStamp = DateTime.Now,
                };

                this._logging.CaptureLogs(info);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        #endregion

    }
    #endregion
}
