
using System;

namespace MultiSeguroViagem.Domain.Entities
{
    public class PedidoItem
    {
        protected PedidoItem() { }

        public PedidoItem(Pedido pedido, Plano plano, DateTime dataIda, DateTime dataVolta, DateTime dataVoltaNova, Destino destino, int dias, decimal valorTotal, int viajantes, decimal descontoBoletoDia, decimal descontoCupomDia, string valorNetDia, decimal ajustePorcentagemDia, decimal cambioDia, decimal descontoPlanoDia)
        {
            Pedido = pedido;
            Plano = plano;
            DataIda = dataIda;
            DataVolta = dataVolta;
            DataVoltaNova = dataVoltaNova;
            Destino = destino;
            Dias = dias;
            ValorTotal = valorTotal;
            Viajantes = viajantes;
            DataCriacao = DateTime.Now;
            DescontoBoletoDia = descontoBoletoDia;
            DescontoCupomDia = descontoCupomDia;
            ValorNetDia = valorNetDia;
            AjustePorcentagemDia = ajustePorcentagemDia;
            CambioDia = cambioDia;
            DescontoPlanoDia = descontoPlanoDia;


    }

        #region properties

        public int IdPedidoItens { get; private set; }
        public Pedido Pedido { get; private set; }
        public Plano Plano { get; private set; }
        public DateTime DataIda { get; private set; }
        public DateTime DataVolta { get; private set; }
        public DateTime DataVoltaNova { get; private set; }
        public Destino Destino { get; private set; }
        public int Dias { get; private set; }
        public decimal ValorTotal { get; private set; }
        public int Viajantes { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public decimal DescontoBoletoDia { get; private set; }
        public decimal DescontoCupomDia { get; private set; }
        public string ValorNetDia { get; private set; }
        public decimal AjustePorcentagemDia { get; private set; }
        public decimal CambioDia { get; private set; }
        public decimal DescontoPlanoDia { get; private set; }


    #endregion

    #region methods

    public void DefineIdItem(int id)
        {
            IdPedidoItens = id;
        }

        public void DefinePedido(Pedido pedido)
        {
            Pedido = pedido;
        }

        public void DefinePlano(Plano plano)
        {
            Plano = plano;
        }

        public void DefineDestino(Destino destino)
        {
            Destino = destino;
        }

   

    #endregion
  }
}
