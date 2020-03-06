using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Exceptions
{
    public class UserAndSensorLocationsDontMatchException : Exception
    {
        public UserAndSensorLocationsDontMatchException(string message) : base(message)
        {
        }
    }
}