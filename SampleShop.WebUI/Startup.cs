using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleShop.WebUI.Startup))]
namespace SampleShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
