using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Exceptions
{
    public class NoDataFoundException : Exception
    {
        public NoDataFoundException(string message) : base(message)
        {
        }
    }
}