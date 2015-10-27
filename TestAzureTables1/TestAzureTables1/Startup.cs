using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestAzureTables1.Startup))]
namespace TestAzureTables1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
