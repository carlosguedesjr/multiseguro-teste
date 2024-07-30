using System.Collections.Generic;
using System.Linq;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class BlackListFraudeRepository : IBlackListFraudeRepository
  {
    public dynamic  BuscaIp()
    {
      const string sql = @"SELECT 
                               *
                           FROM
                               multiSeguroViagem.BlacklistFraude
                           Where datExpiracao > NOW()
                           and bitExcluido = 0";


    
      using (var cnx = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString))
      {
       
        var ip = cnx.Query(sql).ToList();


        MySqlConnection.ClearPool(cnx);

        return ip;
      }
    }
  }
}
