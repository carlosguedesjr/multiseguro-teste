

using Dapper;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using System.Linq;

namespace MultiSeguroViagem.Infra.Repositories
{
	public class TokenRepository : ITokenRepository
	{
		private readonly string _cnx;

		public TokenRepository()
		{
			_cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
		}

		public string BuscaToken(string nomeApi)
		{
			const string sql = @"SELECT 
									Token
								FROM
									multiSeguroViagem.ApiTokens
								WHERE
									NomeApi = @nomeApi;
								";

			using (var cnx = new MySqlConnection(_cnx))
			{
				cnx.Open();

				var token = cnx.Query<string>(sql, new { nomeApi }).FirstOrDefault();


				MySqlConnection.ClearPool(cnx);

				return token;
			}
		}

		public void AtualizaToken(string nomeApi, string token)
		{
			const string sql = @"UPDATE 
									multiSeguroViagem.ApiTokens									
								SET
									Token = @token
								WHERE
									NomeApi = @nomeApi;
								";

			using (var cnx = new MySqlConnection(_cnx))
			{
				cnx.Open();
				cnx.Execute(sql, new { nomeApi, token });   

				MySqlConnection.ClearPool(cnx);        
		 
			}
		}
	}
}
