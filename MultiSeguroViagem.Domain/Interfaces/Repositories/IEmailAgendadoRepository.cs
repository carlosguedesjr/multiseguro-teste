using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;


namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IEmailAgendadoRepository
  {
    string BuscaHtmlVoucher( Pagamento pagamento, IList<Viajante> viajantes, out string html, out string assunto);
    void InsereEmail(EmailAgendado email);
  }
}
