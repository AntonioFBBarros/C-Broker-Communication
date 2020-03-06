using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class SensorData
    {
        public int Id { get; set; }

        public int SensorId { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}