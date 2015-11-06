using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureWebEndPoint.Startup))]
namespace AzureWebEndPoint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
