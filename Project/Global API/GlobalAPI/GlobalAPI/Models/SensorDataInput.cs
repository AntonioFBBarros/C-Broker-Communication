using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class SensorDataInput
    {
        public int SensorId { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public string CurrentLocation { get; set; }

        public int CurrentFloor { get; set; }
    }
}