using System;
using System.Collections.Generic;
using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Domain.Entities
{
    public class IntencaoCompra : Intencao
    {
        #region Constructor

        protected IntencaoCompra() {}

        public IntencaoCompra(string destino, DateTime dataIda, DateTime dataVolta, string email, string nome, string telefone, string origem, string referrer, string ip, ICollection<Viajante> viajantes)
        {
            Destino = destino;
            DataIda = dataIda;
            DataVolta = dataVolta;
            Email = email;
            Origem = origem;
            Nome = nome;
            Telefone = telefone;
            Referrer = referrer;
            Ip = ip;
            Viajantes = viajantes;
        }

        #endregion

        #region Propriedades

        public ICollection<Viajante> Viajantes { get; set; }

        #endregion

        #region Metodos
        public void ValidaIntencaoCompra()
        {
            Valida();
            AssertionConcern.AssertArgumentNotNull(Viajantes, "Os viajantes a ser cadastrados como intenção não podem serem nulos");
        }
        #endregion
    }
}
