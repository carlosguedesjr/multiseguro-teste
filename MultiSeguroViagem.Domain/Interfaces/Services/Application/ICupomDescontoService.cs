using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
    public interface ICupomDescontoService
    {
        CupomDesconto BuscaCupom(string cpf, string codigo, decimal valorCompra, string operadora);
    }
}
