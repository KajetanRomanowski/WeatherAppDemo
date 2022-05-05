using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLibrary.Models;
using Microsoft.AspNet.Identity;
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
            //string userId = RequestContext.Principal.Identity.GetUserId();
            //var data = LoadWeather();
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
        public void Post(Models.WeatherModel newWeather)
        {
            CreateWeather(newWeather.DateTime, newWeather.Temperature, newWeather.Humidity, newWeather.Pressure, newWeather.Wind);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
