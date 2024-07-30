using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Entities
{
    public class Pedido
    {
        #region Constructor

        protected Pedido() { }

        public Pedido(Usuario usuario, int status, decimal valorTotal)
        {
            Usuario = usuario;
            Status = new PedidoStatus { IdPedidoStatus = status };
            ValorTotal = valorTotal;
            DataCriacao = System.DateTime.Now;
        }

        #endregion

        #region properties

        public int IdPedido { get; private set; }
        public Usuario Usuario { get; private set; }
        public Pedido PedidoOrigem { get; private set; }
        public int Responsavel { get; private set; }
        public PedidoStatus Status { get; private set; }
        public decimal ValorTotal { get; private set; }
        public System.DateTime DataCriacao { get; private set; }
        public bool? CancelamentoSolicitado { get; private set; }
        public System.DateTime? DataCancelamento { get; private set; }
        public string ProtocoloCancelamento { get; private set; }
        public string ObservacaoCancelamento { get; private set; }
        public IEnumerable<PedidoItem> Itens { get; private set; }
        public string NomePlano { get; private set; }
        public IList<ArquivoUpload> Arquivos { get; private set; }
        public int BitTentativa { get; private set; }

    #endregion

    #region methods

    public void DefineIdPedido(int id)
        {
            IdPedido = id;
        }

        public void DefineStatus(PedidoStatus status)
        {
            Status = status;
        }
        public void DefineUsuario(Usuario usuario)
        {
            Usuario = usuario;
        }

        public void DefineCancelamento(bool cancelamento)
        {
            CancelamentoSolicitado = cancelamento;
        }

        public void DefineDataCancelamento()
        {
            DataCancelamento = System.DateTime.Now;
        }

        public void DefineProtocolo(string protocolo)
        {
            ProtocoloCancelamento = protocolo;
        }

        public void DefineObservacao(string observacao)
        {
            ObservacaoCancelamento = observacao;
        }

        public void DefinePedidoItem(IEnumerable<PedidoItem> itens)
        {
            Itens = itens;
        }

        public void DefineArquivos(IList<ArquivoUpload> arquivos)
        {
            Arquivos = arquivos;
        }
    
        #endregion
    }
}
