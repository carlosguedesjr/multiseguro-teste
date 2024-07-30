using System;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public  interface IComparacaoRepository
  {
    void CadastraLead(string destino, DateTime dataIda, DateTime dataVolta, string email, string planosId);
  }
}
