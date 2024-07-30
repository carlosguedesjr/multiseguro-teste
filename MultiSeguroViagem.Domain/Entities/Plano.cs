using System.Collections.Generic;
using MultiSeguroViagem.Common.Validations;
using System;

namespace MultiSeguroViagem.Domain.Entities
{
  public class Plano
  {
    #region Construtor

    protected Plano() { }

    public Plano(string nome, Operadora operadora, PlanoStatus status, IList<RealSeguros> realseguros, int idadeMinima, int idadeMaxima, int melhorIdadeMinima, int melhorIdadeMaxima,
        int cotacaoDiasMinimo, int cotacaoDiasMaximo, string observacao, bool dolar)
    {
      Nome = nome;
      Operadora = operadora;
      Status = status;
      IdadeMinima = idadeMinima;
      IdadeMaxima = idadeMaxima;
      MelhorIdadeMinima = melhorIdadeMinima;
      MelhorIdadeMaxima = melhorIdadeMaxima;
      CotacaoDiasMinimo = cotacaoDiasMinimo;
      CotacaoDiasMaximo = cotacaoDiasMaximo;
      Observacao = observacao;
      Dolar = dolar;
      RealSeguros = realseguros;
    }

    #endregion

    #region Propriedades

    public int IdPlano { get; private set; }
    public int IdStatusMinDias { get; private set; }
    public string Nome { get; private set; }
    public string Imagem { get; private set; }
    public Operadora Operadora { get; private set; }
    public PlanoStatus Status { get; private set; }
    public int IdadeMinima { get; private set; }
    public int IdadeMaxima { get; private set; }
    public int MelhorIdadeMinima { get; private set; }
    public int MelhorIdadeMaxima { get; private set; }
    public decimal MelhorIdadePorcentagemAcrescimo { get; private set; }
    public decimal MelhorIdadeValorAcrescimo { get; private set; }
    public decimal AjustePorcentagem { get; private set; }
    public int CotacaoDiasMinimo { get; private set; }
    public int CotacaoDiasMaximo { get; private set; }
    public string Observacao { get; private set; }
    public bool Dolar { get; private set; }
    public bool Gestante { get; private set; }
    public bool PraticaEsporte { get; private set; }
    public bool Estudante { get; private set; }
    public bool MaisVendido { get; private set; }
    public decimal PrecoComercial { get; private set; }
    public decimal PrecoMelhorIdade { get; private set; }
    public decimal Desconto { get; private set; }
    public Destino Destino { get; private set; }
    public decimal Preco { get; private set; }
    public int QuantidadeAvaliacoes { get; private set; }
    public decimal MediaAvaliacoes { get; private set; }
    public IList<Cobertura> Coberturas { get; private set; }
    public IList<ArquivoUpload> Arquivos { get; private set; }
    public IList<RealSeguros> RealSeguros { get; set; }


    public string IdPlanoExterno { get; private set; }
    public string CodigoTarifaExterno { get; private set; }

    #endregion

    #region Métodos

    public void DefinePrecoComercial(decimal preco)
    {
      PrecoComercial = decimal.Round(preco, 4);
    }

    public void DefinePrecoMelhorIdade(decimal preco)
    {
      PrecoMelhorIdade = decimal.Round(preco, 4);
    }

    public decimal CalculaPrecoComercial(decimal preco, decimal desconto)
    {
      AssertionConcern.AssertArgumentNotNull(preco, "O preco não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(desconto, "O desconto não pode ser nulo");

      var precoComercial = preco - (desconto / 100) * preco;
      DefinePrecoComercial(precoComercial);

      return precoComercial;
    }

    public decimal CalculaPrecoMelhorIdade(decimal preco, decimal precoComercial, decimal porcentagemAcrescimo)
    {
      AssertionConcern.AssertArgumentNotNull(preco, "O preco não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(porcentagemAcrescimo, "A porcentagem de acrescimo não pode ser nula");
      AssertionConcern.AssertArgumentNotNull(precoComercial, "O preco comercial não pode ser nulo");

      if (porcentagemAcrescimo <= 0)
        DefinePrecoMelhorIdade(0);

      var precoMelhorIdade = (porcentagemAcrescimo / 100) * preco + precoComercial;

      DefinePrecoMelhorIdade(precoMelhorIdade);

      return precoMelhorIdade;
    }

    public void DefineOperadora(Operadora operadora)
    {
      Operadora = operadora;
    }

    public void DefineDestino(Destino destino)
    {
      Destino = destino;
    }

    public void DefineCoberturas(IList<Cobertura> coberturas)
    {
      Coberturas = coberturas;
    }


    public void DefineArquivos(IList<ArquivoUpload> arquivos)
    {
      Arquivos = arquivos;
    }

    #endregion
  }
}
