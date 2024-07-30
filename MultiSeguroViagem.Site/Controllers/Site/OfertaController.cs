using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Models.Site;
using MultiSeguroViagem.Site.Models.Site.Cotacao;

using System.Text;



namespace MultiSeguroViagem.Site.Controllers.Site
{
  public class OfertaController : Controller
  {
    private readonly IDestinoService _destinoService;
    private readonly ICotacaoService _cotacaoService;
    private readonly IOperadoraRepository _operadoraRepository;
    private readonly IPlanoRepository _planoRepository;

    public OfertaController(IDestinoService destinoService, ICotacaoService cotacaoService, IOperadoraRepository operadoraRepository, IPlanoRepository planoRepository)
    {
      _destinoService = destinoService;
      _cotacaoService = cotacaoService;
      _operadoraRepository = operadoraRepository;
      _planoRepository = planoRepository;
    }

    public ActionResult Destino(string destino)
    {
      var destinoSeo = _destinoService.BuscaDestinoSeo(destino);

      var cotacao = _cotacaoService.RealizaCotacao(destinoSeo.IdDestino, string.Empty, DateTime.Now, DateTime.Now);

      ViewBag.Destino = destinoSeo;
      ViewBag.Planos = cotacao.Planos.OrderByDescending(x => x.MaisVendido).Take(8).ToList();
      return View();
    }

    public ActionResult Promocoes()
    {
      return View();
    }

    public ActionResult PassagensAereas()
    {
      Response.StatusCode = 404;
      return View();
     // return View();
    }

    public ActionResult Operadora(string destino, string operadora)
    {
      var destinoSeo = _destinoService.BuscaDestinoSeo(destino);

      var cotacao = _cotacaoService.RealizaCotacao(destinoSeo.IdDestino, string.Empty, DateTime.Now, DateTime.Now);

      var model = new CotacaoCotadorModel
      {
        Destino = destinoSeo.IdDestino.ToString(),
        DataIda = DateTime.Now.ToString("dd/MM/yyyy"),
        DataVolta = DateTime.Now.ToString("dd/MM/yyyy"),
        Operadora = operadora,
        Origem = "",
        Planos = Mapper.Map<IEnumerable<Plano>, IEnumerable<PlanoModel>>(cotacao.Planos.Where(x => x.Operadora.Nome.ToLower().Replace(" ", "") == operadora))
      };

      ViewBag.Destino = destinoSeo;
      ViewBag.planos = new JavaScriptSerializer().Serialize(model);
     
      ViewBag.Operadora = _operadoraRepository.BuscaOperadora(operadora);
      var operadora2 = ViewBag.Operadora;
      if (operadora == "assistmed")
      {
        if (string.IsNullOrEmpty(operadora2))
        {
          System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
          System.Web.HttpContext.Current.Response.AddHeader("Location", "https://www.multiseguroviagem.com.br/");
          return Redirect("https://www.multiseguroviagem.com.br/");
        }
      }
      else
      {
        if (string.IsNullOrEmpty(operadora2.Nome))
        {
          System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
          System.Web.HttpContext.Current.Response.AddHeader("Location", "https://www.multiseguroviagem.com.br/");
          return Redirect("https://www.multiseguroviagem.com.br/");
        }
      }
      return View(model);
    }



    public ActionResult Plano(string destino, string operadora, string plano)
    {
      
      string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
      char c = url.Last();
      if(c == '/')
      {
        System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
        System.Web.HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().ToLower().Replace(url, url.Remove(url.Length - 1)));
      }

      var destinoSeo = _destinoService.BuscaDestinoSeo(destino);
      if (destinoSeo == null)
      {
        System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
        System.Web.HttpContext.Current.Response.AddHeader("Location", "https://www.multiseguroviagem.com.br/");
        return Redirect("https://www.multiseguroviagem.com.br/");
      }

      var cotacao = _cotacaoService.RealizaCotacao(destinoSeo.IdDestino, string.Empty, DateTime.Now, DateTime.Now);
      var planoEscolhido = cotacao.Planos.FirstOrDefault(x => x.Nome.ToLower().Replace(" ", "").Replace(".", "") == plano);

      if (planoEscolhido == null)
      {
        planoEscolhido = _planoRepository.Busca(plano);
      }
      if (planoEscolhido == null)
      {
        return new HttpNotFoundResult();
      }

      ViewBag.Destino = destinoSeo;
      ViewBag.Planos = cotacao.Planos.OrderByDescending(x => x.MaisVendido).Take(4).ToList();
      ViewBag.Plano = planoEscolhido;

      return View();
    }

  }
}
