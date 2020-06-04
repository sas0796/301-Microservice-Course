using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MT.OnlineRestaurant.Logging.Context;
using MT.OnlineRestaurant.Logging.DAL;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MT.OnlineRestaurant.Logging
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        private readonly ILogging _logging;

        #region Constructior
        public ErrorHandlingFilter(string dbConnectionString)
        {
            _logging = new LoggingRespository(dbConnectionString);
        }
        #endregion
        public override void OnException(ExceptionContext context)
        {
            try
            {
                var exception = context.Exception;

                LoggingInfo info = new LoggingInfo()
                {
                    ActionName = string.Empty,
                    ControllerName = string.Empty,
                    Description = exception.InnerException != null ? exception.InnerException.Message + exception.InnerException.StackTrace : exception.Message + exception.StackTrace,
                    RecordTimeStamp = DateTime.Now,
                };
                this._logging.CaptureLogs(info);

                HandleExceptionAsync(context);
                context.ExceptionHandled = true;
            }
            catch
            {

            }
        }

        private static void HandleExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is NullReferenceException)
                SetExceptionResult(context, exception, HttpStatusCode.NotFound);
            else if (exception is UnauthorizedAccessException)
                SetExceptionResult(context, exception, HttpStatusCode.Unauthorized);            
            else
                SetExceptionResult(context, exception, HttpStatusCode.InternalServerError);
        }

        private static void SetExceptionResult(
            ExceptionContext context,
            Exception exception,
            HttpStatusCode code)
        {
            context.Result = new JsonResult(exception.Message)
            {
                StatusCode = (int)code
            };
        }
    }
}
