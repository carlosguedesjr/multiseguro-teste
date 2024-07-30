using MultiSeguroViagem.Domain.Entities;
using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Repositories
{
  public interface IPedidoRepository
  {
    PedidoItem Busca(int idPedido);

    IList<Pedido> ObtemPedidos(int idUsuario);

    IList<Viajante> ObtemPedidoViajantes(int idPedido);

    IList<Pedido> ObtemPedidoDetalhes(int idPedido);


    Pedido Cadastra(string strIp, Pedido pedido);

    PedidoItem CadastraItem(PedidoItem item);

    void CadastraViajante(IEnumerable<Viajante> viajantes);


    void Atualiza(int idPedido, int idStatus, decimal valorTotal);

    void AtualizaStatus(int status, int idPedido);

    void AtualizaViajante(IEnumerable<Viajante> viajantes);
  }
}
