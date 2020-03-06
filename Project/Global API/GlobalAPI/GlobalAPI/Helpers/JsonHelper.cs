using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Helpers
{
    public class JsonHelper
    {
        public static string convert(string attribute, string msg)
        {
            return "{ \"" + attribute + "\": " + "\"" + msg + "\"" + "}";
        }
    }
}