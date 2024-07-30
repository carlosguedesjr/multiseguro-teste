using System;
using System.Collections.Generic;

namespace MultiIntegra.Common.Models.VitalCard
{
  public class VitalCardEmissaoModel
  {
    public string IdPlano { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Destino { get; set; }
    public IEnumerable<VitalCardViajanteModel> Viajantes { get; set; }
  }
}
