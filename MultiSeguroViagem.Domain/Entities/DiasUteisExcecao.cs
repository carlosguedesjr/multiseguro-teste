
namespace MultiSeguroViagem.Domain.Entities
{
  public class DiasUteisExcecao
  {     
    public DiasUteisExcecao() {    }

    public string Data { get; private set; }
    public string HoraInicio { get; private set; }
    public string HoraFinal { get; private set; }
    public bool Status { get; private set; }
  }
}
