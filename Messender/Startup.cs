using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Messender.Startup))]
namespace Messender
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
