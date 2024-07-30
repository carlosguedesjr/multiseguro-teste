using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MultiSeguroViagem.Site.Models.Site.Cotacao
{
  public class EnviaComparacaoModel
  {
    public string IdDestino { get; set; }

    public string Destino { get; set; }

    public string DataIda { get; set; }

    public string DataVolta { get; set; }

    public string PlanosId { get; set; }

    public string Email { get; set; }

    public string Nome { get; set; }

    public string EmailVendedor { get; set; }

    [DisplayName("CupomAfiliado")]
    public string Cpaff { get; set; }

    public IEnumerable<EnviaComparacaoPlanoModel> Planos { get; set; }
  }
}
