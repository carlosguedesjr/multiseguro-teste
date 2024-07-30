using System;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Entities
{
    public class IntencaoPaga : IntencaoCompra
    {
        #region Constructor

        protected IntencaoPaga() { }

        public IntencaoPaga(string destino, DateTime dataIda, DateTime dataVolta, string email, string origem, string referrer, string ip, ICollection<Viajante> viajantes, string emailPagamento, string telefone, string cep, int idPedido, int idPagamento, decimal valor, decimal valorDesconto,string cupomDesconto, string tipoPagamento, bool pago)
        {
            Destino = destino;
            DataIda = dataIda;
            DataVolta = dataVolta;
            Email = email;
            Origem = origem;
            Referrer = referrer;
            Ip = ip;
            Viajantes = viajantes;
            EmailPagamento = emailPagamento;
            Telefone = telefone;
            Cep = cep;
            IdPedido = idPedido;
            IdPagamento = idPagamento;
            Valor = valor;
            ValorDesconto = valorDesconto;
            CupomDesconto = cupomDesconto;
            TipoPagamento = tipoPagamento;
            Pago = pago;
        }

        #endregion

        #region Propriedades

        public string EmailPagamento { get; set; }
        public string Telefone { get; set; }
        public string Cep { get; set; }
        public int IdPedido { get; set; }
        public int IdPagamento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorDesconto { get; set; }
        public string CupomDesconto { get; set; }
        public string TipoPagamento { get; set; }
        public bool Pago { get; set; }

        #endregion

        #region Metodos
        public void ValidaIntencaoPaga()
        {
            ValidaIntencaoCompra();

            //AssertionConcern.AssertArgumentNotNull(Viajantes, "Os viajantes a ser cadastrados como intenção não podem serem nulos");
        }
        #endregion
    }
}
