using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleProject.WebApp.Startup))]
namespace SampleProject.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
