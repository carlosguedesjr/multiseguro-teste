using System.Linq;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class UsuarioRepository : IUsuarioRepository
  {
    private readonly string _cnx;

    public UsuarioRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }
    
    public Usuario Busca(string email)
    {
      const string sql = "SELECT users.* FROM Usuarios users WHERE users.Email = @Email";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var data = cnx.Query<Usuario>(sql, new { Email = email }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        return data;
      }
    }

    public Usuario Busca(int idUsuario)
    {
      const string sql = "SELECT users.*  FROM Usuarios users WHERE users.IdUsuario =  @IdUsuario";

      using (var cnx = new MySqlConnection(_cnx))
      {
        var data = cnx.Query<Usuario>(sql, new { IdUsuario = idUsuario }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        return data;
      }
    }

    public Usuario Cadastra(Usuario usuario)
    {
      const string sql = @"INSERT INTO Usuarios(Nome, Email, Senha, Telefone, Documento, Endereco, Cep, Numero, Complemento, Bairro, Cidade, Estado, DataCriacao)
                                 VALUES(@Nome, @Email, @Senha, @Telefone, @Documento, @Endereco, @Cep, @Numero, @Complemento, @Bairro, @Cidade, @Estado, @DataCriacao); 
                               
                                 SELECT LAST_INSERT_ID();";

      using (var cnx = new MySqlConnection(_cnx))
      {
        var id = cnx.Query<int>(sql, usuario).Single();

        usuario.DefineIdUsuario(id);

        MySqlConnection.ClearPool(cnx);

        return usuario;
      }
    }

    public void Atualiza(Usuario usuario)
    {
      const string sql = @"UPDATE Usuarios
                                 SET Nome = @Nome, Email = @Email, Senha = @Senha, Status = @Status, Telefone = @Telefone,
                                        Documento = @Documento, Cep = @Cep, Endereco = @Endereco, Numero = @Numero, Complemento = @Complemento,
                                        Bairro = @Bairro, Cidade = @Cidade, Estado = @Estado, DataAlteracao = @DataAlteracao
                                 WHERE IdUsuario = @IdUsuario;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, usuario);

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void Atualiza(int idUsuario, string nome, string telefone, string documento, string cep, string endereco, string numero, string complemento, string bairro, string cidade, string estado)
    {
      const string sql = @"UPDATE Usuarios
                           SET
                              Nome = @Nome,
                              Telefone = @Telefone,
                              Documento = @Documento,
                              Cep = @Cep,
                              Endereco = @Endereco,
                              Numero = @Numero,
                              Complemento = @Complemento,
                              Bairro = @Bairro,
                              Cidade = @Cidade,
                              Estado = @Estado                              
                           WHERE
                              IdUsuario = @IdUsuario;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new { Nome = nome,
                               Telefone = telefone,
                               Documento = documento,
                               Cep = cep,
                               Endereco = endereco,
                               Numero = numero,
                               Complemento = complemento,
                               Bairro = bairro,
                               Cidade = cidade,
                               Estado = estado,
                               IdUsuario = idUsuario});

        MySqlConnection.ClearPool(cnx);
      }
    }
  }
}