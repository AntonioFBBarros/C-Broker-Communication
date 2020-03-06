using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Exceptions
{
    public class SensorExistsException : Exception
    {
        public SensorExistsException(string message) : base(message)
        {
        }
    }
}