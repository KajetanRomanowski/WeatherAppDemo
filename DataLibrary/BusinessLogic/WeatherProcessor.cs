using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class WeatherProcessor
    {
        public static int CreateWeather(DateTime dateTime, double temperature, double humidity,
            double pressure, double wind)
        {
            WeatherModel data = new WeatherModel
            {
                DateTime = dateTime,
                Temperature = temperature,
                Humidity = humidity,
                Pressure = pressure,
                Wind = wind
            };
            string sql = @"insert into dbo.Weather (DateTime, Temperature, Humidity, Pressure, Wind) 
                            values (@DateTime, @Temperature, @Humidity, @Pressure, @Wind);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<WeatherModel> LoadWeather()
        {
            string sql = @"select ID, DateTime, Temperature, Humidity, Pressure, Wind
                            from dbo.Weather order by ID desc;";
            return SqlDataAccess.LoadData<WeatherModel>(sql);
        }
    }
}
