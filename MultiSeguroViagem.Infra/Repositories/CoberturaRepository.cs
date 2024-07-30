using System.Collections.Generic;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra.Repositories
{
    public class CoberturaRepository : ICoberturaRepository
    {
        private readonly string _cnx;

        public CoberturaRepository()
        {
            _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
        }
        public ICollection<Cobertura> Busca(int idPlano)
        {
            const string query = @"SELECT 
                                    cob.IdCobertura,
                                    cob.Nome,
                                    pc.IdPlano,
                                    pc.Incluso,
                                    pc.ValorCobertura,
                                    pc.MoedaCobertura,
                                    pc.ValorAcrescimo,
                                    pc.MoedaAcrescimo,
                                    pc.ValorAcrescimo,
                                    pc.MensagemPlano
                                FROM
                                    Coberturas cob
                                        INNER JOIN
                                    PlanoCoberturas pc ON pc.IdCobertura = cob.IdCobertura
                                WHERE
                                    pc.idPlano = @IdPlano;";

            using (var cnx = new MySqlConnection(_cnx))
            {
                cnx.Open();

                var coberturas = (ICollection<Cobertura>)cnx.Query<Cobertura>(query, new { IdPlano = idPlano });

                MySqlConnection.ClearPool(cnx);

                return coberturas;
            }
        }
    }
}
