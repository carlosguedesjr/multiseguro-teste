using MultiSeguroViagem.Domain.Entities;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IPagamentoRepository
  {
    Pagamento Busca(int idPagamento);
    Pagamento ObtemPagamentoPedido(int idPedido);
    Pagamento BuscaPagamentoPago();

    PagamentoAvulso BuscaPagamentoAvulso(int idPagamento);
    
    Comprovante ObtemComprovante(int idPagamento);
    
    PagamentoItau ObtemPagamentoItau(int idPagamento);
    IList<PagamentoItau> ObtemBoletosAbertos();

    
    IEnumerable<string> CuponsUtilizados(string documento);


    Pagamento Cadastra(Pagamento pagamento);

    PagamentoRedeCard CadastraAutorizacaoCreditoRedeCard(PagamentoRedeCard rede);
    PagamentoRedeCard BuscaPagamentoRedeCard(int idPagamento);

    PagamentoItau CadastraDadosItau(PagamentoItau itau);

    void CadastraPagamentoFinanceiro(Pagamento pagamento, int idVendedorResponsavel);


    void Atualiza(int idPagamento, int status, int operadora, decimal valorDescontoBoleto, decimal valorDescontoCupom, System.DateTime? dataPagamento);
    void AtualizaPagamentoFinanceiro(int idPagamento, int idFinanceiroResponsavel);
    void AtualizaAutorizacao(int idPagamento, string codigoRetorno, string mensagem);
    void AtualizaDataConsultaBoleto(int idPagamentoItau, System.DateTime dataConsultaBoleto);
    void AtualizaCupom(int idPagamento, string cupom);
    void AtualizaStatus(int idPagamento, int status, int operadora, System.DateTime? dataPagamento);     
    void AtualizaStatus(int idPagamento, int status);
    void AtualizaCancelamentoRedeCard(int idPagamento, string mensagemRetorno, string motivoEstorno, System.DateTime? dataCancelamento);
  }
}
