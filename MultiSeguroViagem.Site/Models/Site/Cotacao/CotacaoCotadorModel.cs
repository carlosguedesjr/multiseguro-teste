using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site.Cotacao
{
  public class CotacaoCotadorModel
  {
    [Required(ErrorMessage = "Informe seu destino")]
    public string Destino { get; set; }

    [Required(ErrorMessage = "Informe a data de ida")]
    public string DataIda { get; set; }

    [Required(ErrorMessage = "Informe a data de volta")]
    public string DataVolta { get; set; }



    public string Origem { get; set; }

    public string Operadora { get; set; }

    [DisplayName("CupomAfiliado")]
    public string Cpaff { get; set; }

    public IEnumerable<PlanoModel> Planos { get; set; }

    public IEnumerable<EnviaComparacaoPlanoModel> PlanosComparador { get; set; }

 

  }
}
