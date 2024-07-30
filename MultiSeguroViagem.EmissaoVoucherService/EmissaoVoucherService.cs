using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Timers;

namespace MultiSeguroViagem.EmissaoVoucherService
{
  public partial class EmissaoVoucherService : ServiceBase
  {
    private readonly Timer _tempoVoucher = new Timer();
    private readonly IPedidoService _pedidoService;
    private readonly IPagamentoService _pagamentoService;
    private readonly IContatoEmergenciaService _contatoEmergenciaService;
    private readonly IEmissaoVoucherService _emissaoService;

    public EmissaoVoucherService(IPedidoService pedidoService, IPagamentoService pagamentoService, IContatoEmergenciaService contatoEmergenciaService, IEmissaoVoucherService emissaoService)
    {
      InitializeComponent();

      _pedidoService = pedidoService;
      _pagamentoService = pagamentoService;
      _contatoEmergenciaService = contatoEmergenciaService;
      _emissaoService = emissaoService;
    }

    protected override void OnStart(string[] args)
    {

      _tempoVoucher.Interval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["intevaloTempoExecucaoVoucher"] ?? "60") * 1000;
      _tempoVoucher.Elapsed += Voucher_Elapsed;
      _tempoVoucher.Start();
    }

    protected override void OnStop()
    {
      _tempoVoucher.Enabled = false;
    }

    private void Voucher_Elapsed(object sender, ElapsedEventArgs e)
    {
      _tempoVoucher.Stop();
      bkwEmiteVoucher.RunWorkerAsync();
    }

    private void bkwEmiteVoucher_DoWork(object sender, DoWorkEventArgs e)
    {
      var pagamento = _pagamentoService.BuscaPagamentoPago();

      if (pagamento == null)
        return;

      var viajantes = _pedidoService.ObtemPedidoViajantes(pagamento.Pedido.IdPedido);
      var contatoemergencia = _contatoEmergenciaService.BuscaContato(pagamento.Pedido.Usuario.IdUsuario);

      _emissaoService.EmissaoVoucher(pagamento, viajantes, contatoemergencia);
    }

    private void bkwEmiteVoucher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      _tempoVoucher.Start();
    }
  }
}
