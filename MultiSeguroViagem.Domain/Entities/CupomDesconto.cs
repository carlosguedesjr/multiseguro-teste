using MultiSeguroViagem.Common.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiSeguroViagem.Domain.Entities
{
  public class CupomDesconto
  {
    #region Construtor

    protected CupomDesconto() { }

    public CupomDesconto(string codigo)
    {
      Ativo = true;
      Codigo = codigo;
    }

    #endregion

    #region Propriedades    

    public int IdCupom { get; private set; }
    public bool Ativo { get; private set; }
    public string Codigo { get; private set; }
    public string TipoDesconto { get; private set; }
    public decimal ValorDesconto { get; private set; }
    public decimal ValorMinimoCompra { get; private set; }
    public int QuantidadeUso { get; private set; }
    public int QuantidadeUtilizada { get; private set; }
    public bool ZerarComissao { get; private set; }
    public bool Afiliado { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public IList<CupomOperadoras> Operadoras { get; private set; }

    #endregion

    #region Métodos

    public void DefineOperadora(IList<CupomOperadoras> operadora)
    {
      Operadoras = operadora;
    }

    public void Valida(CupomDesconto cupom, IEnumerable<string> cuponsUtilizados, decimal valorCompra, string operadora)
    {
      AssertionConcern.AssertArgumentNotNull(cupom.Ativo, "O status do cupom não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(cupom.Codigo, "O código do cupom não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(cupom.ValorMinimoCompra, "O valor mínimo da compra do cupom não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(cupom.QuantidadeUso, "A quantidade de uso do cupom não pode ser nula");
      AssertionConcern.AssertArgumentNotNull(cupom.DataVencimento, "A data de vencimento do cupom não pode ser nula");
      AssertionConcern.AssertArgumentNotNull(cuponsUtilizados, "A lista de cupons já utilizados não pode ser nula");

      if (Afiliado)
        throw new InvalidOperationException("Cupom inválído");

      if (cupom.Operadoras.Count > 0)
      {
        var operadoraValida = false;
        foreach (var cupomOperadora in cupom.Operadoras)
        {
          if (string.Equals(cupomOperadora.Nome, operadora))
          {
            operadoraValida = true;
          }
        }

        if (!operadoraValida)
          throw new InvalidOperationException("O cupom não é valído para a operadora escolhida");
      }

      var totalDesconto = cupom.ValorDesconto;

      if (string.Equals(cupom.TipoDesconto, "Porcentagem"))
        totalDesconto = valorCompra * (cupom.ValorDesconto / 100);

      if (valorCompra < totalDesconto)
        throw new InvalidOperationException("O desconto é maior que o valor da compra");

      if (cuponsUtilizados.Contains(cupom.Codigo))
        throw new InvalidOperationException("Cupom já utilizado anteriormente pelo CPF/CNPJ");

      if (cupom.Ativo && (cupom.QuantidadeUso > cupom.QuantidadeUtilizada || cupom.QuantidadeUso == 0) && (cupom.DataVencimento.Date >= DateTime.Now.Date || cupom.DataVencimento.Date == DateTime.MinValue.Date))
      {
        if (cupom.ValorMinimoCompra >= valorCompra)
          throw new InvalidOperationException("O valor da compra não atingiu o valor mínimo do cupom");

        return;
      }

      throw new InvalidOperationException("Código inválido ou expirado");
    }

    public bool ValidaAfiliado()
    {
      return Ativo && Afiliado;
    }

    #endregion
  }
}
