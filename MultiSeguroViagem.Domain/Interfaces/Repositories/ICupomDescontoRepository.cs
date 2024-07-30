using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface ICupomDescontoRepository
  {
    CupomDesconto Busca(string codigo);
  }
}
