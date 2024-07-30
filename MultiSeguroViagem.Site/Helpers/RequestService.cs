using System.Collections.Generic;
using RestSharp;

namespace MultiSeguroViagem.Site.Helpers
{
	public static class RequestService
	{
		/// <summary>
		/// Realiza requisições GET para a URI determinada
		/// </summary>
		/// <param name="urn">URN</param>
		/// <param name="url">URI</param>
		/// <returns>Dictionary: statusCode e conteudo do request</returns>
		public static Dictionary<string, string> Get(string urn, string url)
		{
			var client = new RestClient(urn);

			var request = new RestRequest(url, Method.GET);

			var response = client.Execute<dynamic>(request);

			var result = new Dictionary<string, string>
			{
				{"statusCode", response.StatusCode.ToString()},
				{"conteudo", response.Content}
			};

			return result;
		}
	}
}