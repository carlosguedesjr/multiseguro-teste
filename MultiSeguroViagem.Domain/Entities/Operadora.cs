namespace MultiSeguroViagem.Domain.Entities
{
  public class Operadora
  {
    public int IdOperadora { get; set; }
    public string Nome { get; set; }
    public bool Status { get; set; }
    public string ImagemLogo { get; set; }
    public decimal ValorDolar { get; set; }
    public string Site { get; set; }
    public string SiteBlog { get; set; }
    public string SiteReclameAqui { get; set; }
  }
}
