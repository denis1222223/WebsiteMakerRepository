using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CourseProject.Startup))]
namespace CourseProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
