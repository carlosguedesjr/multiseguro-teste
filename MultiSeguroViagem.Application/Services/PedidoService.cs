
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using MultiSeguroViagem.Common.Validations;


namespace MultiSeguroViagem.Application.Services
{
  public class PedidoService : IPedidoService
  {
    private readonly IPedidoRepository _repo;

    public PedidoService(IPedidoRepository repo)
    {
      _repo = repo;
    }


    public PedidoItem Busca(int idPedido)
    {
      return _repo.Busca(idPedido);
    }

    public IList<Pedido> ObtemPedidos(int idUsuario)
    {
      return _repo.ObtemPedidos(idUsuario);
    }

    public IList<Viajante> ObtemPedidoViajantes(int idPedido)
    {
      return _repo.ObtemPedidoViajantes(idPedido);
    }

    public IList<Pedido> ObtemPedidoDetalhes(int idPedido)
    {
      return _repo.ObtemPedidoDetalhes(idPedido);
    }


    //public void Atualiza(Pedido pedido, PedidoStatus status, bool? cancelamentoSolicitado, string protocoloCancelamento, string observacaoCancelamento)
    //{
    //  pedido.DefineStatus(status);
    //  if (cancelamentoSolicitado != null && (bool)cancelamentoSolicitado)
    //  {
    //    pedido.DefineCancelamento((bool)cancelamentoSolicitado);
    //    pedido.DefineProtocolo(protocoloCancelamento);
    //    pedido.DefineObservacao(observacaoCancelamento);
    //    pedido.DefineDataCancelamento();
    //  }

    //  _repo.Atualiza(pedido);
    //}

    public void Atualiza(int idPedido, int idStatus, decimal valorTotal)
    {
      _repo.Atualiza(idPedido, idStatus, valorTotal);
    }

    public void AtualizaStatus(int status, int idPedido)
    {
      AssertionConcern.AssertArgumentNotNull(status, "O status do pedido não pode ser nulo para atualizar");
      AssertionConcern.AssertArgumentNotNull(idPedido, "O ID do pedido não pode ser nulo para atualizar");

      _repo.AtualizaStatus(status, idPedido);
    }

    public void AtualizaViajante(IEnumerable<Viajante> viajantes)
    {
      _repo.AtualizaViajante(viajantes);
    }


    public Pedido Cadastra(string strIp, Usuario usuario, int status, decimal valorTotal)
    {
      var pedido = new Pedido(usuario, status, valorTotal);

      return _repo.Cadastra(strIp, pedido);
    }

    public PedidoItem CadastraItem(Pedido pedido, Plano plano, System.DateTime dataIda, System.DateTime dataVolta, System.DateTime dataVoltaNova, Destino destino, int dias, decimal valorTotal, int viajantes, decimal descontoBoletoDia, decimal descontoCupomDia, string valorNetDia, decimal ajustePorcentagemDia, decimal CambioDia, decimal DescontoPlanoDia)
    {

      var item = new PedidoItem(pedido, plano, dataIda, dataVolta, dataVoltaNova, destino, dias, valorTotal, viajantes, descontoBoletoDia, descontoCupomDia, valorNetDia, ajustePorcentagemDia, CambioDia, DescontoPlanoDia);

      return _repo.CadastraItem(item);
    }

    public void CadastraViajante(IEnumerable<Viajante> viajantes) => _repo.CadastraViajante(viajantes);
  }
}
