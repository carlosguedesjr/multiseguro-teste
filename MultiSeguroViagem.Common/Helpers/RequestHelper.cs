using RestSharp;
using System.Web.Script.Serialization;
using MultiSeguroViagem.Common.Models;

namespace MultiSeguroViagem.Common.Helpers
{
  public static class RequestHelper
  {
    public static string BuscaToken(string url, string usuario, string senha)
    {
      var retorno = string.Empty;
      var client = new RestClient(url);

      var request = new RestRequest(Method.POST);
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", $"grant_type=password&Username={usuario}&Password={senha}", ParameterType.RequestBody);

      var response = client.Execute(request);

      if (response.StatusCode != System.Net.HttpStatusCode.OK)
        return retorno;

      var tokenResponse = new JavaScriptSerializer().Deserialize<TokenModel>(response.Content);

      retorno = tokenResponse.Access_Token;

      return retorno;
    }


    public static IRestResponse Post(string url, string token, string json)
    {
      var client = new RestClient(url);
      var request = new RestRequest(Method.POST);
      request.AddHeader("authorization", $"Bearer {token}");
      request.AddHeader("content-type", "application/json");
      request.AddParameter("application/json", json, ParameterType.RequestBody);
      return client.Execute(request);
    }
  }
}
