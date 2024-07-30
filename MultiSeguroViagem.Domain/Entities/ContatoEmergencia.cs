using MultiSeguroViagem.Common.Validations;
using System;

namespace MultiSeguroViagem.Domain.Entities
{
    public class ContatoEmergencia
    {
        #region Construtor
        protected ContatoEmergencia() { }

        public ContatoEmergencia(Usuario usuario, string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
            Usuario = usuario;
            DataCriacao = DateTime.Now;
        }
        #endregion

        #region Propriedades   

        public int IdContatoEmergencia { get; private set; }
        public Usuario Usuario { get; private set; }
        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public DateTime DataCriacao { get; private set; }

        #endregion

        #region Métodos

        public void Valida()
        {
            AssertionConcern.AssertArgumentNotNull(Nome, "Nome do contato não pode ser nulo");
            AssertionConcern.AssertArgumentLength(Nome, 1, 150, "Tamanho invalido para o nome do contato");
            AssertionConcern.AssertArgumentNotNull(Telefone, "Telefone do contato não pode ser nulo");
            AssertionConcern.AssertArgumentNotNull(Usuario, "O contato precisa estar vinculado a um usuario");

        }

        #endregion
    }
}
