using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Domain.Entities
{
  public class PagamentoRedeCard
  {
    #region Construtor

    protected PagamentoRedeCard() { }

    public PagamentoRedeCard(Pagamento pagamento, string codigoRetorno, System.DateTime dataTransacao, string nomeCartao, string numeroCartao, string mensagemRetorno,
                                                   string numeroAutorizacao, string numeroSequencial,string numeroPedido, long tid, int quantidadeParcelas, string bandeira)
    {
      Pagamento = pagamento;
      NomeCartao = nomeCartao;
      NumeroCartao = numeroCartao;
      NumeroPedido = numeroPedido;
      QuantidadeParcelas = quantidadeParcelas;
      Bandeira = bandeira;
      Tid = tid;
      DataTransacao = dataTransacao;
      CodigoRetorno = codigoRetorno;
      MensagemRetorno = mensagemRetorno;
      NumeroAutorizacao = numeroAutorizacao;
      NumeroSequencial = numeroSequencial;
    }

    #endregion

    #region properties

    public Pagamento Pagamento { get; private set; }
    public string NomeCartao { get; private set; }
    public string NumeroCartao { get; private set; }
    public string NumeroPedido { get; private set; }
    public int QuantidadeParcelas { get; private set; }
    public string Bandeira { get; private set; }
    public long Tid { get; private set; }
    public System.DateTime DataTransacao { get; private set; }
    public string CodigoRetorno { get; private set; }
    public string MensagemRetorno { get; private set; }
    public string NumeroAutorizacao { get; private set; }
    public bool Autorizado { get; private set; }
    public string NumeroSequencial { get; private set; }
    public string MotivoEstorno { get; private set; }
    public System.DateTime? DataCancelamento { get; private set; }

    #endregion

    #region methods

    public void Valida()
    {
      AssertionConcern.AssertArgumentNotNull(Pagamento, "O pagamento não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(NomeCartao, "O nome do cartão não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(NumeroCartao, "O número do cartão não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(QuantidadeParcelas, "O número do cartão não pode ser nulo");
    }

    public void DefineMensagemRetorno(string mensagem)
    { MensagemRetorno = mensagem; }

    public void DefineMotivoEstorno(string motivoEstorno)
    { MotivoEstorno = motivoEstorno; }

    public void DefineNumeroSequencial(string numeroSequencial)
    { NumeroSequencial = numeroSequencial; }

    public void DefineDataCancelamento(System.DateTime data)
    { DataCancelamento = data; }

    #endregion
  }
}
