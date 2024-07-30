using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;

namespace MultiSeguroViagem.Application.Services
{
  public class DiasUteisService : IDiasUteisService
  {
    readonly IDiasUteisRepository _repo;
    public DiasUteisService(IDiasUteisRepository repo)
    {
      _repo = repo;
    }

    public ICollection<DiasUteisExcecao> BuscaDiasExcecoes()
    {
      return _repo.BuscaDiasExcecoes();
    }

    public ICollection<DiasUteisSemana> BuscaDiasSemana()
    {
      return _repo.BuscaDiasSemana();
    }
  }
}
