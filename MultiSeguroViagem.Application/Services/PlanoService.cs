using System;
using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using System.Linq;

namespace MultiSeguroViagem.Application.Services
{
  public class PlanoService : IPlanoService
  {
    private readonly IPlanoRepository _repo;

    public PlanoService(IPlanoRepository repo)
    {
      _repo = repo;
    }
    
    public ICollection<Plano> BuscaPlanos(int idDestino, DateTime dataIda, DateTime dataVolta)
    {
      var dias = (int)(dataVolta - dataIda).TotalDays;
      return _repo.BuscaPlanos(idDestino, dias);
    }

    public IList<ArquivoUpload> ObtemArquivosUploadPlanos(int idPlano)
    {
      return _repo.ObtemArquivosUploadPlanos(idPlano);
    }
    
    public string DefineCodigoTarifaAssisCard(int idPlano, DateTime dataIda, DateTime dataVolta)
    {
      var tarifas = _repo.ObtemTarifasAssisCard(idPlano);

      var dias = (int)(dataVolta - dataIda).TotalDays + 1;

      var tarifa = tarifas.FirstOrDefault(x => dias >= x.DiaDe && dias <= x.DiaAte || dias == x.DiaDe);

      if (tarifa == null)
      {
        var tarifaAux = tarifas.Aggregate((curMin, x) => curMin == null || x.DiaDe < curMin.DiaDe ? x : curMin);

        if (dias < tarifaAux.DiaDe)
          return tarifaAux.Codigo;
      }

      return tarifa.Codigo ?? string.Empty;
    }
  }
}
