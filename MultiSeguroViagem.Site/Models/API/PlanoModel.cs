using System.Collections.Generic;

namespace MultiSeguroViagem.Site.Models.Api
{
  public class PlanoModel
  {
    public int IdPlano { get; set; }
    public string Nome { get; set; }
    public OperadoraModel Operadora { get; set; }
    public PlanoStatusModel Status { get; set; }
    public int IdadeMinima { get; set; }
    public int IdadeMaxima { get; set; }
    public int MelhorIdadeMinima { get; set; }
    public int MelhorIdadeMaxima { get; set; }
    public decimal MelhorIdadePorcentagemAcrescimo { get; set; }
    public decimal MelhorIdadeValorAcrescimo { get; set; }
    public int CotacaoDiasMinimo { get; set; }
    public int CotacaoDiasMaximo { get; set; }
    public string Observacao { get; set; }
    public bool Dolar { get; set; }
    public bool Gestante { get; set; }
    public bool PraticaEsporte { get; set; }
    public bool Estudante { get; set; }
    public bool MaisVendido { get; set; }
    public decimal PrecoComercial { get; set; }
    public decimal PrecoMelhorIdade { get; set; }
    public decimal Desconto { get; set; }
    public DestinoModel Destino { get; set; }
    public decimal Preco { get; set; }
    public IList<CoberturaModel> Coberturas { get; set; }
    public IList<ArquivoUploadModel> Arquivos { get; set; }
  }
}