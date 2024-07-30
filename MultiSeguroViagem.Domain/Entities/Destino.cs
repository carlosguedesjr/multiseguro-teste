namespace MultiSeguroViagem.Domain.Entities
{
  public class Destino
  {
    public int IdDestino { get; set; }
    public string Nome { get; set; }
  }

  public class DestinoSeo
  {
    public int IdDestino { get; set; }
    public string Slug { get; set; }
    public string Nome { get; set; }
    public string NomeDestinoCotador { get; set; }
    public string BlocoUmTitulo { get; set; }
    public string BlocoUmDescricao { get; set; }
    public string BlocoDoisTitulo { get; set; }
    public string BlocoDoisDescricao { get; set; }
  }
}
