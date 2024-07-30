using System;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;

namespace MultiSeguroViagem.Application.Services
{
  public class CotacaoService : ICotacaoService
  {
    private readonly IPlanoRepository _repo;
    private readonly IComparacaoRepository _comparacaoRepo;
    private readonly ICupomDescontoRepository _cupomDescontoRepository;

    public CotacaoService(IPlanoRepository repo, IComparacaoRepository comparacaoRepo, ICupomDescontoRepository cupomDescontoRepository)
    {
      _repo = repo;
      _comparacaoRepo = comparacaoRepo;
      _cupomDescontoRepository = cupomDescontoRepository;
    }

    public Cotacao RealizaCotacao(int idDestino, string cupomAfiliado, DateTime dataIda, DateTime dataVolta)
    {
      var descontoAfiliado = BuscaCupomAfiliado(cupomAfiliado);

      var dias = (int)((dataVolta - dataIda).TotalDays + 1);
      var planos = _repo.BuscaPlanos(idDestino, dias);

      var cotacao = new Cotacao(idDestino, dataIda, dataVolta, planos);
      cotacao.Valida();
      cotacao.CalculaPrecoComercial(descontoAfiliado);
      
      return cotacao;
    }
    
    public void CadastraLeadComparacao(string destino, string dataIda, string dataVolta, string email, string planosId)
    {
      _comparacaoRepo.CadastraLead(destino, Convert.ToDateTime(dataIda), Convert.ToDateTime(dataVolta), email, planosId);
    }


    private decimal? BuscaCupomAfiliado(string cupomAfiliado)
    {
      if (string.IsNullOrEmpty(cupomAfiliado))
        return null;

      var cupom = _cupomDescontoRepository.Busca(cupomAfiliado);

      if (cupom == null)
        return 0;

      return cupom.ValidaAfiliado() ? cupom.ValorDesconto : 0;
    }
  }
}
