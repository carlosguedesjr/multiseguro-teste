using System;
using System.Collections.Generic;

namespace MultiIntegra.Common.Models.April
{
  public class AprilEmissaoModel
  {
    public string IdPlano { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Destino { get; set; }
    public bool Familiar { get; set; }
    public IEnumerable<AprilViajanteModel> Viajantes { get; set; }
  }
}
