using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class DiasUteisRepository : IDiasUteisRepository
  {
    private readonly string _cnx;

    public DiasUteisRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }

    public ICollection<DiasUteisExcecao> BuscaDiasExcecoes()
    {
      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var excecoes = (IList<DiasUteisExcecao>)cnx.Query<DiasUteisExcecao>("SELECT * FROM DiasUteisExcecao");

        MySqlConnection.ClearPool(cnx);

        return excecoes;
      }
    }

    public ICollection<DiasUteisSemana> BuscaDiasSemana()
    {
      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var diasUteis = (IList<DiasUteisSemana>)cnx.Query<DiasUteisSemana>("SELECT * FROM DiasUteisSemana");

        MySqlConnection.ClearPool(cnx);

        return diasUteis;
      }
    }
  }
}
