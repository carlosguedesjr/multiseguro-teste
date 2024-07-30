using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;
using System;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class CupomDescontoRepository : ICupomDescontoRepository
  {
    private readonly string _cnx;

    public CupomDescontoRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }

    public CupomDesconto Busca(string codigo)
    {
      const string sql = @"SELECT 
	                            Cupons.*
                            FROM 
	                            Cupons
                            WHERE 
	                            upper(Cupons.Codigo) = upper(@Codigo)
                            AND Ativo = true;

                            SELECT 
                              CuponsOperadoras.IdCupom,
                              Operadoras.Nome 
	                          FROM 
                              CuponsOperadoras 
                            INNER JOIN 
                              Operadoras ON Operadoras.IdOperadora = CuponsOperadoras.IdOperadora;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var cup = cnx.QueryMultiple(sql, new { Codigo = codigo });

        var cupom = cup.Read<CupomDesconto>().FirstOrDefault();

        if (cupom == null)
          return null;

        var operadorasCumpom = cup.Read<CupomOperadoras>().ToList();

        cupom.DefineOperadora(operadorasCumpom.Where(x => x.IdCupom == cupom.IdCupom).ToList());

        MySqlConnection.ClearPool(cnx);

        return cupom;
      }
    }

    
  }
}
