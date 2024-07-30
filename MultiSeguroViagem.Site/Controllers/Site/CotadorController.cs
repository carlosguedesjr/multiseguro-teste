using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using System.Web;
using AutoMapper;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Models.Site;
using MultiSeguroViagem.Site.Models.Site.Cotacao;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Threading;

namespace MultiSeguroViagem.Site.Controllers.Site
{
  public class CotadorController : Controller
  {
    private readonly IEmailAgendadoService _emailAgendadoService;
    private readonly ICotacaoService _cotacaoService;
    private readonly IBlackListFraudeRepository _blacklistfraudeRepository;

    public CotadorController(IEmailAgendadoService emailAgendadoService, ICotacaoService cotacaoService, IBlackListFraudeRepository blacklistfraudeRepository)
    {
      _emailAgendadoService = emailAgendadoService;
      _cotacaoService = cotacaoService;
      _blacklistfraudeRepository = blacklistfraudeRepository;
    }

    public ActionResult Cotacao(CotacaoCotadorModel model)
    {

      if (!ModelState.IsValid)
        return View();

      try
      {
        if (Convert.ToDateTime(model.DataVolta).Subtract(Convert.ToDateTime(model.DataIda)).TotalDays >= 365)
          return RedirectToAction("365", model);

        var cotacao = _cotacaoService.RealizaCotacao(Convert.ToInt32(model.Destino), model.Cpaff, Convert.ToDateTime(model.DataIda), Convert.ToDateTime(model.DataVolta));

        if (cotacao != null)
        {
          var enUs = new CultureInfo("en-US");
          model.Planos = Mapper.Map<ICollection<Plano>, ICollection<PlanoModel>>(cotacao.Planos.OrderByDescending(x => x.MaisVendido).ThenBy(x => Convert.ToDecimal(x.PrecoComercial, enUs)).ToList());
          ViewBag.planos = JsonConvert.SerializeObject(model);

          return View(model);
        }
      }
      catch (Exception e)
      {
        ModelState.AddModelError("CotacaoInvalida", e.Message);
      }

      return View(model);
    }

    [ActionName("365")]
    public ActionResult Cotacao365(CotacaoCotadorModel model)
    {
      return View("Cotacao365", model);
    }

    [HttpGet]
    [ValidateInput(false)]
    public ActionResult _CoberturasModal(string json, string nome, string urlDownload)
    {

      var jss = new JavaScriptSerializer();
      var coberturas = jss.Deserialize<IEnumerable<CoberturaModel>>(json);
      ViewBag.Nome = nome;
      ViewBag.UrlDownload = urlDownload;

      return PartialView(coberturas);
    }


    [HttpPost]
    [ValidateInput(false)]
    public ActionResult _ListaPlanos(CotacaoCotadorModel model, string tipoFiltro, string filtro)
    {
      var enUs = new CultureInfo("en-US");
      JavaScriptSerializer jss = new JavaScriptSerializer();
      jss.MaxJsonLength = Int32.MaxValue;
      switch (tipoFiltro)
      {
        case "MaisVendido":
          if (filtro.Equals("Crescente"))
            model.Planos = model.Planos.OrderBy(x => Convert.ToDecimal(x.PrecoComercial, enUs)).ToList();

          if (filtro.Equals("MaisVendido"))
            model.Planos = model.Planos.OrderByDescending(x => x.MaisVendido).ThenBy(x => Convert.ToDecimal(x.PrecoComercial, enUs)).ToList();

          if (filtro.Equals("Decrescente"))
            model.Planos = model.Planos.OrderByDescending(x => Convert.ToDecimal(x.PrecoComercial, enUs)).ToList();
          break;

        case "Seguradora":
          if (!filtro.Equals("Seguradora"))
            model.Planos = model.Planos.Where(p => p.Operadora.Nome == filtro).OrderByDescending(p => p.MaisVendido).ThenBy(p => Convert.ToDecimal(p.PrecoComercial, enUs)).ToList();
          break;

        case "TipoViagem":
          if (filtro.Equals("Prática de Esporte"))
            model.Planos = model.Planos.Where(p => p.PraticaEsporte).ToList();

          else if (filtro.Equals("Gestante"))
            model.Planos = model.Planos.Where(p => p.Gestante).ToList();

          else if (filtro.Equals("Estudante"))
            model.Planos = model.Planos.Where(p => p.Estudante).ToList();
          break;
      }

      return PartialView("Cotador/_ListaPlanos", model);
    }

    [HttpPost]
    public ActionResult Viajante(CotacaoCotadorModel model)
    {


      if (model != null)
      {
        return RedirectToAction("Viajantes", "Viajante", model);
      }

      return View("Cotacao", model);
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult Contato365(Cotacao365Contato model)
    {


      try
      {
        var html = $"## LEAD - Cotação acima de 365 dias ##<br/> Destino: {model.Destino} <br /> Data Ida: {model.DataIda} <br /> Data Volta: {model.DataVolta} <br /> Email: {model.Email}  <br /> Nome Contato: {model.Nome} <br /> Horário Contato: {model.Horario} <br /> Telefone: {model.Telefone} <br />";

        _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, System.Configuration.ConfigurationManager.AppSettings["destinatariosCotacao365"], "LEAD - Cotação acima de 365 dias", html, html.Replace("<br />", ""));

        ViewBag.Sucesso = "Obrigado, em breve entraremos em contato!";
        ModelState.Clear();
      }

      catch (Exception)
      {
        ViewBag.Erro = "Ocorreu um problema ao enviar sua mensagem, tente novamente mais tarde!";
      }

      return View("Cotacao365");
    }

    [HttpGet]
    [ValidateInput(false)]
    public ActionResult Comparador(string idDestino, string dataIda, string dataVolta, string itens, CotacaoCotadorModel afiliado)
    {


      string cpaff = afiliado.Cpaff;
      DateTime dataIdaComp = Convert.ToDateTime(dataIda);
      DateTime dataVoltaComp = Convert.ToDateTime(dataVolta);
      if (DateTime.Compare(dataIdaComp, DateTime.Now.Date) < 0 || DateTime.Compare(dataVoltaComp, DateTime.Now.Date) < 0)
      {
        return View("CotacaoInvalida");
      }
      var cotacao = _cotacaoService.RealizaCotacao(Convert.ToInt32(idDestino), cpaff, Convert.ToDateTime(dataIda), Convert.ToDateTime(dataVolta));

      var planosId = itens.Split(',');
      var planos = cotacao.Planos.Where(x => x.IdPlano.ToString() == (planosId.Length > 0 ? planosId[0] : "0")
                                                               || x.IdPlano.ToString() == (planosId.Length > 1 ? planosId[1] : "0")
                                                               || x.IdPlano.ToString() == (planosId.Length > 2 ? planosId[2] : "0")
                                                               || x.IdPlano.ToString() == (planosId.Length > 3 ? planosId[3] : "0"))
                                              .OrderBy(x => x.PrecoComercial);
      var model = new CotacaoCotadorModel
      {
        Destino = idDestino,
        DataIda = Convert.ToDateTime(dataIda).ToString("dd/MM/yyyy"),
        DataVolta = Convert.ToDateTime(dataVolta).ToString("dd/MM/yyyy"),
        Operadora = planos.FirstOrDefault().Operadora.Nome,
        Planos = Mapper.Map<IEnumerable<Plano>, IEnumerable<PlanoModel>>(planos)
      };

      return View(model);
    }

    [HttpPost]
    [ValidateInput(false)]
    public JsonResult EnviarComparacao(EnviaComparacaoModel model)
    {
      try
      {
        _cotacaoService.CadastraLeadComparacao(model.Destino, model.DataIda, model.DataVolta, model.Email, model.PlanosId);

        EnviaComparacao(model);

        return Json(new { sucesso = true });
      }
      catch (Exception e)
      {
        return Json(new { erro = e.Message });
      }
    }

    private void EnviaComparacao(EnviaComparacaoModel model)
    {
      model.DataIda = Convert.ToDateTime(model.DataIda).ToString("yyyy-MM-dd");
      model.DataVolta = Convert.ToDateTime(model.DataVolta).ToString("yyyy-MM-dd");
      model.EmailVendedor = model.EmailVendedor ?? Constantes.ATENDIMENTO_REMETENTE;

      model.Nome = model.Nome ?? "";
      var html = RenderViewToString(ControllerContext, "~/Views/Cotador/ComparadorEmailHtml.cshtml", model);

      var texto = Regex.Replace(Regex.Replace(html, "<style type=\"text/css\">.*</style>|<.*?>", "", RegexOptions.Singleline).Trim().Replace("  ", ""), @"[\r\n]{2,}", "\n\n");
      texto = Regex.Replace(texto, @"[\t]{2,}", "");

      var vendedores = new Dictionary<string, string>
      {
        { Constantes.EMAIL_NOME_REMETENTE, Constantes.ATENDIMENTO_REMETENTE },
        { System.Configuration.ConfigurationManager.AppSettings["emailRenata"], "Renata Rocha" },
        { System.Configuration.ConfigurationManager.AppSettings["emailBruno"], "Bruno Alves" }
      };

      var nomeVendedor = vendedores.FirstOrDefault(x => x.Key == model.EmailVendedor).Value ?? Constantes.EMAIL_NOME_REMETENTE;

      _emailAgendadoService.InsereEmailComparador(model.EmailVendedor,
                                        nomeVendedor,
                                        model.Email,
                                        $"Orçamento de Seguro Viagem - {nomeVendedor}",
                                        html,
                                        texto,
                                        model.Nome);
    }

    private static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
    {
      var viewEngineResult = partial ? ViewEngines.Engines.FindPartialView(context, viewPath) : ViewEngines.Engines.FindView(context, viewPath, null);

      if (viewEngineResult == null)
        throw new FileNotFoundException("View não encontrada");

      var view = viewEngineResult.View;
      context.Controller.ViewData.Model = model;

      string resultado;

      using (var sw = new StringWriter())
      {
        var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
        view.Render(ctx, sw);
        resultado = sw.ToString();
      }

      return resultado;
    }
  }
}
