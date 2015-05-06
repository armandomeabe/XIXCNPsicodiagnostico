using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AcreditacionesBackend.Startup))]
namespace AcreditacionesBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
