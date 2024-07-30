using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Common.Helpers;
using System.Configuration;

namespace MultiSeguroViagem.Application.Services
{
  public class TokenService : ITokenService
  {
    readonly ITokenRepository _repo;
    public TokenService(ITokenRepository repo)
    {
      _repo = repo;
    }
    public string BuscaToken(string nomeApi)
    {
      return _repo.BuscaToken(nomeApi);
    }

    public string AtualizaToken()
    {
      var url = $"{ConfigurationManager.AppSettings["uriApiMultiIntegra"]}/token";

      var token = RequestHelper.BuscaToken(url, ConfigurationManager.AppSettings["usuarioApi"], ConfigurationManager.AppSettings["senhaApi"]);
      
      _repo.AtualizaToken(Constantes.API_MULTIINTEGRA_NOME, token);

      return token;
    }
  }
}
