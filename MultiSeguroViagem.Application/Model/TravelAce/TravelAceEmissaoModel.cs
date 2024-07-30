using System;
using System.Collections.Generic;

namespace MultiIntegra.Common.Models.TravelAce
{
  public class TravelAceEmissaoModel
  {
    public string IdPlano { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Destino { get; set; }
    public string Valor { get; set; }
    public string CodigoTarifa { get; set; }
    public string TipoTarifa { get; set; }
    public IEnumerable<TravelAceViajantesModel> Viajantes { get; set; }
  }
}
