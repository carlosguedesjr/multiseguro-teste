using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
    public interface IContatoEmergenciaService 
    {
        void Cadastra(Usuario usuario, string nome, string telefone);

        ContatoEmergencia BuscaContato(int idUsuario);
    }
}
