using System.Web.Mvc;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Models.Site;

namespace MultiSeguroViagem.Site.Controllers.Site
{
  public class SobreController : Controller
  {
    private readonly IEmailAgendadoService _emailAgendadoService;
    private readonly IDestinoService _destinoService;

    public SobreController(IEmailAgendadoService emailAgendadoService, IDestinoService destinoService)
    {
      _emailAgendadoService = emailAgendadoService;
      _destinoService = destinoService;
    }

    public ActionResult Index()
    {
      return View("Sobre");
    }

    public ActionResult Destinos()
    {
      var destinos = _destinoService.BuscaDestinosSeo();

      return View(destinos);
    }

    public ActionResult PoliticaDePrivacidade()
    {
      return View();
    }

    public ActionResult Faq()
    {
      return View();
    }

    public ActionResult Coberturas()
    {
      return View();
    }

    public ActionResult ComoComprar()
    {
      return View();
    }

    public ActionResult Afiliados()
    {
            Response.StatusCode = 404;
            return View();
    }

    public ActionResult OQueESeguroViagem()
    {
      return View();
    }

    public ActionResult Depoimentos()
    {
      return View();
    }

    public ActionResult TratadodeSchengen()
    {
      return View();
    }

    public ActionResult Contato()
    {
      return View();
    }

    [HttpPost]
    public ActionResult EnviarFormularioContato(FormumarioContatoModel model)
    {
      try
      {
        if (!ModelState.IsValid)
          return View("Contato", model);

        var html = $"Recebemos um contato através do nosso formulário e os dados são:<br/> Nome:  {model.Nome} <br /> Email:  {model.Email} <br /> Telefone: {model.Telefone} <br /><br /> Mensagem: <br /> {model.Mensagem} ";
        //var texto = html.Replace("<br />", "");

        _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, System.Configuration.ConfigurationManager.AppSettings["emailDestinatarioContato"], "Contato - No que podemos ajudar?", html, html.Replace("<br />", ""));

        ViewBag.Sucesso = "Obrigado, em breve entraremos em contato!";
        ModelState.Clear();
      }
      catch (System.Exception e)
      {
        ViewBag.Erro = "Ocorreu um problema ao enviar sua mensagem, tente novamente mais tarde!";
      }

      return View("Contato");
    }

    public ActionResult EnviarFormularioAfiliado(FormularioAfiliadoModel model)
    {
      try
      {
        if (!ModelState.IsValid)
          return View("Afiliados", model);

        var html = $"Nome:  {model.Nome} <br /> Email:  {model.Email} <br /> Telefone: {model.Telefone} <br /><br /> Mensagem: <br /> {model.Mensagem} ";
        var texto = html.Replace("<br />", "");
        _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, System.Configuration.ConfigurationManager.AppSettings["emailDestinatarioAfiliados"], "Contato - Torne-se afiliado", html, texto);
        ViewBag.Sucesso = "Obrigado, em breve entraremos em contato!";
        ModelState.Clear();

      }
      catch (System.Exception)
      {
        ViewBag.Erro = "Ocorreu um problema ao enviar sua mensagem, tente novamente mais tarde!";
      }
      return View("Afiliados");
    }
  }
}