using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [Authorize]
    public class LoggedInController : Controller
    {
        public async Task<ActionResult> Index()
        {
            AuthenticatedUser user = this.Session["user"] as AuthenticatedUser;
            ViewBag.Title = "Weather Data";
            if (user != null)
            {
                var weathers = await APIHelper.GetWeather(user.Access_Token);
                return View(weathers);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> LogOut()
        {
            {
                this.Session.Clear();
                bool loggedOut = await APIHelper.LogOut();
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> Post()
        {
            AuthenticatedUser user = this.Session["user"] as AuthenticatedUser;
            if (user != null)
            {           
                WeatherModel weatherModel = await APIHelper.GetWeatherExternal();
                bool posted = await APIHelper.PostWeather(weatherModel);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
