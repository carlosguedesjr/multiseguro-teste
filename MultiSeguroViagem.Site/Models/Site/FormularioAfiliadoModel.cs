using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class FormularioAfiliadoModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Informe o e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o telefone")]
        [RegularExpression(@"^\([0-9]{2,}\) [0-9]{4,5}-[0-9]{4,}$", ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Informe a mensagem")]
        public string Mensagem { get; set; }

       

    }
}