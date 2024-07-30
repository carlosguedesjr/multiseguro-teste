using Dapper;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using MultiSeguroViagem.Domain.Entities;
using System.Linq;

namespace MultiSeguroViagem.Infra.Repositories
{
    public class ContatoEmergenciaRepository : IContatoEmergenciaRepository
    {
        private readonly string _cnx;

        public ContatoEmergenciaRepository()
        {
            _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
        }

        public void CadastraContato(ContatoEmergencia contato)
        {
            const string sql = @"INSERT INTO ContatoEmergencia(IdUsuario,Nome,Telefone,DataCriacao) 
                                    VALUES(@IdUsuario,@Nome,@Telefone,@DataCriacao);";

            using (var cnx = new MySqlConnection(_cnx))
            {
                cnx.Open();

                cnx.Execute(sql, new
                {

                    IdUsuario = contato.Usuario.IdUsuario,
                    Nome = contato.Nome,
                    Telefone = contato.Telefone,
                    DataCriacao = contato.DataCriacao

                });

                MySqlConnection.ClearPool(cnx);
            }
        }

        public ContatoEmergencia BuscaContato(int idUsuario)
        {
            const string sql = @"SELECT 
                               IdContatoEmergencia,
                               Nome,
                               Telefone
                            FROM
                                multiSeguroViagem.ContatoEmergencia
                                WHERE IdUsuario = @idUsuario
                            ORDER BY IdContatoEmergencia DESC
                            LIMIT 1;";

            using (var cnx = new MySqlConnection(_cnx))
            {
                cnx.Open();

                var contato = cnx.Query<ContatoEmergencia>(sql, new { idUsuario }).FirstOrDefault();
                MySqlConnection.ClearPool(cnx);

                return contato;
            }
        }
    }
}
