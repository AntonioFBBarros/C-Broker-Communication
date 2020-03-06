using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Exceptions
{
    public class InvalidIntervalException : Exception
    {
        public InvalidIntervalException(string message) : base(message)
        {
        }
    }
}