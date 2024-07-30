using System;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using System.Collections.Generic;

namespace MultiSeguroViagem.Application.Services
{
  public class IntencaoService : IIntencaoService
  {
    private readonly IIntencaoRepository _intencaoRepository;

    public IntencaoService(IIntencaoRepository intencaoRepository)
    {
      _intencaoRepository = intencaoRepository;
    }

    public void Cadastra(string idDestino, DateTime dataIda, DateTime dataVolta, string email,string nome, string telefone, string origem, string referer, string ip)
    {
      var intencao = new Intencao(TrataDestino(idDestino), dataIda, dataVolta, email, nome, telefone, origem, referer, ip);

      if(intencao.BlackList())
        return;

      intencao.Valida();

      _intencaoRepository.Cadastra(intencao);
    }

    public void Cadastra(string idDestino, DateTime dataIda, DateTime dataVolta, string email, string nome, string telefone, string origem, string referer, string ip, ICollection<Viajante> viajantes)
    {
      var intencao = new IntencaoCompra(TrataDestino(idDestino), dataIda, dataVolta, email, nome, telefone, origem, referer, ip, viajantes);

      if (intencao.BlackList())
        return;

      intencao.ValidaIntencaoCompra();

      _intencaoRepository.Cadastra(intencao);
    }

    public void Cadastra(string idDestino, DateTime dataIda, DateTime dataVolta, string email, string origem, string referer, string ip, ICollection<Viajante> viajantes, string emailPagamento, string telefone, string cep, int idPedido, int idPagamento, decimal valor, decimal valorDesconto, string cupomDesconto, string tipoPagamento, bool pago)
    {
      var intencao = new IntencaoPaga(TrataDestino(idDestino), dataIda, dataVolta, email, origem, referer, ip, viajantes, emailPagamento, telefone, cep, idPedido, idPagamento, valor, valorDesconto, cupomDesconto, tipoPagamento, pago);

      if (intencao.BlackList())
        return;

      intencao.ValidaIntencaoPaga();

      _intencaoRepository.Cadastra(intencao);
    }

    public void AtualizaStatusPagamento(int idPagamento)
    {
      _intencaoRepository.AtualizaStatusPagamento(idPagamento);
    }

    public string TrataDestino(string idDestino)
    {
      var destinos = new Dictionary<string, string>
                {
                    {"1", "América do Norte"},
                    {"2", "África"},
                    {"3", "América Central"},
                    {"4", "América do Sul"},
                    {"5", "Ásia"},
                    {"6", "Brasil"},
                    {"7", "Europa"},
                    {"8", "Oceania"},
                    {"9", "Oriente Médio"}
                };

      return destinos[idDestino];
    }
  }
}
