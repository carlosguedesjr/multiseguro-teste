using System.ComponentModel.DataAnnotations;

namespace MultiSeguroViagem.Site.Models.Site.Pagamento
{
  public class PagamentoAvulsoModel
  {
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Documento { get; set; }
    public string Cep { get; set; }
    public string DataIda { get; set; }
    public string DataVolta { get; set; }
    public string ValorTotal { get; set; }
    public string ValorDescontoBoleto { get; set; }
    public string ValorDescontoCupom { get; set; }
    public string NomeDestino { get; set; }
    public string NomePlano { get; set; }
    [Range(typeof(bool), "true", "true", ErrorMessage = "É necessário concordar com o termo")]
    public bool TermosUso { get; set; }

    public int IdPagamento { get; set; }
    public int IdPedido { get; set; }
    public int BitTentativa { get; set; }
    public string IdOperadorasPagamento { get; set; }
    public string Erro { get; set; }
    public string OperadoraPagamento { get; set; }
    public string Endereco { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }

    public string Bandeira { get; set; }
    public string Parcelas { get; set; }
    public string NumeroCartao { get; set; }
    public string NomeCartao { get; set; }
    public string MesAno { get; set; }
    public string CodigoSeguranca { get; set; }

    public string p { get; set; } // IdPedido
    public string i { get; set; } // IdPagamento
    public string o { get; set; } // IdOperadorasPagamento

    //public System.DateTime DataVencimento { get; set; }
    //public string v { get; set; } // Valor
    //public string e { get; set; } // Email
    //public string n { get; set; } // Nome
    //public string d { get; set; } // Documento
    //public string c { get; set; } // Cep
    //public string dv { get; set; } // DataVencimentoPagamento
    //public string end { get; set; } // endereço
    //public string barr { get; set; } // Bairro
    //public string cid { get; set; } // Cidade
    //public string est { get; set; } // Estado

  }
}
