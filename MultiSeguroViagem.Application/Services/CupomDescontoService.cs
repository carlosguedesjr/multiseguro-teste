using System;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;

namespace MultiSeguroViagem.Application.Services
{
    public class CupomDescontoService : ICupomDescontoService
    {
        private readonly ICupomDescontoRepository _repo;
        private readonly IPagamentoRepository _repoPagamento;

        public CupomDescontoService(ICupomDescontoRepository repo, IPagamentoRepository repoPagamento)
        {
            _repo = repo;
            _repoPagamento = repoPagamento;
        }

        public CupomDesconto BuscaCupom(string documento, string codigo, decimal valorCompra, string operadora)
        {
            var cuponsUtilizados = _repoPagamento.CuponsUtilizados(documento);
            var cupom = _repo.Busca(codigo);

            if (cupom == null)
                throw new InvalidOperationException("Cupom não encontrado");
            
            cupom.Valida(cupom, cuponsUtilizados, valorCompra, operadora);

            return cupom;
        }
    }
}
