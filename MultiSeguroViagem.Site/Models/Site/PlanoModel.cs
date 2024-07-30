using System.Collections.Generic;
namespace MultiSeguroViagem.Site.Models.Site
{
  public class PlanoModel
  {
    public int IdPlano { get; set; }
    public int IdStatusMinDias { get; set; }
    public int CotacaoDiasMinimo { get; set; }
    public int CotacaoDiasMaximo { get; set; }
    public string Nome { get; set; }
    public OperadoraModel Operadora { get; set; }
    public int IdadeMinima { get; set; }
    public int IdadeMaxima { get; set; }
    public int MelhorIdadeMinima { get; set; }
    public int MelhorIdadeMaxima { get; set; }
    public string MelhorIdadeValorAcrescimo { get; set; }
    public decimal AjustePorcentagem { get; set; }
    public decimal Desconto { get; set; }
    //public string Observacao { get;  set; }
    public DestinoModel Destino { get; set; }
    public string Preco { get; set; }
    public string PrecoComercial { get; set; }
    public string PrecoMelhorIdade { get; set; }
    public bool Gestante { get; set; }
    public bool PraticaEsporte { get; set; }
    public bool Estudante { get; set; }
    public bool MaisVendido { get; set; }
    public IList<CoberturaModel> Coberturas { get; set; }
    public IList<ArquivoUploadModel> Arquivos { get; set; }
    public IList<RealSegurosModel> RealSeguros { get; set; }

  }

  public class EnviaComparacaoPlanoModel
  {
    public int IdPlano { get; set; }
    public int IdStatusMinDias { get; set; }
    public int CotacaoDiasMinimo { get; set; }
    public int CotacaoDiasMaximo { get; set; }
    public string Nome { get; set; }
    public OperadoraModel Operadora { get; set; }
    public decimal AjustePorcentagem { get; set; }
    public decimal Desconto { get; set; }
    public int IdadeMinima { get; set; }
    public int IdadeMaxima { get; set; }
    public int MelhorIdadeMinima { get; set; }
    public int MelhorIdadeMaxima { get; set; }
    public string MelhorIdadeValorAcrescimo { get; set; }
    //public string Observacao { get;  set; }
    public DestinoModel Destino { get; set; }
    public string Preco { get; set; }
    public string PrecoComercial { get; set; }
    public string PrecoMelhorIdade { get; set; }
    public bool Gestante { get; set; }
    public bool PraticaEsporte { get; set; }
    public bool Estudante { get; set; }
    public bool MaisVendido { get; set; }
    public IList<CoberturaModel> Coberturas { get; set; }
    public IList<ArquivoUploadModel> Arquivos { get; set; }
  }
}
