using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;

namespace MultiSeguroViagem.Application.Services
{
    public class CoberturaService : ICoberturaService
    {

        private readonly ICoberturaRepository _repo;

        public CoberturaService(ICoberturaRepository repo)
        {
            _repo = repo;
        }

        public ICollection<Cobertura> BuscaCoberturas(int idPlano)
        {
            return _repo.Busca(idPlano);
        }
    }
}