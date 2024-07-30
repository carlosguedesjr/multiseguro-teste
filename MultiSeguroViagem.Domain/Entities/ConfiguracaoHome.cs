using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Entities
{
  public class ConfiguracaoHome
  {
    protected ConfiguracaoHome() { }
    
    public int IdConfiguracao { get; private set; }
    public string H1 { get; private set; }
    public string H2 { get; private set; }
    public string CaminhoImagemBackground { get; private set; }
    public bool ExibirBanners { get; private set; }
    public IEnumerable<Banner> Banners { get; private set; }

    public void SetaBanners(IEnumerable<Banner> banners)
    {
      Banners = banners;
    }
  }
}
