
namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        string BuscaToken(string nomeApi);
        void AtualizaToken(string nomeApi, string token);
    }
}
