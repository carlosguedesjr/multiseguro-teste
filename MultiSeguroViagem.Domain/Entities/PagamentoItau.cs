using System;
using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Domain.Entities
{
    public class PagamentoItau
    {
        #region Construtor

        protected PagamentoItau() { }

        public PagamentoItau(Pagamento pagamento, string valor, string observacao, string nome, string codigoInscricao,
                       string numeroInscricao, string cep, DateTime dataVencimento, string urlRetorno,  string obsAdicional1, string obsAdicional2, string obsAdicional3)
        {
            Pagamento = pagamento;                     
            Valor = valor;
            Observacao = observacao;        
            Nome = nome;
            CodigoInscricao = codigoInscricao;
            NumeroInscricao = numeroInscricao;        
            Cep = cep;     
            DataVencimento = dataVencimento;
            UrlRetorno = urlRetorno;
            ObsAdicional1 = obsAdicional1;
            ObsAdicional2 = obsAdicional2;
            ObsAdicional3 = obsAdicional3;
        }

        #endregion

        #region properties

        public int IdPagamentoItau { get; private set; }
        public Pagamento Pagamento { get; private set; }
        public string Pedido { get; private set; }
        public string Valor { get; private set; }
        public string Observacao { get; private set; }
        public string Nome { get; private set; }
        public string CodigoInscricao { get; private set; }
        public string NumeroInscricao { get; private set; }
        public string Cep { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public string UrlRetorno { get; private set; }
        public string ObsAdicional1 { get; private set; }
        public string ObsAdicional2 { get; private set; }
        public string ObsAdicional3 { get; private set; }
        public string DataConsultaBoleto { get; private set; }

        #endregion

        #region methods

        public void Valida()
        {
            AssertionConcern.AssertArgumentNotNull(Pagamento, "O pagamento não pode ser nulo");  
        }

        public void DefineIdItau(int id)
        {
            IdPagamentoItau = id;
        }

        public void DefinePagamento(Pagamento pagamento)
        {
            Pagamento = pagamento;
        }

        #endregion
    }
}
