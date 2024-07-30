namespace MultiSeguroViagem.Domain.Entities
{
  public class BlackListFraude
  {
    public int idBlacklistFraude { get; set; }
    public string strIp { get; set; }
    public string datExpiracao { get; set; }
    public string bitExcluido { get; set; }
  }
}
