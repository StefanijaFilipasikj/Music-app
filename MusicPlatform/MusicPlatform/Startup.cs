using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicPlatform.Startup))]
namespace MusicPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
