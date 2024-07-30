using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class NoScriptModel
    {
        [Required(ErrorMessage = "Informe seu nome")]
        public string nome { get; set; }
    }
}