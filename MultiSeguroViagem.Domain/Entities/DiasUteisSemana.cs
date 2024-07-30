
namespace MultiSeguroViagem.Domain.Entities
{
  public class DiasUteisSemana
  {
    protected DiasUteisSemana() { }

    public string DiaSemana { get; private set; }
    public string HoraInicio { get; private set; }
    public string HoraFinal { get; private set; }
    public bool Status { get; private set; }

  }
}
