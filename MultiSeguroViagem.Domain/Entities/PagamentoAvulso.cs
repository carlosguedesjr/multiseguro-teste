using System;

namespace MultiSeguroViagem.Domain.Entities
{
  public class PagamentoAvulso
  {
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Documento { get; set; }
    public string Cep { get; set; }
    public string DataIda { get; set; }
    public string DataVolta { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorDescontoCupom { get; set; }
    public decimal ValorDescontoBoleto { get; set; }
    public DateTime DataVencimento { get; set; }
    public string IdPagamentoOperadora { get; set; }
    public string OperadorasAceitas { get; set; }
    public string Status { get; set; }

    public Plano Plano { get; set; }
    public Destino Destino { get; set; }
  }
}
