
using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site.Login
{
    public class CadastroModel
    {
        [Required(ErrorMessage = "Informe seu nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe seu email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe sua senha")]
        public string Senha { get; set; }

        public string EmailRecuperarSenha { get; set; }
    }
}