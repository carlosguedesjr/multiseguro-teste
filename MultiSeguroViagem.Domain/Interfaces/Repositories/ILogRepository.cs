using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface ILogRepository
  {
    void FalhaEmissaoVoucher(Pagamento pagamento, IList<Viajante> viajantes, string nomeOperadora, string json, string exception);
  }
}
