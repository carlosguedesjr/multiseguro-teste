using MultiSeguroViagem.Domain.Entities;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface IEmailAgendadoService
  {
    void InsereEmail(string remetente, string nomeRemetente, string destinatario, string assunto, string mensagemHtml, string mensagemText);
    void InsereEmailComparador(string remetente, string nomeRemetente, string destinatario, string assunto, string mensagemHtml, string mensagemText, string nomeDestinatario);
    void EmailVoucher(string remetente, string nomeRemetente, string destinatario, Pagamento pagamento, IList<Viajante> viajantes);
    void EmailEstornoRedeCard(string remetente, string nomeRemetente, string destinatario, string numeroPedido, string nomeViajante, string nomeOperadora, string valor, bool sucesso);
  }
}
