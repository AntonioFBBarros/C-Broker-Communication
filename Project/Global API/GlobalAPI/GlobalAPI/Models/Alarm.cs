using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class Alarm
    {
        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Reason { get; set; }

        public int Floor { get; set; }

        public string Location { get; set; }
    }
}