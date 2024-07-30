using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
    public interface IContatoEmergenciaRepository 
    {
        void CadastraContato(ContatoEmergencia contato);

        ContatoEmergencia BuscaContato(int idUsuario);
    }
}
