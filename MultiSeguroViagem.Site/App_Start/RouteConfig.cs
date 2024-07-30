using System.Web.Mvc;
using System.Web.Routing;

namespace MultiSeguroViagem.Site
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
        name: "SeguroViagem",
        url: "seguro-viagem",
        defaults: new { controller = "SeguroViagem", action = "Index" }
      );

      routes.MapRoute(
        name: "SeguroViagemIntercambio",
        url: "seguro-viagem-intercambio",
        defaults: new { controller = "SeguroViagem", action = "SeguroViagemIntercambio" }
      );

      routes.MapRoute(
        name: "SeguroViagemPortugues",
        url: "seguro-viagem-para-estrangeiro",
        defaults: new { controller = "SeguroViagem", action = "SeguroViagemPortugues" }
      );

      routes.MapRoute(
        name: "SeguroViagemIngles",
        url: "travelers-insurance-brazil",
        defaults: new { controller = "SeguroViagem", action = "SeguroViagemIngles" }
      );

     routes.MapRoute(
        name: "SeguroViagemEspanhol",
        url: "asistencia-al-viajero-brasil",
        defaults: new { controller = "SeguroViagem", action = "SeguroViagemEspanhol" }
      );


      routes.MapRoute(
        name: "SeguroViagemInternacional",
        url: "seguro-viagem-internacional",
        defaults: new { controller = "SeguroViagem", action = "SeguroViagemInternacional" }
      );

      routes.MapRoute(
       name: "SeguroViagemEuropa",
       url: "seguro-viagem-europa",
       defaults: new { controller = "SeguroViagem", action = "SeguroViagemEuropa" }
     );

      routes.MapRoute(
       name: "SeguroViagemCovid19",
       url: "seguro-viagem-covid-19",
       defaults: new { controller = "SeguroViagem", action = "SeguroViagemCovid19" }
      );

      routes.MapRoute(
        name: "Comparador",
        url: "cotador/comparador/{idDestino}/{dataIda}/{dataVolta}",
        defaults: new { controller = "Cotador", action = "Comparador" }
      );

      routes.MapRoute(
        name: "DestinoSeo",
        url: "oferta/{destino}",
        defaults: new { controller = "Oferta", action = "Destino" }
      );

      routes.MapRoute(
        name: "OperadoraSeo",
        url: "oferta/{destino}/{operadora}",
        defaults: new { controller = "Oferta", action = "Operadora" }
      );

      routes.MapRoute(
        name: "PlanoSeo",
        url: "oferta/{destino}/{operadora}/{plano}",
        defaults: new { controller = "Oferta", action = "Plano" }
      );

      routes.MapRoute(
      name: "Promocoes",
      url: "promocoes",
      defaults: new { controller = "Oferta", action = "Promocoes" }
    );

      routes.MapRoute(
      name: "PassagensAereas",
      url: "passagens-aereas-hoteis",
      defaults: new { controller = "Oferta", action = "PassagensAereas" }
   );

   routes.MapRoute(
     name: "VoucherOnline",
     url: "voucher-online",
     defaults: new { controller = "Login", action = "VoucherOnline" }
   );

      routes.MapRoute(
            name: "Amp",
            url: "amp",
            defaults: new { controller = "Home", action = "IndexAmp" }
          );


      routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );

      
    }
  }
}
