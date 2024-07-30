using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface ICotacaoService
  {
    Cotacao RealizaCotacao(int idDestino, string cupomAfiliado, System.DateTime dataIda, System.DateTime dataVolta);

    void CadastraLeadComparacao(string destino, string dataIda, string dataVolta, string email, string planosId);
  }
}
