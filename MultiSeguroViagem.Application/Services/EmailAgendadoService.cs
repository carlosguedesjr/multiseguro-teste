using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MultiSeguroViagem.Application.Services
{
  public class EmailAgendadoService : IEmailAgendadoService
  {
    private readonly IEmailAgendadoRepository _emailAgendadoRepository;

    public EmailAgendadoService(IEmailAgendadoRepository repo)
    {
      _emailAgendadoRepository = repo;
    }

    public void InsereEmail(string remetente, string nomeRemetente, string destinatario, string assunto, string mensagemHtml, string mensagemText)
    {
      var email = new EmailAgendado(remetente, nomeRemetente, destinatario, assunto, mensagemHtml, mensagemText);
      email.Valida();
      _emailAgendadoRepository.InsereEmail(email);
    }

    public void InsereEmailComparador(string remetente, string nomeRemetente, string destinatario, string assunto, string mensagemHtml, string mensagemText, string nomeDestinatario)
    {
      var email = new EmailAgendado(remetente, nomeRemetente, destinatario, assunto, mensagemHtml, mensagemText, nomeDestinatario);
      email.Valida();
      _emailAgendadoRepository.InsereEmail(email);
    }

    public void EmailVoucher(string remetente, string nomeRemetente, string destinatario, Pagamento pagamento, IList<Viajante> viajantes)
    {
      string html, assunto;
      _emailAgendadoRepository.BuscaHtmlVoucher(pagamento, viajantes, out html, out assunto);

      var texto = Regex.Replace(Regex.Replace(html, "<style type=\"text/css\">.*</style>|<.*?>", string.Empty).Trim().Replace("  ", ""), @"[\r\n]{2,}", "\n\n");
      texto = Regex.Replace(texto, @"[\t]{2,}", "");

      InsereEmail(remetente, nomeRemetente, destinatario, assunto, html, texto);
    }

    public void EmailEstornoRedeCard(string remetente, string nomeRemetente,string destinatario, string numeroPedido, string nomeViajante, string nomeOperadora, string valor,  bool sucesso)
    {
      string html, assunto;

      if (sucesso)
      {
        html = ConteudoEmailSucessoEstornoRedeCard(numeroPedido, nomeViajante, nomeOperadora, valor);
        assunto = $"Problema no processamento do Pedido {numeroPedido}";
      }
      else
      {
        html = ConteudoEmailErroEstornoRedeCard(numeroPedido, nomeOperadora);
        assunto = $"Atenção, não foi possivel estornar o pagamento do Pedido {numeroPedido}";
      }
      var texto = Regex.Replace(Regex.Replace(html, "<.*?>", string.Empty).Trim().Replace("  ", ""), @"[\r\n]{2,}", "\n\n");
      texto = Regex.Replace(texto, @"[\t]{2,}", "");

      InsereEmail(remetente, nomeRemetente, destinatario, assunto, html, texto);
    }   

    private string ConteudoEmailSucessoEstornoRedeCard(string numeroPedido, string nomeViajante, string nomeOperadora, string valor) {

      var html = $@"<html>
                    <head>
                        <meta http-equiv='content-type' content='text/html' ; charset='utf-8' /> </head>
                    <body> <span style='display:none!important;color:#f7f7f7;font-size:1px'>Seja qual for o seu destino, conta com a gente.</span>
                        <table cellpadding='0' cellspacing='0' border='0' width='100%' style='table-layout:fixed;background-color:#ffffff'>
                            <tbody>
                                <tr>
                                    <td align='center'>
                                        <table cellpadding='0' cellspacing='0' border='0' align='center' style='width:100%;max-width:650px' width='650'>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <table align='center' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF' style='width:100%'>
                                                            <tbody>
                                                                <tr>
                                                                    <td align='center' style='padding-top:14px;padding-bottom:20px'> <img src='https://www.multiseguroviagem.com.br/www/img/email/logo.png' alt='' border='0' style='display:block' align='center' width='162' height='65'> </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center'>
                                        <table cellpadding='0' cellspacing='0' border='0' align='center' style='width:100%;max-width:650px' width='650' bgcolor='#FFFFFF'>
                                            <tbody>
                                                <tr>
                                                    <td align='center' style='padding-left:45px;padding-right:45px;padding-top:20px;padding-bottom:15px'>
                                                        <p style='font-family:' Open Sans ',sans-serif;font-size:16px;color:#000;line-height:1.4em;margin-bottom:0.3em'> Olá,{nomeViajante}</p>
                                                        <p style='font-family:' Open Sans ',sans-serif;font-size:14px;color:#000;line-height:1.4em'> Devido a um problema no processamento do pedido {numeroPedido} junto a operadora {nomeOperadora}, o seu voucher não foi emitido. Pedimos desculpas pelo ocorrido e informamos que o valor R$ {valor} foi estornado do seu cartão automaticamente. </p>
                                                        <p style='font-family:' Open Sans ',sans-serif;font-size:14px;color:#000;line-height:1.4em'> Em breve um de nossos vendedores entrará em contato para esclarecer suas dúvidas. Caso prefira entre em contato através dos números  (19) 3201-0447 ou nos envie um email para atendimento@multiseguroviagem.com.br. </p>
                                                        <p style='font-family:' Open Sans ',sans-serif;font-size:14px;color:#000;line-height:1.4em'> Agradecemos sua compreensão.</p>                                  
                                                    </td>
                                                </tr>                           
                                                <tr>
                                                    <td align='center' style='padding-left:45px;padding-right:45px;padding-bottom:5px'>
                                                      <p style='font-family:' Open Sans ',sans-serif;font-size:12px;color:#000;line-height:1.2em;margin-top:1.2em;margin-bottom:1.2em'><a href='https://www.multiseguroviagem.com.br/' target='_blank'>www.multiseguroviagem.com.br</a></p>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </body>
                    </html>";

      return html;
    }

    private string ConteudoEmailErroEstornoRedeCard(string numeroPedido, string nomeOperadora)
    {

      var html = $@"<html>
                    <head>
                        <meta http-equiv='content-type' content='text/html' ; charset='utf-8' /> </head>
                    <body> <span style='display:none!important;color:#f7f7f7;font-size:1px'>Seja qual for o seu destino, conta com a gente.</span>
                        <table cellpadding='0' cellspacing='0' border='0' width='100%' style='table-layout:fixed;background-color:#ffffff'>
                            <tbody>
                                <tr>
                                    <td align='center'>
                                        <table cellpadding='0' cellspacing='0' border='0' align='center' style='width:100%;max-width:650px' width='650'>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <table align='center' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF' style='width:100%'>
                                                            <tbody>
                                                                <tr>
                                                                    <td align='center' style='padding-top:14px;padding-bottom:20px'> <img src='https://www.multiseguroviagem.com.br/www/img/email/logo.png' alt='' border='0' style='display:block' align='center' width='162' height='65'> </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center'>
                                        <table cellpadding='0' cellspacing='0' border='0' align='center' style='width:100%;max-width:650px' width='650' bgcolor='#FFFFFF'>
                                            <tbody>
                                                <tr>
                                                    <td align='center' style='padding-left:45px;padding-right:45px;padding-top:20px;padding-bottom:15px'>
                                                        <p style='font-family:' Open Sans ',sans-serif;font-size:16px;color:#000;line-height:1.4em;margin-bottom:0.3em'> Atenção!</p>
                                                        <p style='font-family:' Open Sans ',sans-serif;font-size:14px;color:#000;line-height:1.4em'> Não foi possivel estornar o pagamento na RedeCard para o pedido {numeroPedido} da operadora {nomeOperadora}, favor realizar o estorno manualmente. </p>                             
                                                    </td>
                                                </tr>                           
                                                <tr>
                                                    <td align='center' style='padding-left:45px;padding-right:45px;padding-bottom:5px'>
                                                      <p style='font-family:' Open Sans ',sans-serif;font-size:12px;color:#000;line-height:1.2em;margin-top:1.2em;margin-bottom:1.2em'><a href='https://www.multiseguroviagem.com.br/' target='_blank'>www.multiseguroviagem.com.br</a></p>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </body>
                    </html>";

      return html;
    }
  }
}
