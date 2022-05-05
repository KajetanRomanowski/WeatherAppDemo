using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class WeatherModel
    {
        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double Wind { get; set; }
    }
}