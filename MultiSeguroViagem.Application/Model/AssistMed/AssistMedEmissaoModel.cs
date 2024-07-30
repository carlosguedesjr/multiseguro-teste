using System;
using System.Collections.Generic;

namespace MultiIntegra.Common.Models.AssistMed
{
  public class AssistMedEmissaoModel
  {
    public string IdPlano { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Destino { get; set; }
    public bool Familiar { get; set; }
    public IEnumerable<AssistMedViajantesModel> Viajantes { get; set; }
  }
}
