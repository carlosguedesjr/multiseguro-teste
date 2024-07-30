using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IOperadoraRepository
  {
    /// <summary>
    /// Busca dados da operadora
    /// </summary>
    /// <param name="nome">Operadora</param>
    /// <returns></returns>
    Operadora BuscaOperadora(string nome);
  }
}
