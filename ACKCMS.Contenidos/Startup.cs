using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACKCMS.Contenidos.Startup))]
namespace ACKCMS.Contenidos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
