using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SampleProject.WebApi.Startup))]

namespace SampleProject.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
