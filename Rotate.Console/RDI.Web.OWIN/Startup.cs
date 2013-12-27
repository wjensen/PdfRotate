using Owin;

namespace RDI.Web.OWIN
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            
        }
    }
}