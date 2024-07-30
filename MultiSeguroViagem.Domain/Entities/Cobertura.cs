using System;
namespace MultiSeguroViagem.Domain.Entities
{
  public class Cobertura
  {
    public bool Ativo { get; set; }
    public int IdCobertura { get; set; }
    public int IdPlano { get; set; }
    public string Nome { get; set; }
    public bool Incluso { get; set; }
    public decimal ValorCobertura { get; set; }
    public string MoedaCobertura { get; set; }
    public decimal ValorAcrescimo { get; set; }
    public string MoedaAcrescimo { get; set; }
    public string MensagemPlano { get; set; }
  }
}
