using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
    public interface IPlanoService
    {
        ICollection<Plano> BuscaPlanos(int idDestino, System.DateTime dataIda, System.DateTime dataVolta);

        IList<ArquivoUpload> ObtemArquivosUploadPlanos(int idPlano);

        string DefineCodigoTarifaAssisCard(int idPlano, System.DateTime dataIda, System.DateTime dataVolta);
    }
}