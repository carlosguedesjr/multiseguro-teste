using System;

namespace MultiSeguroViagem.Domain.Entities
{
  public class Viajante
  {
    #region Constructor

    protected Viajante() { }

    public Viajante(string nome, string cpf, DateTime dataNascimento, int sexo, decimal valorUnitario, string nomePlano)
    {
      Nome = nome;
      Cpf = cpf;
      DataNascimento = dataNascimento;
      Sexo = sexo;
      ValorUnitario = valorUnitario;
      NomePlano = nomePlano;
    }

    public Viajante(PedidoItem item, decimal valorUnitario, string nome, string cpf, DateTime dataNascimento, int sexo, string condicao)
    {
      Item = item;
      ValorUnitario = valorUnitario;
      Nome = nome;
      Cpf = cpf;
      DataNascimento = dataNascimento;
      Sexo = sexo;
      Condicao = condicao;
    }

    #endregion

    #region properties

    public int IdViajante { get; private set; }
    public PedidoItem Item { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public int Sexo { get; private set; }
    public string Condicao { get; private set; }
    public string NomePlano { get; private set; }

    public string IdVoucher { get; private set; }
    public string CodigoBilhete { get; private set; }
    public string CodigoVoucher { get; private set; }
    public string UriVoucher { get; private set; }
    public string UriCondicaoGeral { get; private set; }

    #endregion

    #region Métodos

    public void AtualizaIdVoucher(string id)
    {
      IdVoucher = id;
    }

    public void AtualizaCodigoBilhete(string codigo)
    {
      CodigoBilhete = codigo;
    }

    public void AtualizaVoucher(string codigo)
    {
      CodigoVoucher = codigo;
    }

    public void AtualizaUriVoucher(string uri)
    {
      UriVoucher = uri.Contains("http://") || uri.Contains("https://") ? uri : uri.Insert(0, "http://");
    }

    public void AtualizaUriCondicaoGeral(string uri)
    {
      UriCondicaoGeral = uri;
    }

    #endregion
  }
}
