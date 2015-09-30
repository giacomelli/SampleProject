using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ProjectSample.WebApi.Startup))]

namespace ProjectSample.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
