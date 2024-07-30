
using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IPlanoRepository
  {
    Plano Busca(string nome);
    ICollection<Plano> BuscaPlanos(int idDestino, int quantidadeDias);
    IList<ArquivoUpload> ObtemArquivosUploadPlanos(int idPlano);
    IList<TarifasAssistCard> ObtemTarifasAssisCard(int idPlano);
  }
}
