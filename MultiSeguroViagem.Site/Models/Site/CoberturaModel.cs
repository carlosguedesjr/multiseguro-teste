using System.Web.Mvc;
namespace MultiSeguroViagem.Site.Models.Site
{
  public class CoberturaModel
  {
    public bool Ativo { get; set; }
    public int IdCobertura { get; set; }
    public string Nome { get; set; }
    public bool Incluso { get; set; }
    public decimal ValorCobertura { get; set; }
    public string MoedaCobertura { get; set; }
    public decimal ValorAcrescimo { get; set; }
    public string MoedaAcrescimo { get; set; }
    [AllowHtml]
  public string MensagemPlano { get; set; }
  }
}
