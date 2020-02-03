using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    class Temperature
    {
        public Temperature() : this(DateTime.Now) { }
        public Temperature(DateTime date) : this(date, 0.0) { }
        public Temperature(double celsius) : this(DateTime.Now, celsius) { }
        public Temperature(DateTime date, double celsius)
        {
            Date = date;
            Celsius = celsius;
        }
        public int ID { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public double Celsius { get; set; } = 0.0;
        public double Kelvin
        {
            get
            {
                return Celsius + 273.15;
            }
            set
            {
                Celsius = value - 273.15;
            }
        }
        public double Fahrenheit
        {
            get
            {
                return Celsius * 1.8 + 32.0;
            }
            set
            {
                Celsius = (value - 32.0) / 1.8;
            }
        }

    }
}
