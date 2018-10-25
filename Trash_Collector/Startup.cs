using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trash_Collector.Startup))]
namespace Trash_Collector
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
