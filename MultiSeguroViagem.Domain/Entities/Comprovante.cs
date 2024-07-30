using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Domain.Entities
{
    public class Comprovante
    {
        #region Construtor

        protected Comprovante() { }

        public Comprovante(Pagamento pagamento, PagamentoRedeCard pagamentoRedeCard)
        {
            Pagamento = pagamento;
            PagamentoRedeCard = pagamentoRedeCard;
        }

        #endregion

        #region Propriedades

        public Pagamento Pagamento { get; private set; }
        public PagamentoRedeCard PagamentoRedeCard { get; private set; }
        //public Usuario Usuario { get; private set; }

        #endregion

        #region Métodos

        public void Valida()
        {
            AssertionConcern.AssertArgumentNotNull(Pagamento, "O pagamento não pode ser nulo");
        }

        #endregion
    }
}
