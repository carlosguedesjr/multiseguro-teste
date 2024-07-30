namespace MultiSeguroViagem.Domain.Interfaces.Services.Services
{
  public interface IItauService
  {
    string GeraCripto(string idPedido, string codigoEmpresa, string valor, string observacao, string chaveItau, string nome,
                      string codigoInscricao, string numeroInscricao, string endereco, string bairro, string cep, string cidade, string estado,
                      string dataVencimento, string urlRetorno, string obsAdicional1, string obsAdicional2, string obsAdicional3);

    string SituacaoPagamento(string codigoEmpresa, string chaveItau, string pedido);
  }
}
