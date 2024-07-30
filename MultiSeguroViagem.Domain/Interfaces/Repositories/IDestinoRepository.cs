using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IDestinoRepository
  {
    ICollection<Destino> Busca();

    /// <summary>
    /// Busca todos os destinhos configurados para SEO
    /// </summary>
    /// <returns>Dados do destino</returns>
    ICollection<DestinoSeo> BuscaDestinosSeo();

    /// <summary>
    /// Obtem dados do destino informado para SEO
    /// </summary>
    /// <param name="destino">Nome Destino</param>
    /// <returns>Dados do destino</returns>
    DestinoSeo BuscaDestinoSeo(string destino);
  }
}