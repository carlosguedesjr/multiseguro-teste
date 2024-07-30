using MultiSeguroViagem.Domain.Entities;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public  interface IDiasUteisRepository
  {
    ICollection<DiasUteisSemana> BuscaDiasSemana();
    ICollection<DiasUteisExcecao> BuscaDiasExcecoes();
  }
}
