using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
    public interface ICoberturaRepository 
    {
        ICollection<Cobertura> Busca(int idPlano);
    }
}