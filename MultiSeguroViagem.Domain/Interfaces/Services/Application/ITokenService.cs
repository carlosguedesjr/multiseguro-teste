

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface ITokenService
  {
    string BuscaToken(string nomeApi);
    string AtualizaToken();
  }
}
