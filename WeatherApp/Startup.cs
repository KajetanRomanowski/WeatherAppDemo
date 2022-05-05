using Microsoft.Owin;
using Owin;
using WeatherApp.Helpers;

[assembly: OwinStartup(typeof(WeatherApp.Startup))]

namespace WeatherApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            APIHelper.InitializeClient();
        }
    }
}
