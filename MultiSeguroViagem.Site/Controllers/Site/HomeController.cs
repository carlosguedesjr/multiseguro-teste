using System;
using System.Web.Mvc;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Models.Site;
using MultiSeguroViagem.Site.Models.Site.Cotacao;
using System.Net;
using System.IO;



namespace MultiSeguroViagem.Site.Controllers.Site
{
  public class HomeController : Controller
  {
    private readonly IEmailAgendadoService _emailAgendadoService;
    private readonly IIntencaoService _intencaoService;
    private readonly IConfiguracaoHomeRepository _configuracaoHomeRepository;
    private readonly IBlackListFraudeRepository _blacklistfraudeRepository;

    public HomeController(IEmailAgendadoService emailAgendadoService, IIntencaoService intencaoService, IConfiguracaoHomeRepository configuracaoHomeRepository, IBlackListFraudeRepository blacklistfraudeRepository)
    {
      _emailAgendadoService = emailAgendadoService;
      _intencaoService = intencaoService;
      _configuracaoHomeRepository = configuracaoHomeRepository;
      _blacklistfraudeRepository = blacklistfraudeRepository;


    }

    public ActionResult Index()
    {

      var configuracoes = _configuracaoHomeRepository.Busca();

      ViewBag.Configuracoes = configuracoes;

      return View("");
    }

    public ActionResult IndexAmp()
    {

      var configuracoes = _configuracaoHomeRepository.Busca();

      ViewBag.Configuracoes = configuracoes;

      return View("IndexAmp");
    }

    public ActionResult Cotacao(CotacaoHomeModel model)
    {
      

      if (!ModelState.IsValid)
        return View("Index", model);

      if (Convert.ToDateTime(model.DataVolta).Subtract(Convert.ToDateTime(model.DataIda)).TotalDays >= 365)
      {
        var html = $"## LEAD - Cotação acima de 365 dias ##<br/> Destino: {_intencaoService.TrataDestino(model.Destino)} <br /> Data Ida: {model.DataIda} <br /> Data Volta: {model.DataVolta} <br /> Email: {model.Email} <br />";
        _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, System.Configuration.ConfigurationManager.AppSettings["destinatariosCotacao365"], "LEAD - Cotação acima de 365 dias", html, html.Replace("<br />", ""));
      }

      _intencaoService.Cadastra(model.Destino, Convert.ToDateTime(model.DataIda), Convert.ToDateTime(model.DataVolta), model.Email, model.Nome, model.Telefone, model.Origem, Request.UrlReferrer?.ToString(), Request.UserHostAddress);

      return RedirectToAction("Cotacao", "Cotador", model);
    }

    public ActionResult NoScript()
    {
      

      return View();
    }

    [HttpPost]
    [ActionName("Contato")]
    public ActionResult NoScriptContato(FormumarioContatoModel model)
    {
     

      try
      {
        if (!ModelState.IsValid)
          return View("NoScript", model);

        var html = $"## LEAD - Formulário NoScript ##<br/> Nome:  {model.Nome} <br /> Email:  {model.Email} <br /> Telefone: {model.Telefone} <br />";

        _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, "atendimento@multiseguroviagem.com.br", "LEAD - MultiSeguroViagem", html, html.Replace("<br />", ""));

        ViewBag.Sucesso = "Obrigado, em breve entraremos em contato!";
        ModelState.Clear();
      }

      catch (System.Exception e)
      {
        ViewBag.Erro = "Ocorreu um problema ao enviar sua mensagem, tente novamente mais tarde!";
      }

      return View("NoScript");
    }
  }
}
