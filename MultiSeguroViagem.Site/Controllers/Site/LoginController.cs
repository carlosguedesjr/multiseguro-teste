using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Helpers;
using MultiSeguroViagem.Site.Models.Site;
using MultiSeguroViagem.Site.Models.Site.Login;
using static MultiSeguroViagem.Common.Helpers.Funcoes;


namespace MultiSeguroViagem.Site.Controllers.Site
{
  public class LoginController : Controller
  {
    private readonly IPedidoService _pedidoService;
    private readonly IUsuarioService _usuarioService;
    private readonly IPlanoService _planoService;
    private readonly IEmailAgendadoService _emailAgendadoService;
    private readonly IPagamentoService _pagamentoService;

    public LoginController(IPedidoService pedidoService, IUsuarioService usuarioService, IPlanoService planoService, IEmailAgendadoService emailAgendadoService, IPagamentoService pagamentoService)
    {
      _pedidoService = pedidoService;
      _usuarioService = usuarioService;
      _planoService = planoService;
      _emailAgendadoService = emailAgendadoService;
      _pagamentoService = pagamentoService;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Login(LoginCadastroModel model)
    {
      try
      {
        if (!ModelState.IsValid)
          throw new Exception("Usuário e/ou Senha inválidos!");

        var usuario = _usuarioService.Autenticacao(model.Login.Email, model.Login.Senha);

        var cookie = new Cookie();
        cookie.SetCookie(FormsAuthentication.FormsCookieName, string.Concat(usuario.Nome.Split(' ')[0], "#", model.Login.Email), 3600, Response);
      }
      catch (Exception e)
      {
        ViewBag.ErroLogin = e.Message;
        return View("Index", model);
      }

      return RedirectToAction("Conta", "Login");
    }

    public ActionResult VoucherOnline()
    {
      return View();
    }

    public ActionResult DetalhesVoucherOnline(int IdPedido, string CPF)
    {
      try
      {
        var viajantes = _pedidoService.ObtemPedidoViajantes(IdPedido);
        var bitConfereVoucher = 0;
        foreach (var viajante in viajantes) {
          if (viajante.Cpf.Equals(CPF.Replace('.',' ').Replace('-',' ').Trim().RemoveEspacos()))
          {
            bitConfereVoucher = 1;
          }
        }
        if (bitConfereVoucher == 1)
        {
          var pedidos = Mapper.Map<IList<Pedido>, IList<PedidoModel>>(_pedidoService.ObtemPedidoDetalhes(IdPedido));
          var pedido = ((List<PedidoModel>)pedidos).Find(x => x.IdPedido == IdPedido);
          ViewBag.Viajantes = viajantes;
          //Recupera Condições gerais
          var arquivosUploads = new List<ArquivoUpload>();

          foreach (var item in pedido.Itens)
          {
            arquivosUploads.Add(_planoService.ObtemArquivosUploadPlanos(item.Plano.IdPlano).FirstOrDefault(x => x.TipoArquivo == "CondicaoGeral"));
          }

          ViewBag.CondicoesGerais = arquivosUploads;
          return View(pedido);
        }
        Session["bitEncontrado"] = 0;

        return RedirectToAction("VoucherOnline", "Login");
      }
      catch (Exception ex)
      {
        return RedirectToAction("VoucherOnline", "Login");
      }
      
    }

    public ActionResult Conta()
    {
      if (!User.Identity.IsAuthenticated) return RedirectToAction("Index", "Login");

      var usuario = Mapper.Map<Usuario, UsuarioModel>(_usuarioService.BuscaPorEmail(User.Identity.Name.Split('#')[1]));
      var pedidos = Mapper.Map<IList<Pedido>, IList<PedidoModel>>(_pedidoService.ObtemPedidos(usuario.IdUsuario));

      ViewBag.Pedidos = pedidos;
      TempData["PersistePedidos"] = pedidos;

      return View(usuario);
    }

    public ActionResult SalvaDados(UsuarioModel model)
    {
      try
      {
        ViewBag.Dados = true;

        if (TempData["PersistePedidos"] != null)
        {
          ViewBag.Pedidos = TempData["PersistePedidos"];
          TempData.Keep("PersistePedidos");
        }

        if (!ModelState.IsValid)
          return View("Conta", model);

        _usuarioService.Atualiza(model.IdUsuario, model.Nome, model.Email, model.NovaSenha, model.Telefone, model.Documento, model.Cep,
                                model.Endereco, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado.Substring(2));

        ViewBag.DadosAtualizados = "Dados atualizados!";

      }
      catch (Exception ex)
      {
        ViewBag.Erro = "Ocorreu um problema ao atualizar os dados!";
      }
      return View("Conta", model);
    }

    public ActionResult Cadastro(LoginCadastroModel model)
    {
      try
      {
        var usuario = _usuarioService.Cadastra(model.Cadastro.Nome, model.Cadastro.Email, model.Cadastro.Senha, null, null, null, null, null, null, null, null, null);

        Login(new LoginCadastroModel { Login = { Email = usuario.Email, Senha = model.Cadastro.Senha } });

        return RedirectToAction("Conta", "Login");
      }
      catch (Exception e)
      {
        ViewBag.ErroCadastro = e.Message;
        return View("Index", model);
      }
    }

    public ActionResult MaisDetalhes(int idPedido)
    {
      try
      {
        if (TempData["PersistePedidos"] != null)
        {
          var pedido = ((List<PedidoModel>)TempData["PersistePedidos"]).Find(x => x.IdPedido == idPedido);
          TempData.Keep("PersistePedidos");

          ViewBag.Viajantes = _pedidoService.ObtemPedidoViajantes(pedido.IdPedido);

          //Recupera Condições gerais
          var arquivosUploads = new List<ArquivoUpload>();

          foreach (var item in pedido.Itens)
          {
            arquivosUploads.Add(_planoService.ObtemArquivosUploadPlanos(item.Plano.IdPlano).FirstOrDefault(x => x.TipoArquivo == "CondicaoGeral"));
          }

          ViewBag.CondicoesGerais = arquivosUploads;

          return View(pedido);
        }
      }
      catch (Exception ex)
      {
        return RedirectToAction("Conta");
      }

      return RedirectToAction("Conta");
    }

    public ActionResult PagarAgora(int idPedido)
    {
      var pagamento = _pagamentoService.ObtemPagamentoPedido(idPedido);

      if (pagamento != null)
        return RedirectToAction("Avulso", "Pagamento", new { p = Encrypt(idPedido.ToString()), i = Encrypt(pagamento.IdPagamento.ToString()) });

      return View("Conta");
    }

    public ActionResult EsqueciSenha(string email)
    {
      try
      {
        var senha = _usuarioService.ResetaSenha(email);

        var html = RenderRazorViewToString("~/Views/Email/_EsqueciMinhaSenha.cshtml", senha);
        var texto = Regex.Replace(Regex.Replace(html, "<.*?>", string.Empty).Trim().Replace("  ", ""), @"[\r\n]{2,}", "\n\n");

        html = Regex.Replace(html, "[\r\n]{2,}", "").Trim().Replace("  ", "");

        _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, email, "Multi Seguro Viagem - Nova Senha", html, texto);
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
      }

      return Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }

    public void LogOut()
    {
      var cookie = new Cookie();
      cookie.RemoveCookie(FormsAuthentication.FormsCookieName, Response);
    }

    private string RenderRazorViewToString(string viewName, object model)
    {
      ViewData.Model = model;

      using (var sw = new StringWriter())
      {
        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

        viewResult.View.Render(viewContext, sw);

        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

        return sw.GetStringBuilder().ToString();
      }
    }
  }
}
