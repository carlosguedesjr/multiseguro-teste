using System.Web.Http;
using Microsoft.Practices.Unity;
using MultiSeguroViagem.IoC;
using MultiSeguroViagem.Site.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiSeguroViagem.Site
{
    public class ConfigureApi
    {
        public static class WebApiConfig
        {
            public static void Register(HttpConfiguration config)
            {
                var container = new UnityContainer();
                DependencyResolver.ApiResolve(container);
                config.DependencyResolver = new UnityResolver(container);

                var formatters = config.Formatters;
                formatters.Remove(formatters.XmlFormatter);

                var jsonSettings = formatters.JsonFormatter.SerializerSettings;
#if DEBUG
                jsonSettings.Formatting = Formatting.Indented;
#endif
                jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    "DefaultApi",
                    "api/v1/{controller}/{id}",
                    new { id = RouteParameter.Optional }
                );
            }
        }
    }
}