using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureBlobModule.Startup))]
namespace AzureBlobModule
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
