using System;
using MultiSeguroViagem.Common.Resources;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using static MultiSeguroViagem.Common.Helpers.Funcoes;

namespace MultiSeguroViagem.Application.Services
{
  public class UsuarioService : IUsuarioService
  {
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
      _repo = repo;
    }

    public Usuario BuscaPorId(int idUsuario)
    {
      var usuario = _repo.Busca(idUsuario);
      if (usuario == null)
        throw new Exception(UsuarioErros.UsuarioNaoEncontrado);

      return usuario;
    }

    public Usuario BuscaPorEmail(string email)
    {
      var user = _repo.Busca(email);
      if (user == null)
        throw new Exception(UsuarioErros.UsuarioNaoEncontrado);

      return user;
    }

    public Usuario Cadastra(string nome, string email, string senha, string telefone, string documento, string cep, string endereco, string numero,
                   string complemento, string bairro, string cidade, string estado)
    {
      var usuarioExiste = _repo.Busca(email);
      if (usuarioExiste != null)
        throw new Exception(UsuarioErros.EmailDuplicado);

      var usuario = new Usuario(nome, email, senha, telefone, documento, cep, endereco, numero, complemento, bairro, cidade, estado);

      usuario.Valida();

      return _repo.Cadastra(usuario);
    }

    public void Atualiza(int idUsuario, string nome, string email, string senha, string telefone, string documento, string cep, string endereco, string numero,
                   string complemento, string bairro, string cidade, string estado)
    {
      var usuario = BuscaPorId(idUsuario);

      usuario.DefineNome(nome);
      usuario.DefineEmail(email);
      usuario.DefineSenha(senha);
      usuario.DefineTelefone(telefone);
      usuario.DefineDocumento(documento);
      usuario.DefineCep(cep);
      usuario.DefineEndereco(endereco);
      usuario.DefineNumero(numero);
      usuario.DefineComplemento(complemento);
      usuario.DefineBairro(bairro);
      usuario.DefineCidade(cidade);
      usuario.DefineEstado(estado);
      usuario.DefineDataAlteracao();
      usuario.Valida();

      _repo.Atualiza(usuario);
    }

    public void Deleta(int idUsuario)
    {
      var usuario = BuscaPorId(idUsuario);
      usuario.Desativa();

      _repo.Atualiza(usuario);
    }

    public Usuario Autenticacao(string email, string senha)
    {
      var usuario = BuscaPorEmail(email);

      if (usuario.Senha != Encrypt(senha))
        throw new Exception(UsuarioErros.CredenciaisInvalidas);

      if (usuario.Status == false)
        throw new Exception(UsuarioErros.UsuarioDesativado);

      return usuario;
    }

    public string ResetaSenha(string email)
    {
      var usuario = BuscaPorEmail(email);
      var senha = usuario.ResetaSenha();
      usuario.Valida();

      _repo.Atualiza(usuario);
      return senha;
    }

    public void ResetaSenha(Usuario usuario, string senha)
    {
      usuario.DefineSenha(senha);
      _repo.Atualiza(usuario);
    }

    public Usuario RegistraUsuarioCheckout(string nome, string email, string senha, string telefone, string documento, string cep, string endereco, string numero, string complemento, string bairro, string cidade, string estado, out bool novoUsuario)
    {
      var usuario = _repo.Busca(email);

      if (usuario != null)
      {
        novoUsuario = false;
        _repo.Atualiza(usuario.IdUsuario, nome, telefone, documento, cep, endereco, numero, complemento, bairro, cidade, estado);
        return usuario;
      }

      usuario = new Usuario(nome, email, senha, telefone, documento, cep, endereco, numero, complemento, bairro, cidade, estado);
      usuario.Valida();
      novoUsuario = true;

      return _repo.Cadastra(usuario);
    }
  }
}