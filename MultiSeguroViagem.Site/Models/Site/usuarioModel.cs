using System;
using System.ComponentModel.DataAnnotations;
using MultiSeguroViagem.Site.Attributes;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Informe o e-mail")]
        public string Email { get; set; }

        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe o telefone")]
        [RegularExpression(@"^\([0-9]{2,}\) [0-9]{4,5}-[0-9]{4,}$", ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }

        [ValidaDocumento(ErrorMessage = "Documento inválido")]
        [Required(ErrorMessage = "Informe CPF/CNPJ")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "Informe o CEP")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Informe o endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Informe o número")]
        public string Numero { get; set; }
        public string Complemento { get; set; }

        [Required(ErrorMessage = "Informe o bairro")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Informe a cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Informe o estado")]
        public string Estado { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Status { get; set; }

        public string NovaSenha { get; set; }
    }
}