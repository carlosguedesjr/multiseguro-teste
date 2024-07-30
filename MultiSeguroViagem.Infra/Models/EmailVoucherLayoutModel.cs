
namespace MultiSeguroViagem.Infra.Models
{
  public class EmailVoucherLayoutModel
  {
    public int IdOperadora { get; set; }
    public string Assunto { get; set; }
    public string MensagemHtml { get; set; }
    public string MensagemText { get; set; }
    public string ViajanteHtml { get; set; }
    public string ViajanteText { get; set; }
    public System.DateTime DataCriacao { get; set; }
  }
}
