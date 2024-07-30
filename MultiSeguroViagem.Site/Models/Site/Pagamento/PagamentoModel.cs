using System.ComponentModel.DataAnnotations;
using MultiSeguroViagem.Site.Attributes;

namespace MultiSeguroViagem.Site.Models.Site.Pagamento
{
  public class PagamentoModel
  {
    [Required(ErrorMessage = "Informe o nome")]
    public string NomeCompleto { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Informe o e-mail")]
    public string Email { get; set; }

    [ValidaDocumento(ErrorMessage = "Documento inválido")]
    [Required(ErrorMessage = "Informe CPF/CNPJ")]
    public string Documento { get; set; }

    [Required(ErrorMessage = "Informe o telefone")]
    [RegularExpression(@"^\([0-9]{2,}\) [0-9]{4,5}-[0-9]{4,}$", ErrorMessage = "Telefone inválido")]
    public string TelefonePessoal { get; set; }

    [Required(ErrorMessage = "Informe o CEP")]
    public string Cep { get; set; }

    [Required(ErrorMessage = "Informe o endereço")]
    public string Endereco { get; set; }

    [Required(ErrorMessage = "Informe o bairro")]
    public string Bairro { get; set; }

    public string Complemento { get; set; }

    [Required(ErrorMessage = "Informe o estado")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "Informe a cidade")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "Informe o número")]
    public string Numero { get; set; }

    [Required(ErrorMessage = "Informe o nome de contato")]
    public string NomeContato { get; set; }

    [Required(ErrorMessage = "Informe o número de contato")]
    [RegularExpression(@"^\([0-9]{2,}\) [0-9]{4,5}-[0-9]{4,}$", ErrorMessage = "Telefone inválido")]
    public string TelefoneContato { get; set; }

    public string Carrinho { get; set; }

    [Required(ErrorMessage = "Informe o método de pagamento")]
    public string OperadoraPagamento { get; set; }

    public string Bandeira { get; set; }

    public string Parcelas { get; set; }

    public string NumeroCartao { get; set; }

    public string NomeCartao { get; set; }

    public string MesAno { get; set; }

    public string CodigoSeguranca { get; set; }

    public string Cupom { get; set; }

    public string ValorTotal { get; set; }

    public string DescontoCupom { get; set; }

    public string DescontoBoleto { get; set; }

    public string DescontoTotal { get; set; }

    public string DataNovaVolta { get; set; }


    [Range(typeof(bool), "true", "true", ErrorMessage = "É necessário concordar com o termo")]
    public bool TermosUso { get; set; }
    
    public bool Estudante { get; set; }

    public string EmailCotacao { get; set; }

    public string OrigemLead { get; set; }
  }
}
