using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class Rigs
    {
        public int id;
        public string name;
        public double lng;
        public double lat;

        public Rigs (int id, string name, double lng, double lat)
        {
            this.id = id;
            this.name = name;
            this.lat = lat;
            this.lng = lng;
        }
    }
}
