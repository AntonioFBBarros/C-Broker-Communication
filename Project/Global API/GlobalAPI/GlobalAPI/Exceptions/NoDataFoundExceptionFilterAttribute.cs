using GlobalAPI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace GlobalAPI.Exceptions
{
    public class NoDataFoundExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NoDataFoundException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NoContent)
                {
                    Content = new StringContent(JsonHelper.convert("Error", context.Exception.Message), Encoding.UTF8, "application/json")
                };
                context.Response = resp;
            }
        }
    }
}