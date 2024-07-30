
using MultiSeguroViagem.Domain.Entities;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface IDiasUteisService
  {
    ICollection<DiasUteisSemana> BuscaDiasSemana();
    ICollection<DiasUteisExcecao> BuscaDiasExcecoes();
  }
}
