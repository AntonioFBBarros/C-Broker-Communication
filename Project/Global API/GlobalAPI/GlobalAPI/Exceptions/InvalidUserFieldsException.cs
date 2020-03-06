using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Exceptions
{
    public class InvalidUserFieldsException : Exception
    {
        public InvalidUserFieldsException(string message) : base(message)
        {
        }
    }
}