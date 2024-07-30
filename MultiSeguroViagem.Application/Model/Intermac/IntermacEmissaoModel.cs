using System;
using System.Collections.Generic;

namespace MultiIntegra.Common.Models.Intermac
{
  public class IntermacEmissaoModel
  {
    public string IdPlano { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Destino { get; set; }
    public bool Familiar { get; set; }
    public IEnumerable<IntermacViajantesModel> Viajantes { get; set; }
  }
}
