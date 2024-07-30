using System;
using System.Collections.Generic;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;

namespace MultiSeguroViagem.Application.Services
{
  public class DestinoService : IDestinoService
  {
    private readonly IDestinoRepository _repo;

    public DestinoService(IDestinoRepository repo)
    {
      _repo = repo;
    }

    public ICollection<Destino> ObtemDestinos()
    {
      return _repo.Busca();
    }

    public ICollection<DestinoSeo> BuscaDestinosSeo()
    {
      return _repo.BuscaDestinosSeo();
    }

    public DestinoSeo BuscaDestinoSeo(string destino)
    {
      if (string.IsNullOrEmpty(destino))
        throw new Exception("Para buscar os dados do destino o mesmo não pode ser nulo");

      return _repo.BuscaDestinoSeo(destino.RemoveAcentuacao().RemoveEspacos().ToLower());
    }
  }
}
