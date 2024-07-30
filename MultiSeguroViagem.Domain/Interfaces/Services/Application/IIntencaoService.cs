using System;
using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
    public interface IIntencaoService
    {
        /// <summary>
        /// Cadastra uma intenção de cotação
        /// </summary>
        void Cadastra(string destino, DateTime dataIda, DateTime dataVolta, string email, string nome, string telefone, string origem, string referer, string ip);

        /// <summary>
        /// Cadastra uma intenção de compra
        /// </summary>
        void Cadastra(string destino, DateTime dataIda, DateTime dataVolta, string email, string nome, string telefone, string origem, string referer, string ip, ICollection<Viajante> viajantes);

        /// <summary>
        /// Cadastra uma intenção de pagamento
        /// </summary>
        void Cadastra(string idDestino, DateTime dataIda, DateTime dataVolta, string email, string origem, string referer, string ip, ICollection<Viajante> viajantes, string emailPagamento, string telefone, string cep, int idPedido, int idPagamento, decimal valor, decimal valorDesconto, string cupomDesconto, string tipoPagamento, bool pago);

        /// <summary>
        /// Atualiza status do pagamento
        /// </summary>
        void AtualizaStatusPagamento(int idPagamento);

        /// <summary>
        /// Retorna o nome do destino de acordo com o ID informado
        /// </summary>
        string TrataDestino(string idDestino);
    }
}
