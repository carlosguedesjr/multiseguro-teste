using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
    public interface ICoberturaService 
    {
        ICollection<Cobertura> BuscaCoberturas(int idPlano);
    }
}