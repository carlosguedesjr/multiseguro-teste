

using MultiSeguroViagem.Common.Validations;
using System;

namespace MultiSeguroViagem.Domain.Entities
{
    public class EmailAgendado
    {
        #region Construtor

        protected EmailAgendado() { }

        public EmailAgendado(string remetente, string nomeRemetente, string destinatario, string assunto, string mensagemHtml, string mensagemText, string nomeDestinatario = "")
        {
            Remetente = remetente;
            NomeRemetente = nomeRemetente;
            Destinatario = destinatario;
            Assunto = assunto;
            MensagemHtml = mensagemHtml;
            MensagemText = mensagemText;
            DataCriacao = DateTime.Now;
            NomeDestinatario = nomeDestinatario;
    }

        #endregion

        #region Propriedades   

        public int IdEmailAgendado { get; private set; }
        public string Remetente { get; private set; }
        public string NomeRemetente { get; private set; }
        public string Destinatario { get; private set; }
        public string Assunto { get; private set; }
        public string MensagemHtml { get; private set; }
        public string MensagemText { get; private set; }
        public string NomeDestinatario { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataEnvio { get; private set; }
        public bool Enviado { get; private set; }

        #endregion

        #region Métodos

        public void Valida()
        {
            AssertionConcern.AssertArgumentNotNull(Remetente, "O remetente do e-mail não pode ser nulo");
            AssertionConcern.AssertArgumentNotNull(Destinatario, "O destinatario do e-mail não pode ser nulo");
            AssertionConcern.AssertArgumentNotNull(Assunto, "O assunto do e-mail não pode ser nulo");
        }

        #endregion
    }
}
