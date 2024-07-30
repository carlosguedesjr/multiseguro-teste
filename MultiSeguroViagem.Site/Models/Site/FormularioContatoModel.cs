using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class FormumarioContatoModel
    {
        [Required(ErrorMessage = "Informe seu nome")]
        public string Nome { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Informe seu email")]  
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe seu telefone")]
        [RegularExpression(@"^\([0-9]{2,}\) [0-9]{4,5}-[0-9]{4,}$", ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Informe sua mensagem")]
        public string Mensagem { get; set; }
    }
}