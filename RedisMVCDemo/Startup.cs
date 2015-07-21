using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RedisMVCDemo.Startup))]
namespace RedisMVCDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
