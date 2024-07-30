using System;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class PedidoItemModel
    {
        public int IdPedidoItens { get; private set; }
        public PedidoModel Pedido { get; private set; }
        public PlanoModel Plano { get; private set; }
        public DateTime DataIda { get; private set; }
        public DateTime DataVolta { get; private set; }
    public DateTime DataVoltaNova { get; private set; }
    public DestinoModel Destino { get; private set; }
        public int Dias { get; private set; }
        public decimal ValorTotal { get; private set; }
        public int Viajantes { get; private set; }
        public DateTime DataCriacao { get; private set; }
    }
}
