using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(x_nova_template.Startup))]
namespace x_nova_template
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
