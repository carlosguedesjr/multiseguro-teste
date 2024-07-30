namespace MultiSeguroViagem.Site.Models.Site
{
  public class SeguroViagemPostModel
  {
    public SeguroViagemPostModel(string titulo, string data, string link, string categoria)
    {
      Titulo = titulo;
      Data = data;
      Link = link;
      Categoria = categoria;
    }

    public string Titulo { get; set; }
    public string Data { get; set; }
    public string Link { get; set; }
    public string Categoria { get; set; }
  }
}