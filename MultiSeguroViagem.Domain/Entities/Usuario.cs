using MultiSeguroViagem.Common.Validations;
using System;
using MultiSeguroViagem.Common.Resources;
using MultiSeguroViagem.Common.Helpers;

namespace MultiSeguroViagem.Domain.Entities
{
    public class Usuario
    {
        #region Construtor

        protected Usuario() { }

        public Usuario(string nome, string email, string senha, string telefone,string documento, string cep, string endereco, string numero, 
                       string complemento, string bairro, string cidade, string estado)
        {
            Nome = nome;
            Email = email;
            DefineSenha(senha);
            Telefone = telefone;
            Documento = documento;
            Cep = cep;
            Endereco = endereco;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;          
            DataCriacao = DateTime.Now;
        }

        #endregion

        #region Propriedades    
        public int IdUsuario { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string Telefone { get; private set; }
        public string Documento { get; private set; }
        public string Cep { get; private set; }
        public string Endereco { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public bool Status { get; private set; }

        #endregion

        #region Métodos

        public void DefineNome(string nome)
        {
            AssertionConcern.AssertArgumentNotNull(nome, UsuarioErros.NomeNaoPodeSerNulo);
            AssertionConcern.AssertArgumentLength(nome, 4, 150, UsuarioErros.NomeTamanhoInvalido);

            Nome = nome;
            DataAlteracao = DateTime.Now;
        }

        public void DefineEmail(string email)
        {
            AssertionConcern.AssertArgumentNotNull(email, UsuarioErros.EmailNaoPodeSerNulo);
            AssertionConcernEmail.AssertEmailFormat(email);

            Email = email;
            DataAlteracao = DateTime.Now;
        }

        public void DefineSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha)) return;

            AssertionConcern.AssertArgumentNotNull(senha, UsuarioErros.SenhaInvalida);
            AssertionConcern.AssertArgumentBiggerThen(senha, 6, UsuarioErros.SenhaInvalida);

            Senha = Funcoes.Encrypt(senha);
            DataAlteracao = DateTime.Now;
        }

        public void DefineTelefone(string telefone)
        {
            AssertionConcern.AssertArgumentNotNull(telefone, UsuarioErros.TelefoneNaoPodeSerNulo);
            Telefone = telefone;
        }

        public void DefineDocumento(string documento)
        {
            AssertionConcern.AssertArgumentNotNull(documento, UsuarioErros.DocumentoNaoPodeSerNulo);
            Documento = documento;
        }

        public void DefineCep(string cep)
        {
            AssertionConcern.AssertArgumentNotNull(cep, "Cep não pode ser nulo");
            Cep = cep;
        }

        public void DefineEndereco(string endereco)
        {
            AssertionConcern.AssertArgumentNotNull(endereco, "Endereço não pode ser nulo");
            Endereco = endereco;
        }
        
        public void DefineNumero(string numero)
        {
            AssertionConcern.AssertArgumentNotNull(numero, "Número não pode ser nulo");
            Numero = numero;
        }

        public void DefineComplemento(string complemento)
        {
            Complemento = complemento;
        }

        public void DefineBairro(string bairro)
        {
            AssertionConcern.AssertArgumentNotNull(bairro, "Bairro não pode ser nulo");
            Bairro = bairro;
        }

        public void DefineCidade(string cidade)
        {
            AssertionConcern.AssertArgumentNotNull(cidade, "Cidade não pode ser nulo");
            Cidade = cidade;
        }

        public void DefineEstado(string estado)
        {
            AssertionConcern.AssertArgumentNotNull(estado, "Estado não pode ser nulo");
            Estado = estado;
        }        

        public void DefineDataAlteracao()
        {
            DataAlteracao = DateTime.Now;
        }

        public string ResetaSenha()
        {
            var senha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = Funcoes.Encrypt(senha);

            DataAlteracao = DateTime.Now;

            return senha;
        }

        public void Desativa()
        {
            Status = false;
            DataAlteracao = DateTime.Now;
        }

        public void Valida()
        {
            AssertionConcern.AssertArgumentNotNull(Nome, UsuarioErros.NomeNaoPodeSerNulo);
            AssertionConcern.AssertArgumentLength(Nome, 4, 150, UsuarioErros.NomeTamanhoInvalido);

            AssertionConcern.AssertArgumentNotNull(Email, UsuarioErros.EmailNaoPodeSerNulo);
            AssertionConcernEmail.AssertEmailFormat(Email);

            AssertionConcern.AssertArgumentNotNull(Senha, UsuarioErros.SenhaNaoPodeSerNulo);

        }

        public void DefineIdUsuario(int id)
        {
            IdUsuario = id;
        }

        #endregion
    }
}
