using System;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Entities.Enum;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface IPagamentoService
  {
    Comprovante ObtemComprovante(int idPagamento);

    PagamentoAvulso BuscaPagamentoAvulso(int idPagamento);

    Pagamento Busca(int idPagamento);

    Pagamento ObtemPagamentoPedido(int idPedido);

    PagamentoItau ObtemPagamentoItau(int idPagamento);

    IList<PagamentoItau> ObtemBoletosAbertos();


    Pagamento Cadastra(Pedido pedido, int status, int operadora, string nome, string email, string documento, string cep, DateTime dataVencimento, decimal valor, decimal valorDescontoCupom, decimal valorDescontoBoleto, decimal valorDescontoTotal);

    PagamentoRedeCard CadastraAutorizacaoCreditoRedeCard(Pagamento pagamento, string codigoRetorno, string dataTransacao, string horaTransacao, string nomeCartao, string numeroCartao, string mensagemRetorno, string numeroAutorizacao, string numeroSequencial, string numeroPedido, long tid, int quantidadeParcelas, string bandeira);

    PagamentoRedeCard BuscaPagamentoRedeCard(int idPagamento);

    PagamentoItau CadastraDadosItau(Pagamento pagamento, string valor, string observacao, string nome, string codigoInscricao, string numeroInscricao, string cep, DateTime dataVencimento, string urlRetorno, string obsAdicional1, string obsAdicional2, string obsAdicional3);

    void CadastraPagamentoFinanceiro(Pagamento pagamento, int idVendedorResponsavel);


    void Atualiza(int idPagamento, int status, int operadora, decimal valorDescontoBoleto, decimal valorDescontoCupom, DateTime? dataPagamento);

    void AtualizaAutorizacao(int idPagamento, string codigoRetorno, string mensagem);

    void AtualizaStatus(int idPagamento, int status, int operadora, DateTime? dataPagamento);

    void AtualizaStatus(int idPagamento, int status);

    void AtualizaPagamentoFinanceiro(int idPagamento, int idFinanceiroResponsavel);

    void AtualizaDataConsultaBoleto(int idPagamentoItau, System.DateTime dataConsultaBoleto);

    void AtualizaCupom(int idPagamento, string cupom);


    void AlteraFunilRdStation(string email, string identificador, RdStationEstagioLead estagio, bool oportunidade);

    void MarcaVendaRdStation(string identificador, decimal total, string email);

    bool ValidaVencimento(int idPagamento);

    bool ValidaVencimento(DateTime dataVencimento);

    string ProcessaPagamentoItau(string urlRetorno, string identificador, string observacao, string obsAdicional1, string obsAdicional2,
                                 string obsAdicional3, string codigoEmpresaItau, string codigoItau, string valorTotalPedido, string idPedido,
                                 string nomeCompleto, string cep, string endereco, string bairro, string cidade, string estado, Pagamento pagamento);

    void ProcessaPagamentoRedeCard(Pagamento pagamento, string dataValidadeCartao, string codigoSegurancaCartao, string numeroCartao, string bandeiraCartao,
                                   string nomeCartao, string parcelas, string numeroPedido, string valorTotalPedido, string filiacaoRede, string senhaRede);

    void ProcessaCancelamentoRedeCard(Pagamento pagamento, string filiacaoRede, string senhaRede, string motivoCancelamento);

    Pagamento BuscaPagamentoPago();
  }
}
