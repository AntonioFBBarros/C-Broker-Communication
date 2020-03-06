
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShowGraphs
{
    class Place
    {
        public List<String> floors { get; set; }


        public Place(string local)
        {
            Local = local;
            floors = new List<string>();
        }


        public string Local { get; set; }



        public bool existsFloor(string floor)
        {
            foreach (var item in floors)
            {
                if (item == floor)
                {
                    return true;
                }
            }

            return false;
        }

        public void addFloor(string floor)
        {
            foreach (var item in floors)
            {
                if (item == floor)
                {
                    return;
                }
            }
            floors.Add(floor);
        }









    }


}
