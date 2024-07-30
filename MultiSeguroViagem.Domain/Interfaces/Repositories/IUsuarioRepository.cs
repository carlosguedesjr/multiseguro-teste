using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IUsuarioRepository
  {
    Usuario Busca(int idUsuario);
    Usuario Busca(string email);

    Usuario Cadastra(Usuario usuario);

    void Atualiza(Usuario usuario);
    void Atualiza(int idUsuario, string nome, string telefone, string documento, string cep, string endereco, string numero, string complemento, string bairro, string cidade, string estado);
  }
}