using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
    public interface IUsuarioService
    {
        Usuario BuscaPorId(int idUsuario);

        Usuario BuscaPorEmail(string email);

        Usuario Cadastra(string nome, string email, string senha, string telefone, string cpf, string cep, string endereco, string numero, string complemento, string bairro, string cidade, string estado);

        void Atualiza(int idUsuario, string nome, string email, string senha, string telefone, string documento, string cep, string endereco, string numero,
                       string complemento, string bairro, string cidade, string estado);

        void Deleta(int idUsuario);

        Usuario Autenticacao(string email, string senha);

        string ResetaSenha(string email);

        void ResetaSenha(Usuario usuario, string senha);

        Usuario RegistraUsuarioCheckout(string nome, string email, string senha, string telefone, string cpf, string cep, string endereco, string numero, string complemento, string bairro, string cidade, string estado, out bool novoUsuario);
    }
}