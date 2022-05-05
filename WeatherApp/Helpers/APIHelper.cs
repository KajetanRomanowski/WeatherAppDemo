using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using WeatherApp.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Helpers
{
    public static class APIHelper
    {
        public static HttpClient apiClient;

        public static void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
        }

        public static async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<bool> Register(string username, string password, string confirmPassword)
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            RegisterBindingModel model = new RegisterBindingModel();
            model.Email = username;
            model.Password = password;
            model.ConfirmPassword = confirmPassword;

            var data = JsonConvert.SerializeObject(model);

            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var  response = await apiClient.PostAsync("/api/account/register", byteContent);
            return response.IsSuccessStatusCode;
        }

        

        public static async Task<WeatherModel> GetWeather(string key, int id)
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            

            using (HttpResponseMessage response = await apiClient.GetAsync($"/api/values/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<WeatherModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<IEnumerable<WeatherModel>> GetWeather(string key)
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);


            using (HttpResponseMessage response = await apiClient.GetAsync($"/api/values"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<IEnumerable<WeatherModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<WeatherModel> GetWeatherExternal()
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await apiClient.GetAsync($"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Bydgoszcz%2C%20Poland/today?unitGroup=metric&elements=datetime%2CdatetimeEpoch%2Ctemp%2Chumidity%2Cwindspeed%2Cpressure&include=current&key=MW74S87MT5KAXCDHTMV5A4QEQ&contentType=json"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);
                    WeatherModel resultModel = new WeatherModel();
                    string dt = String.Empty;
                    string temp = String.Empty;
                    foreach (var day in result.days)
                    {
                        dt = day.datetime;
                    }
                    dt = dt + " " + result.currentConditions.datetime;

                    temp = result.currentConditions.temp;
                    if (result.currentConditions.temp == null)
                        temp = "0";
                    temp = temp.Replace(".", ",");
                    resultModel.Temperature = Convert.ToDouble(temp);

                    temp = result.currentConditions.humidity;
                    if (result.currentConditions.humidity == null)
                        temp = "0";
                    temp = temp.Replace(".", ",");
                    resultModel.Humidity = Convert.ToDouble(temp);

                    temp = result.currentConditions.pressure;
                    if (result.currentConditions.pressure == null)
                        temp = "0";
                    temp = temp.Replace(".", ",");
                    resultModel.Pressure = Convert.ToDouble(temp);

                    temp = result.currentConditions.windspeed;
                    if (result.currentConditions.windspeed == null)
                        temp = "0";
                        temp = temp.Replace(".", ",");
                    resultModel.Wind = Convert.ToDouble(temp);

                    resultModel.DateTime = Convert.ToDateTime(dt);
                    return resultModel;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<bool> PostWeather(WeatherModel weatherModel)
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = JsonConvert.SerializeObject(weatherModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await apiClient.PostAsync("/api/values", byteContent);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> LogOut()
        {
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent content = null;

            using (HttpResponseMessage response = await apiClient.PostAsync("/api/account/logout",content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}