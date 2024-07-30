namespace MultiSeguroViagem.Domain.Entities
{
  public class Banner
  {
    #region Construtor

    protected Banner() { }

    public Banner(int idBanner, string tag, string tituloModal, string tituloModalBloco1, string descricaoModalBloco1, string tituloModalBloco2, string descricaoModalBloco2, string caminhoImagem)
    {
      IdBanner = idBanner;
      Tag = tag;
      TituloModal = tituloModal;
      TituloModalBloco1 = tituloModalBloco1;
      DescricaoModalBloco1 = descricaoModalBloco1;
      TituloModalBloco2 = tituloModalBloco2;
      DescricaoModalBloco2 = descricaoModalBloco2;
      CaminhoImagem = caminhoImagem;
    }

    #endregion

    #region Propriedades    

    public int IdBanner { get; private set; }
    public string Identificador { get; private set; }
    public string Tag { get; private set; }
    public string TituloModal { get; private set; }
    public string TituloModalBloco1 { get; private set; }
    public string DescricaoModalBloco1 { get; private set; }
    public string TituloModalBloco2 { get; private set; }
    public string DescricaoModalBloco2 { get; private set; }
    public string FiltroOperadora { get; private set; }
    public string FiltroDestino { get; private set; }
    public string CaminhoImagem { get; private set; }

    #endregion

    #region Métodos


    #endregion
  }
}
