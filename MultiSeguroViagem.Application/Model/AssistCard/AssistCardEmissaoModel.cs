using System;
using System.Collections.Generic;

namespace MultiIntegra.Common.Models.AssistCard
{
  public class AssistCardEmissaoModel
  {
    public string IdPlano { get; set; }
    public string CodigoTarifa { get; set; }
    public string Destino { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Moeda { get; set; }
    public string Valor { get; set; }
    public string Markup { get; set; }
    public string Desconto { get; set; }
    public bool Familiar { get; set; }
    public IEnumerable<AssistCardViajanteModel> Viajantes { get; set; }
  }
}
