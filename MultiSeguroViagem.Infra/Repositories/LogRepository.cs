using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class LogRepository : ILogRepository
  {
    public void FalhaEmissaoVoucher(Pagamento pagamento, IList<Viajante> viajantes, string nomeOperadora, string json, string exception)
    {
      var pagamentoJson = new JavaScriptSerializer().Serialize(pagamento);
      var viajantesJson = new JavaScriptSerializer().Serialize(viajantes);

      const string sql = @"INSERT INTO LogEmissaoVouchers(IdPedido, Pagamento, Viajantes, Operadora, JsonRequest, Exception)
                                 VALUES(@IdPedido,
                                        @Pagamento,
                                        @Viajantes,
                                        @Operadora,
                                        @JsonRequest,
                                        @Exception);";

      using (var cnx = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString))
      {
        cnx.Execute(sql, new
        {
          pagamento.Pedido.IdPedido,
          Pagamento = pagamentoJson,
          Viajantes = viajantesJson,
          Operadora = nomeOperadora,
          JsonRequest = json,
          exception
        });

        MySqlConnection.ClearPool(cnx);
      }
    }
  }
}
