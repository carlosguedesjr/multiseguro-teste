using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;

namespace MultiSeguroViagem.Application.Services
{
    public class ContatoEmergenciaService : IContatoEmergenciaService
    {
        private readonly IContatoEmergenciaRepository _repo;

        public ContatoEmergenciaService(IContatoEmergenciaRepository repo)
        {
            _repo = repo;
        }

        public ContatoEmergencia BuscaContato(int idUsuario)
        {
            return _repo.BuscaContato(idUsuario);
        }

        public void Cadastra(Usuario usuario, string nome, string telefone)
        {
            var contato = new ContatoEmergencia(usuario, nome, telefone);
            contato.Valida();

            _repo.CadastraContato(contato);
        }
    }
}
