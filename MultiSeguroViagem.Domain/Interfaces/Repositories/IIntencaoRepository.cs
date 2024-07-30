using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
    public interface IIntencaoRepository
    {
        /// <summary>
        /// Cadastra uma intenção de cotação
        /// </summary>
        void Cadastra(Intencao intencao);

        /// <summary>
        /// Cadastra uma intenção de compra
        /// </summary>
        void Cadastra(IntencaoCompra intencao);

        /// <summary>
        /// Cadastra uma intenção de pagamento
        /// </summary>
        void Cadastra(IntencaoPaga intencao);

        /// <summary>
        /// Atualiza status do pagamento
        /// </summary>
        void AtualizaStatusPagamento(int idPagamento);
    }
}
