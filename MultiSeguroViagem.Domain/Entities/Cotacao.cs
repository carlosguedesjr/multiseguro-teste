using System;
using System.Globalization;
using System.Collections.Generic;
using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Domain.Entities
{
  public class Cotacao
  {
    #region Construtor

    protected Cotacao() { }

    public Cotacao(int idDestino, DateTime dataIda, DateTime dataVolta, ICollection<Plano> planos)
    {
      IdDestino = idDestino;
      DataIda = dataIda;
      DataVolta = dataVolta;
      Planos = planos;
    }

    #endregion

    #region Propriedades

    public DateTime DataIda { get; private set; }
    public DateTime DataVolta { get; private set; }

    public int IdDestino { get; private set; }
    public ICollection<Plano> Planos { get; private set; }

    #endregion

    #region Métodos

    public void CalculaPrecoComercial(decimal? cupomValorDesconto)
    {
      foreach (var plano in Planos)
      {
        
          var desconto = cupomValorDesconto ?? plano.Desconto;
          var ajuste = plano.AjustePorcentagem;
          var PrecoAjuste = plano.Preco;
       if (ajuste > 0)
          {
            PrecoAjuste = plano.Preco * (100 / (100 - ajuste));
          }
        var precoComercial = plano.CalculaPrecoComercial(PrecoAjuste, desconto);
  
        plano.CalculaPrecoMelhorIdade(PrecoAjuste, precoComercial, plano.MelhorIdadePorcentagemAcrescimo);
      }
    }

    public void Valida()
    {
      AssertionConcern.AssertArgumentNotNull(DataIda, "A data de ida não pode ser nula");
      AssertionConcern.AssertArgumentNotNull(DataVolta, "A data de volta não pode ser nula");
      AssertionConcern.AssertArgumentNotNull(IdDestino, "O ID do destino não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(Planos, "A lista de planos não pode ser nula");

      var msgErro = string.Empty;

      if (DateTime.Compare(DataIda.Date, DateTime.Now.Date) < 0)
        msgErro = "A data de ida deve ser maior ou igual a hoje<br>";

      if (DateTime.Compare(DataVolta.Date, DateTime.Now.Date) < 0)
        msgErro += "A data de volta deve ser maior que hoje<br>";

      if (DateTime.Compare(DataVolta.Date, DataIda.Date) < 0)
        msgErro += "A data de volta deve ser maior que a data de ida";

      if (!string.IsNullOrEmpty(msgErro))
      {
        throw new InvalidOperationException(msgErro);
      }
      
    }

    public void SetaPlanos(ICollection<Plano> planos)
    {
      Planos = planos;
    }

    #endregion
  }
}
