
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShowGraphs
{
    class Sensor
    {
        public Sensor(string id, string battery, DateTime timestamp, string humidity, string temperature, string floor, string location)
        {
            Id = id;
            TimeStamp = timestamp;
            Battery = battery;
            Humidity = humidity;
            Temperature = temperature;
            Floor = floor;
            Location = location;
        }

        public Sensor(string id, string battery, DateTime timestamp, string humidity, string temperature, string floor, string localizacao, string reason)
        {
            Id = id;
            TimeStamp = timestamp;
            Battery = battery;
            Humidity = humidity;
            Temperature = temperature;
            Floor = floor;
            Location = localizacao;
            Reason = reason;

        }


        public string Reason { get; }

        public string Floor { get; }
        public string Location { get; }
        public DateTime TimeStamp { get; }


        public string Humidity { get; }




        public string Id { get; }



        public string Temperature { get; }


        public string Battery { get; }





        public string Print()
        {
            string output = "Sensor ID: " + Id;
            output += " battey: " + Battery + " timestamp: " + TimeStamp;
            output += " humidity:" + Humidity + " temperature: " + Temperature;
            return output;

        }







    }


}
