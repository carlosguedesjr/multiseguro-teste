using System.Net;
using System.Net.Mail;
using System.Text;

namespace MultiSeguroViagem.Common.Helpers
{
    public class Email
    {
        /// <summary>
        /// Envio de e-mail transacional
        /// </summary>
        /// <param name="emitente">Emitente</param>
        /// <param name="destinatario">Destinatario</param>
        /// <param name="assunto">Assunto</param>
        /// <param name="corpo">Corpo</param>
        public static void EnviaEmail(string emitente, string destinatario, string assunto, string corpo)
        {
            var email = new MailMessage()
            {
                To = { destinatario },
                From = new MailAddress(emitente),
                Priority = MailPriority.Normal,
                IsBodyHtml = true,
                Subject = assunto,
                SubjectEncoding = Encoding.GetEncoding("ISO-8859-1"),
                Body = corpo,
                BodyEncoding = Encoding.GetEncoding("ISO-8859-1"),
                
            };

            var clientSmtp = new SmtpClient("transational5.cenariocapital.com.br")
            {
                EnableSsl = false,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("contato@multiseguroviagem.com.br", "6*QPX23{*^v(")
            };

            clientSmtp.Send(email);
        }
    }
}
