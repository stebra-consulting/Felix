using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_RedisCache2.Startup))]
namespace MVC_RedisCache2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
