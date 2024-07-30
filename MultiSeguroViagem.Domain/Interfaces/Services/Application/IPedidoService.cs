using MultiSeguroViagem.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface IPedidoService
  {
    PedidoItem Busca(int idPedido);

    IList<Pedido> ObtemPedidos(int idUsuario);

    IList<Viajante> ObtemPedidoViajantes(int idPedido);

    IList<Pedido> ObtemPedidoDetalhes(int idPedido);


    Pedido Cadastra(string strIp, Usuario usuario, int status, decimal valorTotal);

    void CadastraViajante(IEnumerable<Viajante> viajantes);

    PedidoItem CadastraItem(Pedido pedido, Plano plano, DateTime dataIda, DateTime dataVolta, DateTime dataVoltaNova, Destino destino, int dias, decimal valorTotal, int viajantes, decimal descontoBoletoDia, decimal descontoCupomDia, string valorNetDia, decimal AjustePorcentagemDia, decimal CambioDia, decimal DescontoPlanoDia);


    void Atualiza(int idPedido, int idStatus, decimal valorTotal);

    void AtualizaStatus(int status, int idPedido);

    void AtualizaViajante(IEnumerable<Viajante> viajantes);
  }
}
