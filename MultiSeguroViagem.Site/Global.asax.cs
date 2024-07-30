using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MultiSeguroViagem.Site.Mappers;
using System.Web.Http;
using static MultiSeguroViagem.Site.ConfigureApi;

namespace MultiSeguroViagem.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoMapperConfig.RegisterMappings();

            var container = new UnityContainer();
            IoC.DependencyResolver.SiteResolve(container);

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
