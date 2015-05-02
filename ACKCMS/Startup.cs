using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACKCMS.Startup))]
namespace ACKCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
