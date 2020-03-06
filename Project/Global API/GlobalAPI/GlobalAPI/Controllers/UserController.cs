using GlobalAPI.Exceptions;
using GlobalAPI.Helpers;
using GlobalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlobalAPI.Controllers
{
    public class UserController : ApiController
    {
        // POST: api/User
        [InvalidUserFieldsExceptionFilter]
        public IHttpActionResult Post([FromBody]User user)
        {
            SqlServerHelper.CreateUser(user.Email, user.Password);

            return Content(HttpStatusCode.Created, new MessageHelper { Message = "User Created" });
        }
    }
}
