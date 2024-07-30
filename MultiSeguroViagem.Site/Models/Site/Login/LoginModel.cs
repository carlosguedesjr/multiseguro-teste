using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Informe seu email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe sua senha")]
        public string Senha { get; set; }

        public string EmailRecuperarSenha { get; set; }
    }

    public class LogadoModel
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Documento { get; set; }

    }
}