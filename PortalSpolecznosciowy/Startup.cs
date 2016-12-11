using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortalSpolecznosciowy.Startup))]
namespace PortalSpolecznosciowy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
