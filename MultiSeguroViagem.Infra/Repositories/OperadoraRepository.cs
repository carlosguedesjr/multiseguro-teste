using System.Collections.Generic;
using System.Linq;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class OperadoraRepository : IOperadoraRepository
  {
    public Operadora BuscaOperadora(string nome)
    {
      const string sql = @"SELECT 
                               *
                           FROM
                               multiSeguroViagem.Operadoras
                           WHERE
                               REPLACE(LOWER(REMOVE_ACCENTS(Nome)), ' ', '') = @operadora;";

      using (var cnx = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString))
      {
        var operadora = cnx.Query<Operadora>(sql, new { operadora = nome }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        return operadora;
      }
    }
  }
}
