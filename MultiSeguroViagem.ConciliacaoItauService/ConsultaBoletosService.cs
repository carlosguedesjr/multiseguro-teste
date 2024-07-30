using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Entities.Enum;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using RestSharp;
using System;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;
using System.Xml;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Services;

namespace MultiSeguroViagem.ConciliacaoItauService
{
  public partial class ConsultaBoletosService : ServiceBase
  {
    private readonly Timer _tempoExecucao = new Timer();

    private readonly IIntencaoService _intencaoService;
    private readonly IEmailAgendadoService _emailAgendadoService;
    private readonly IPagamentoService _pagamentoService;
    private readonly IItauService _itauService;
    private readonly IPedidoService _pedidoService;

    public ConsultaBoletosService(IIntencaoService intencaoService, IEmailAgendadoService emailAgendadoService, IPagamentoService pagamentoService, IItauService itauService, IPedidoService pedidoService)
    {
      _intencaoService = intencaoService;
      _emailAgendadoService = emailAgendadoService;
      _pagamentoService = pagamentoService;
      _itauService = itauService;
      _pedidoService = pedidoService;

      InitializeComponent();
    }

    protected override void OnStart(string[] args)
    {
      //System.Threading.Thread.Sleep(15000);
      _tempoExecucao.Elapsed += ConsultaBoleto_Elapsed;
      _tempoExecucao.Start();
    }

    protected override void OnStop()
    {
      _tempoExecucao?.Dispose();

      _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, System.Configuration.ConfigurationManager.AppSettings["emailPausaServico"], "Conciliação boleto", "Conciliação boleto - Serviço parado", "Conciliação boleto - Serviço parado");
    }

    private void ConsultaBoleto_Elapsed(object sender, ElapsedEventArgs e)
    {
      _tempoExecucao.Stop();

      bkwConsultaBoleto.RunWorkerAsync();
    }

    private void bkwConsultaBoleto_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      var boletos = _pagamentoService.ObtemBoletosAbertos();

      foreach (var boleto in boletos)
      {
        try
        {
          var response = ConsultaSituacaoBoleto(boleto);

          if (response.StatusCode != System.Net.HttpStatusCode.OK)
            continue;

          var xmlDoc = new XmlDocument();
          xmlDoc.LoadXml(response.Content);

          var situacaoPagamento = xmlDoc.DocumentElement.SelectSingleNode("//PARAMETER/PARAM[@ID='sitPag']").Attributes["VALUE"].Value;

          if (string.IsNullOrEmpty(situacaoPagamento) || !string.Equals(situacaoPagamento, "00"))
            continue;

          var dataPagamento = DateTime.ParseExact(xmlDoc.DocumentElement.SelectSingleNode("//PARAMETER/PARAM[@ID='dtPag']").Attributes["VALUE"].Value, "ddMMyyyy", CultureInfo.InvariantCulture);

          _pagamentoService.AtualizaStatus(boleto.Pagamento.IdPagamento, (int)EnumPagamentoStatus.Pago, (int)EnumPagamentoOperadora.Itau, dataPagamento);

          _pagamentoService.AtualizaPagamentoFinanceiro(boleto.Pagamento.IdPagamento, Constantes.VENDEDOR_PADRAO);

          _pagamentoService.MarcaVendaRdStation(System.Configuration.ConfigurationManager.AppSettings["rdStationIdentificador"], Convert.ToDecimal(boleto.Valor, new CultureInfo("pt-BR")), boleto.Pagamento.Email);

          _intencaoService.AtualizaStatusPagamento(boleto.Pagamento.IdPagamento);

          if (boleto.Pagamento.Pedido != null && boleto.Pagamento.Pedido.IdPedido > 0)
            _pedidoService.AtualizaStatus((int)EnumPedidoStatus.PagamentoConfirmado, boleto.Pagamento.Pedido.IdPedido);

          var html = $"Pagamento de ID {boleto.Pagamento.IdPagamento} conciliado!<br/>Para consultar o pagamento no Itaú, utilizar o número de pedido {boleto.Pedido}";
          var texto = html.Replace("<br />", "");

          _emailAgendadoService.InsereEmail(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, System.Configuration.ConfigurationManager.AppSettings["emailConciliacao"], "Conciliação Boleto - Itau", html, texto);
        }

        finally
        {
          _pagamentoService.AtualizaDataConsultaBoleto(boleto.IdPagamentoItau, DateTime.Now);
        }
      }
    }

    private void bkwConsultaBoleto_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
      var intervalo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["intevaloConsulta"]);

      _tempoExecucao.Interval = intervalo;
      _tempoExecucao.Start();
    }


    private IRestResponse ConsultaSituacaoBoleto(PagamentoItau item)
    {
      var situacaoCripto = _itauService.SituacaoPagamento(Constantes.CREDENCIAL_ITAU_CODEMPRESA, Constantes.CREDENCIAL_ITAU_CODITAU, item.Pedido);

      var request = new RestRequest(Method.POST);
      request.AddHeader("cache-control", "no-cache");
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", "DC=" + situacaoCripto, ParameterType.RequestBody);

      var client = new RestClient(Constantes.URI_ITAU_CONSULTA);
      var response = client.Execute(request);

      return response;
    }
  }
}