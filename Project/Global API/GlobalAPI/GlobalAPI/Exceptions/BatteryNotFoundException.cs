using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Exceptions
{
    public class BatteryNotFoundException : Exception
    {
        public BatteryNotFoundException(string message) : base(message)
        {
        }
    }
}