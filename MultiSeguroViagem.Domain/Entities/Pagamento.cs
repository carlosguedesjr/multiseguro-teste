using System;
using System.Collections.Generic;
using System.Globalization;

namespace MultiSeguroViagem.Domain.Entities
{

    public class Pagamento
  {
    #region ctor

    protected Pagamento() { }

    public Pagamento(Pedido pedido, int status, int operadora, string nome, string email, string documento, string cep,
                    DateTime dataVencimento, decimal valor, decimal valorDescontoCupom, decimal valorDescontoBoleto, decimal valorDescontoTotal)
    {
      Pedido = pedido;
      Status = status;
      Operadora = operadora;
      Nome = nome;
      Email = email;
      Documento = documento;
      Cep = cep;
      DataVencimento = dataVencimento;
      Valor = valor;
      ValorDescontoCupom = valorDescontoCupom;
      ValorDescontoBoleto = valorDescontoBoleto;
      ValorDescontoTotal = valorDescontoTotal;
      DataCriacao = DateTime.Now;
    }


    #endregion

    #region properties

    public int IdPagamento { get; set; }
    public Pedido Pedido { get; set; }
    public int Status { get; set; }
    public int Operadora { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Documento { get; set; }
    public string Cep { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorDescontoCupom { get; set; }
    public decimal ValorDescontoBoleto { get; set; }
    public decimal ValorDescontoTotal { get; set; }
    public string CupomDesconto { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public DateTime? DataCancelamento { get; set; }

    #endregion

    #region methods

    public void DefineIdPagamento(int id)
    {
      IdPagamento = id;
    }

    public void DefineStatus(int status)
    {
      Status = status;
    }

    public void DefineDataPagamento(DateTime dataPagmento)
    {
      DataPagamento = dataPagmento;
    }

    public void DefineOperadora(int operadora)
    {
      Operadora = operadora;
    }

    public void DefinePedido(Pedido pedido)
    {
      Pedido = pedido;
    }

    /// <summary>
    /// Valida se existe dia de possível operação comercial entre a data da compra e data de ida para realizar o estorno do pagamento
    /// </summary>
    /// <returns>Retorna true quando existe dia/hora útil entre a data da compra e a data de ida</returns>
    public bool ValidaDiaUtilEntreCompraEmbarque(DateTime dataCompra, DateTime dataIda, List<DiasUteisSemana> diasUteisSemana, List<DiasUteisExcecao> diasUteisExcecao)
    {
      var culture = new CultureInfo("pt-BR");
      var diasUteis = 0;

      var dataAux = dataCompra;

      while (dataAux.Date <= dataIda.Date)
      {
        var excecao = diasUteisExcecao.Find(x => x.Data == dataAux.ToString("dd/MM") && x.Status);
        if (excecao == null)
        {
          var diaSemana = diasUteisSemana.Find(x => x.DiaSemana.ToLower().Equals(culture.DateTimeFormat.GetDayName(dataAux.DayOfWeek).ToLower()) && x.Status);

          if (diaSemana != null && (dataAux != dataCompra || dataAux >= DateTime.Parse($"{dataAux:dd/MM/yyyy} {diaSemana.HoraInicio}:00") && dataAux <= DateTime.Parse($"{dataAux:dd/MM/yyyy} {diaSemana.HoraFinal}:00")))
          {
            diasUteis++;
          }
        }
        else
        {
          if (!string.IsNullOrEmpty(excecao.HoraInicio)
              && !string.IsNullOrEmpty(excecao.HoraFinal)
              && (dataAux != dataCompra || (dataAux >= DateTime.Parse($"{excecao.Data}/{DateTime.Now.Year} {excecao.HoraInicio}")
              && dataAux <= DateTime.Parse($"{excecao.Data}/{DateTime.Now.Year} {excecao.HoraFinal}"))))
          {
            diasUteis++;
          }
        }

        dataAux = dataAux.AddDays(1);
      }

      return diasUteis > 0;
    }

    #endregion
  }
}
