using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            {
                ViewBag.Title = "Log In";
                
                if (this.Session["user"]==null)
                    return View();
                else
                    return RedirectToAction("Index", "LoggedIn");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection formCollection)
        {
            {
                try
                {
                    ViewBag.Title = "Log In";
                    AuthenticatedUser user = new AuthenticatedUser();
                    user = await APIHelper.Authenticate(formCollection[1], formCollection[2]);
                    this.Session["user"] = user;
                    return RedirectToAction("Index", "LoggedIn");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
        }


        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Title = "Register";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(FormCollection formCollection)
        {
            ViewBag.Title = "User Sign Up";
            bool registered = await APIHelper.Register(formCollection[1], formCollection[2], formCollection[3]);

            return RedirectToAction("Index");
            
        }

        
        
    }
}
