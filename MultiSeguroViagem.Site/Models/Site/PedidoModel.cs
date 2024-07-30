using System.Collections.Generic;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class PedidoModel
    {
        public int IdPedido { get; set; }
        public int BitTentativa { get; set; }
        public UsuarioModel Usuario { get; set; }
        public PedidoModel PedidoOrigem { get; set; }
        public int Responsavel { get; set; }
        public PedidoStatusModel Status { get; set; }
        public decimal ValorTotal { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public bool? CancelamentoSolicitado { get; set; }
        public System.DateTime? DataCancelamento { get; set; }
        public string ProtocoloCancelamento { get; set; }
        public string ObservacaoCancelamento { get; set; }
        public System.Collections.Generic.IEnumerable<PedidoItemModel> Itens { get; set; }
        public string NomePlano { get; set; }

        public IList<ArquivoUploadModel> Arquivos { get; set; }
    }
}