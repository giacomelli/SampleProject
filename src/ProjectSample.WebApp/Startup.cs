using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectSample.WebApp.Startup))]
namespace ProjectSample.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
