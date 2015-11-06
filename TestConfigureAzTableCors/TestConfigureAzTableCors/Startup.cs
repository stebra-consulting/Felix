using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestConfigureAzTableCors.Startup))]
namespace TestConfigureAzTableCors
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
