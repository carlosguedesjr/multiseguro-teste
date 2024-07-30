using System;
using System.Text.RegularExpressions;
using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Domain.Entities
{
  public class Intencao
  {
    #region Constructor

    protected Intencao() { }

    public Intencao(string destino, DateTime dataIda, DateTime dataVolta, string email, string nome, string telefone, string origem, string referrer, string ip)
    {
      Destino = destino;
      DataIda = dataIda;
      DataVolta = dataVolta;
      Email = email;
      Nome = nome;
      Telefone = telefone;
      Origem = origem;
      Referrer = referrer;
      Ip = ip;
    }

    #endregion

    #region Propriedades

    public string Destino { get; set; }
    public DateTime DataIda { get; set; }
    public DateTime DataVolta { get; set; }
    public string Email { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Origem { get; set; }
    public string Referrer { get; set; }
    public string Ip { get; set; }
    #endregion

    #region Metodos

    public void Valida()
    {
      AssertionConcern.AssertArgumentNotNull(Destino, "O destino a ser cadastrado como intenção não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(DataIda, "A data de ida a ser cadastrada como intenção não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(DataVolta, "A data de volta a ser cadastrada como intenção não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(Email, "O email a ser cadastrado como intenção não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(Origem, "A origem a ser cadastrada como intenção não pode ser nulo");
      AssertionConcern.AssertArgumentNotNull(Ip, "O ip a ser cadastrado como intenção não pode ser nulo");

      if (Referrer == null)
        Referrer = "Indefinido";
    }
    #endregion

    public bool BlackList()
    {
      const string pattern = @"(desenvolvimento@cenariocapital.com.br)|(mariane.montedori@cenarioleads.com.br)|(mariane.montedori@gmail.com)|(lara.fernandes@afiliace.com.br)|(carlos.nogueira@cenariocapital.com.br)|(fabio.vieira@cenariocapital.com.br)|(diego.souza@cenariocapital.com.br)|(danilo.baldin@cenariocapital.com.br)|(bruno@rossialves.com.br)";

      return Regex.IsMatch(Email.ToLower(), pattern, RegexOptions.IgnoreCase);
    }
  }
}