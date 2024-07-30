using System.Collections.Generic;
using System.Linq;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class DestinoRepository : IDestinoRepository
  {
    private readonly string _cnx;

    public DestinoRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }

    public ICollection<Destino> Busca()
    {
      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var destinos = (IList<Destino>)cnx.Query<Destino>("SELECT * FROM Destinos");

        MySqlConnection.ClearPool(cnx);

        return destinos;
      }
    }

    public ICollection<DestinoSeo> BuscaDestinosSeo()
    {
      const string sql = @"SELECT 
                              DES.Nome AS NomeDestinoCotador,
                              SEO.IdDestino,
                              SEO.Slug,
                              SEO.Nome,
                              SEO.BlocoUmTitulo,
                              SEO.BlocoUmDescricao
                          FROM
                              multiSeguroViagem.SeoDestinos SEO
                                  INNER JOIN
                              Destinos DES ON SEO.IdDestino = DES.IdDestino;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var dados = cnx.Query<DestinoSeo>(sql).ToList();

        MySqlConnection.ClearPool(cnx);

        return dados;
      }
    }

    public DestinoSeo BuscaDestinoSeo(string destino)
    {
      const string sql = @"SELECT * FROM multiSeguroViagem.SeoDestinos where Slug = @destino;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var dados = cnx.Query<DestinoSeo>(sql, new { destino }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        return dados;
      }
    }
  }
}

