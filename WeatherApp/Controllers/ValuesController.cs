using DataLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using static DataLibrary.BusinessLogic.WeatherProcessor;

namespace WeatherApp.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private List<WeatherModel> data = LoadWeather();
        // GET api/values
        public IHttpActionResult Get()
        {
            if (data == null)
            return Content(HttpStatusCode.NotFound, "Requested data has not been found.");
            return Ok(data);
        }
        
        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            WeatherModel weather = data.FirstOrDefault(d => d.ID == id);
            if (weather == null)
                return Content(HttpStatusCode.NotFound, "Requested data has not been found.");
            return Ok(weather);
        }



        // POST api/values
        [Authorize(Roles = "Admin")]
        public void Post(Models.WeatherModel newWeather)
        {
            CreateWeather(newWeather.DateTime, newWeather.Temperature, newWeather.Humidity, newWeather.Pressure, newWeather.Wind);
        }

    }
}
