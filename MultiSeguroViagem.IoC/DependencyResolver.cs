using Microsoft.Practices.Unity;
using MultiSeguroViagem.Application.Services;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Services.Services;
using MultiSeguroViagem.Infra.Repositories;
using MultiSeguroViagem.Service;
using Unity.Mvc5;

namespace MultiSeguroViagem.IoC
{
  public static class DependencyResolver
  {
    #region API

    public static void ApiResolve(UnityContainer container)
    {
      //Repository
      container.RegisterType<IPlanoRepository, PlanoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IDestinoRepository, DestinoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IUsuarioRepository, UsuarioRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<ICoberturaRepository, CoberturaRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<ICupomDescontoRepository, CupomDescontoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPagamentoRepository, PagamentoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IComparacaoRepository, ComparacaoRepository>(new HierarchicalLifetimeManager());

      //Application
      container.RegisterType<IPlanoService, PlanoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IDestinoService, DestinoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IUsuarioService, UsuarioService>(new HierarchicalLifetimeManager());
      container.RegisterType<ICoberturaService, CoberturaService>(new HierarchicalLifetimeManager());
      container.RegisterType<ICupomDescontoService, CupomDescontoService>(new HierarchicalLifetimeManager());
      container.RegisterType<ICotacaoService, CotacaoService>(new HierarchicalLifetimeManager());

      //Services
      container.RegisterType<IRedeCardService, RedeCardService>(new HierarchicalLifetimeManager());

    }

    #endregion

    #region SITE

    public static void SiteResolve(UnityContainer container)
    {
      //Services
      container.RegisterType<IRedeCardService, RedeCardService>(new HierarchicalLifetimeManager());
      container.RegisterType<IItauService, ItauService>(new HierarchicalLifetimeManager());

      //Application
      container.RegisterType<IPagamentoService, PagamentoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IPedidoService, PedidoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IUsuarioService, UsuarioService>(new HierarchicalLifetimeManager());
      container.RegisterType<IContatoEmergenciaService, ContatoEmergenciaService>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmailAgendadoService, EmailAgendadoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IPlanoService, PlanoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IIntencaoService, IntencaoService>(new HierarchicalLifetimeManager());
      //container.RegisterType<IBannerService, BannerService>(new HierarchicalLifetimeManager());
      container.RegisterType<ICotacaoService, CotacaoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IDestinoService, DestinoService>(new HierarchicalLifetimeManager());


      //Repository
      container.RegisterType<IPagamentoRepository, PagamentoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPedidoRepository, PedidoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IUsuarioRepository, UsuarioRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IContatoEmergenciaRepository, ContatoEmergenciaRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmailAgendadoRepository, EmailAgendadoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPlanoRepository, PlanoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IIntencaoRepository, IntencaoRepository>(new HierarchicalLifetimeManager());
      //container.RegisterType<IBannerRepository, BannerRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IComparacaoRepository, ComparacaoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IDestinoRepository, DestinoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IOperadoraRepository, OperadoraRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IBlackListFraudeRepository, BlackListFraudeRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<ICupomDescontoRepository, CupomDescontoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IConfiguracaoHomeRepository, ConfiguracaoHomeRepository>(new HierarchicalLifetimeManager());

      System.Web.Mvc.DependencyResolver.SetResolver(new UnityDependencyResolver(container));
    }

    #endregion

    #region Servico Emissão

    public static void ServicoResolve(UnityContainer container)
    {
      //Services
      container.RegisterType<IRedeCardService, RedeCardService>(new HierarchicalLifetimeManager());
      container.RegisterType<IItauService, ItauService>(new HierarchicalLifetimeManager());

      //Application
      container.RegisterType<IPagamentoService, PagamentoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IPedidoService, PedidoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IPlanoService, PlanoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmailAgendadoService, EmailAgendadoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IContatoEmergenciaService, ContatoEmergenciaService>(new HierarchicalLifetimeManager());
      container.RegisterType<ITokenService, TokenService>(new HierarchicalLifetimeManager());
      container.RegisterType<IDiasUteisService, DiasUteisService>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmissaoVoucherService, EmissaoVoucherService>(new HierarchicalLifetimeManager());

      //Repository
      container.RegisterType<IPagamentoRepository, PagamentoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPedidoRepository, PedidoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPlanoRepository, PlanoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmailAgendadoRepository, EmailAgendadoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IContatoEmergenciaRepository, ContatoEmergenciaRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<ITokenRepository, TokenRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IDiasUteisRepository, DiasUteisRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<ILogRepository, LogRepository>(new HierarchicalLifetimeManager());
    }

    #endregion

    #region Conciliacao Boleto

    public static void ConciliacaoBoletoResolve(UnityContainer container)
    {
      // Service
      container.RegisterType<IItauService, ItauService>(new HierarchicalLifetimeManager());
      container.RegisterType<IRedeCardService, RedeCardService>(new HierarchicalLifetimeManager());

      // Application
      container.RegisterType<IIntencaoService, IntencaoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmailAgendadoService, EmailAgendadoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IPagamentoService, PagamentoService>(new HierarchicalLifetimeManager());
      container.RegisterType<IPedidoService, PedidoService>(new HierarchicalLifetimeManager());

      // Repository
      container.RegisterType<IIntencaoRepository, IntencaoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IEmailAgendadoRepository, EmailAgendadoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPagamentoRepository, PagamentoRepository>(new HierarchicalLifetimeManager());
      container.RegisterType<IPedidoRepository, PedidoRepository>(new HierarchicalLifetimeManager());
    }

    #endregion
  }
}
