using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site.Cotacao
{
  public class CotacaoHomeModel
  {
    [Required(ErrorMessage = "Informe seu destino")]
    public string Destino { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Informe a data de ida")]
    public string DataIda { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Informe a data de volta")]
    public string DataVolta { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Informe seu e-mail")]
    public string Email { get; set; }

    public string Origem { get; set; }

    public string Operadora { get; set; }

    [DisplayName("CupomAfiliado")]
    public string Cpaff { get; set; }

    [Required(ErrorMessage = "Informe seu Telefone")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "Informe seu Nome")]
    public string Nome { get; set; }


    public IEnumerable<PlanoModel> Planos { get; set; }
  }
}
