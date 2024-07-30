using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MultiSeguroViagem.Domain.Interfaces.Repositories;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class ComparacaoRepository : IComparacaoRepository
  {
    public void CadastraLead(string destino, DateTime dataIda, DateTime dataVolta, string email, string planosId)
    {
      try
      {
        var db = GetDatabase();

        var collection = db.GetCollection<BsonDocument>("comparacoes");

        var lead = new BsonDocument
        {
          { "destino", destino },
          { "dataIda", dataIda },
          { "dataVolta", dataVolta },
          { "email", email },
          { "planosId", planosId }
        };

        collection.InsertOne(lead);
      }
      catch (Exception e) { }
    }

    private static IMongoDatabase GetDatabase()
    {
      var client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
      return client.GetDatabase("MultiSeguroViagem");
    }
  }
}
