
namespace MultiSeguroViagem.Domain.Entities.Enum
{
  public enum EnumPedidoStatus
  {
    PagamentoConfirmado = 1,
    AguardandoPagamento = 2,
    PedidoCancelado = 3,
    CancelamentoSolicitado = 4,
    VoucherEntregue = 5,
    CancelamentoRecusado = 6,
    PedidoVencido = 7,
    AguardandoCompensacao = 8,
    VoucherEmitidoAutomaticamente = 9,
    EmitirVoucherManualmente = 10
  }
}
