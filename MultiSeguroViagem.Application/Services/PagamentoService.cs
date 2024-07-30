using System;
using MultiSeguroViagem.Application.Model;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Entities.Enum;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using RestSharp;
using static MultiSeguroViagem.Common.Helpers.Funcoes;
using System.Collections.Generic;
using MultiSeguroViagem.Domain.Interfaces.Services.Services;
using System.Globalization;
using MultiSeguroViagem.Common.Validations;

namespace MultiSeguroViagem.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _repo;
        private readonly IItauService _itauService;
        private readonly IRedeCardService _redeCardService;

        public PagamentoService(IPagamentoRepository repo)
        {
            _repo = repo;
        }

        public PagamentoService(IPagamentoRepository repo, IItauService itauService, IRedeCardService redeCardService)
        {
            _repo = repo;
            _itauService = itauService;
            _redeCardService = redeCardService;
        }

        public Comprovante ObtemComprovante(int idPagamento)
        {
            var comprovante = _repo.ObtemComprovante(idPagamento);
            comprovante.Valida();

            return comprovante;
        }

        public PagamentoAvulso BuscaPagamentoAvulso(int idPagamento)
        {
            return _repo.BuscaPagamentoAvulso(idPagamento);
        }

        public Pagamento Busca(int idPagamento)
        {
            return _repo.Busca(idPagamento);
        }

        public Pagamento ObtemPagamentoPedido(int idPedido)
        {
            return _repo.ObtemPagamentoPedido(idPedido);
        }

        public PagamentoItau ObtemPagamentoItau(int idPagamento)
        {
            return _repo.ObtemPagamentoItau(idPagamento);
        }

        public IList<PagamentoItau> ObtemBoletosAbertos()
        {
            return _repo.ObtemBoletosAbertos();
        }

        public Pagamento BuscaPagamentoPago()
        {
            return _repo.BuscaPagamentoPago();
        }


        public PagamentoRedeCard CadastraAutorizacaoCreditoRedeCard(Pagamento pagamento, string codigoRetorno, string dataTransacao, string horaTransacao, string nomeCartao, string numeroCartao, string mensagemRetorno, string numeroAutorizacao, string numeroSequencial, string numeroPedido, long tid, int quantidadeParcelas, string bandeira)
        {
            var data = DateTime.Now;
            var redeCard = new PagamentoRedeCard(pagamento, codigoRetorno, data, nomeCartao, numeroCartao, mensagemRetorno, numeroAutorizacao, numeroSequencial,
                                                 numeroPedido, tid, quantidadeParcelas, bandeira);
            return _repo.CadastraAutorizacaoCreditoRedeCard(redeCard);
        }

        public PagamentoRedeCard BuscaPagamentoRedeCard(int idPagamento)
        {
            return _repo.BuscaPagamentoRedeCard(idPagamento);
        }

        public PagamentoItau CadastraDadosItau(Pagamento pagamento, string valor, string observacao, string nome, string codigoInscricao, string numeroInscricao, string cep, DateTime dataVencimento, string urlRetorno, string obsAdicional1, string obsAdicional2, string obsAdicional3)
        {
            var itau = new PagamentoItau(pagamento, valor, observacao, nome, codigoInscricao, numeroInscricao, cep, dataVencimento, urlRetorno, obsAdicional1, obsAdicional2, obsAdicional3);
            itau.Valida();

            return _repo.CadastraDadosItau(itau);
        }

        public Pagamento Cadastra(Pedido pedido, int status, int operadora, string nome, string email, string documento, string cep, DateTime dataVencimento, decimal valor, decimal valorDescontoCupom, decimal valorDescontoBoleto, decimal valorDescontoTotal)
        {
            var pagamento = new Pagamento(pedido, status, operadora, nome, email, documento, cep, dataVencimento, valor, valorDescontoCupom, valorDescontoBoleto, valorDescontoTotal);

            return _repo.Cadastra(pagamento);
        }

        public void CadastraPagamentoFinanceiro(Pagamento pagamento, int idVendedorResponsavel) => _repo.CadastraPagamentoFinanceiro(pagamento, idVendedorResponsavel);



        public void Atualiza(int idPagamento, int status, int operadora, decimal valorDescontoBoleto, decimal valorDescontoCupom, DateTime? dataPagamento)
        {
            _repo.Atualiza(idPagamento, status, operadora, valorDescontoBoleto, valorDescontoCupom, dataPagamento);
        }

        public void AtualizaPagamentoFinanceiro(int idPagamento, int idFinanceiroResponsavel)
        {
            _repo.AtualizaPagamentoFinanceiro(idPagamento, idFinanceiroResponsavel);
        }

        public void AtualizaAutorizacao(int idPagamento, string codigoRetorno, string mensagem)
        {
            _repo.AtualizaAutorizacao(idPagamento, codigoRetorno, mensagem);
        }

        public void AtualizaStatus(int idPagamento, int status, int operadora, DateTime? dataPagamento)
        {
            AssertionConcern.AssertArgumentNotNull(idPagamento, "O ID do pagamento não pode ser null para atualizar");
            AssertionConcern.AssertArgumentNotNull(status, "O status do pagamento não pode ser null para atualizar");
            AssertionConcern.AssertArgumentNotNull(operadora, "A operadora não pode ser null para atualizar o pagamento");
            //AssertionConcern.AssertArgumentNotNull(dataPagamento, "A data do pagamento não pode ser null para atualizar");

            _repo.AtualizaStatus(idPagamento, status, operadora, dataPagamento);
        }

        public void AtualizaStatus(int idPagamento, int status)
        {
            AssertionConcern.AssertArgumentNotNull(idPagamento, "O ID do pagamento não pode ser null para atualizar");

            AssertionConcern.AssertArgumentNotNull(status, "O status do pagamento não pode ser null para atualizar");

            _repo.AtualizaStatus(idPagamento, status);
        }

        public void AtualizaCancelamentoRedeCard(int idPagamento, string mensageRetorno, string motivoEstorno, DateTime? dataCancelamento)
        {
            AssertionConcern.AssertArgumentNotNull(idPagamento, "O ID do pagamento não pode ser null para atualizar");

            _repo.AtualizaCancelamentoRedeCard(idPagamento, mensageRetorno, motivoEstorno, dataCancelamento);
        }

        public void AtualizaDataConsultaBoleto(int idPagamentoItau, DateTime dataConsultaBoleto)
        {
            _repo.AtualizaDataConsultaBoleto(idPagamentoItau, dataConsultaBoleto);
        }

        public void AtualizaCupom(int idPagamento, string cupom)
        {
            if (!string.IsNullOrEmpty(cupom))
                _repo.AtualizaCupom(idPagamento, cupom);
        }



        public void AlteraFunilRdStation(string email, string identificador, RdStationEstagioLead estagio, bool oportunidade)
        {
            /*var model = new FunilRdStationModel()
            {
              auth_token = identificador,
              lead = new FunilLeadRdStationModel(estagio, oportunidade)
            };

            var client = new RestClient("https://www.rdstation.com.br");
            var request = new RestRequest($"/api/1.2/leads/{email}", Method.PUT);

            request.AddHeader("content-type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

            client.Execute(request);*/
        }

        public void MarcaVendaRdStation(string identificador, decimal total, string email)
        {
            /*var model = new VendaRdStationModel(total, email);

            var client = new RestClient("https://www.rdstation.com.br");
            var request = new RestRequest($"/api/1.2/services/{identificador}/generic", Method.POST);

            request.AddHeader("content-type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

            var x = client.Execute(request);*/
        }

        public bool ValidaVencimento(int idPagamento)
        {
            return Busca(idPagamento).DataVencimento.Date < DateTime.Now.Date;
        }

        public bool ValidaVencimento(DateTime dataVencimento)
        {
            return dataVencimento.Date < DateTime.Now.Date;
        }

        public string ProcessaPagamentoItau(string urlRetorno, string identificador, string observacao, string obsAdicional1, string obsAdicional2,
                                            string obsAdicional3, string codigoEmpresaItau, string codigoItau, string valorTotalPedido, string idPedido,
                                            string nomeCompleto, string cep, string endereco, string bairro, string cidade, string estado, Pagamento pagamento)
        {
            var pagamentoItau = ObtemPagamentoItau(pagamento.IdPagamento) ?? CadastraDadosItau(pagamento,
                                                                                               valorTotalPedido,
                                                                                               observacao,
                                                                                               nomeCompleto,
                                                                                               identificador.Length == 11 ? "01" : "02",
                                                                                               identificador,
                                                                                               cep,
                                                                                               DateTime.Now.Date,
                                                                                               urlRetorno,
                                                                                               obsAdicional1,
                                                                                               obsAdicional2,
                                                                                               obsAdicional3);

            var cripto = _itauService.GeraCripto(pagamentoItau.IdPagamentoItau.ToString().PadLeft(8, '0'),
                                                 codigoEmpresaItau,
                                                 valorTotalPedido,
                                                 observacao,
                                                 codigoItau,
                                                 nomeCompleto,
                                                 (identificador.Length == 11) ? "01" : "02",
                                                 identificador,
                                                 endereco,
                                                 bairro,
                                                 cep,
                                                 cidade,
                                                 estado.Equals("-") ? estado : (estado.Length > 2) ? estado.Substring(2) : estado,
                                                 DateTime.Now.ToString("ddMMyyyy"),
                                                 urlRetorno,
                                                 obsAdicional1,
                                                 obsAdicional2,
                                                 obsAdicional3);

            //      AtualizaStatus(pagamento.IdPagamento, pagamento.Status, pagamento.Operadora, null);

            //      AtualizaCupom(pagamento.IdPagamento, cupom);
            //#if !DEBUG
            //            AlteraFunilRdStation(email, rdStationIdentificador, RdStationEstagioLead.Lead_Qualificado, true);
            //#endif
            return cripto;
        }

        public void ProcessaPagamentoRedeCard(Pagamento pagamento, string dataValidadeCartao, string codigoSegurancaCartao, string numeroCartao, string bandeiraCartao,
                                              string nomeCartao, string parcelas, string numeroPedido, string valorTotalPedido, string filiacaoRede, string senhaRede)

        {
            var retorno = _redeCardService.GetAuthorizedCredit(dataValidadeCartao.Substring(dataValidadeCartao.IndexOf("/") + 1),
                                                               codigoSegurancaCartao,
                                                               filiacaoRede,
                                                               dataValidadeCartao.Substring(0, 2),
                                                               numeroCartao,
                                                               numeroPedido,
                                                               "01",
                                                               parcelas,
                                                               nomeCartao,
                                                               "0",
                                                               senhaRede,
                                                               valorTotalPedido,
                                                               parcelas.Equals("00") ? 04 : 08);

            pagamento.DefineDataPagamento(DateTime.Now);
            pagamento.DefineStatus((int)EnumPagamentoStatus.Pago);
            pagamento.DefineOperadora((int)EnumPagamentoOperadora.RedeCard);

            CadastraAutorizacaoCreditoRedeCard(pagamento, retorno["CodRet"], retorno["Data"], retorno["Hora"], nomeCartao, numeroCartao.Substring(numeroCartao.Length - 4), retorno["Msgret"], retorno["NumAutor"], retorno["NumSqn"], retorno["NumPedido"], (long)Convert.ToDouble(retorno["Tid"]), Convert.ToInt32(parcelas), bandeiraCartao);

            //      AtualizaStatus(pagamento.IdPagamento, pagamento.Status, pagamento.Operadora, (DateTime)pagamento.DataPagamento);

            //      AtualizaCupom(pagamento.IdPagamento, cupom);

            //      AtualizaPagamentoFinanceiro(pagamento.IdPagamento, idVendedor);

            //      var cultura = CultureInfo.InvariantCulture;

            //#if !DEBUG
            //            MarcaVendaRdStation(rdStationIdentificador, Decimal.Parse(valorTotalPedido, cultura), emailUsuario);
            //#endif

        }


        public void ProcessaCancelamentoRedeCard(Pagamento pagamento, string filiacaoRede, string senhaRede, string motivoCancelamento)
        {
            var pagamentoRedeCard = BuscaPagamentoRedeCard(pagamento.IdPagamento);

            pagamentoRedeCard.DefineMotivoEstorno(motivoCancelamento);

            try
            {
                var retorno = _redeCardService.Cancelamento(filiacaoRede, senhaRede, pagamentoRedeCard.DataTransacao.ToString("yyyyMMdd"), pagamentoRedeCard.NumeroAutorizacao, pagamentoRedeCard.NumeroSequencial, pagamentoRedeCard.Tid.ToString());

                pagamento.DefineStatus((int)EnumPagamentoStatus.Estornado);
                pagamentoRedeCard.DefineDataCancelamento(DateTime.Now);

                AtualizaStatus(pagamento.IdPagamento, pagamento.Status);
            }
            catch (Exception ex)
            { pagamentoRedeCard.DefineMensagemRetorno(ex.Message); }
            AtualizaCancelamentoRedeCard(pagamento.IdPagamento, pagamentoRedeCard.MensagemRetorno, pagamentoRedeCard.MotivoEstorno, pagamentoRedeCard?.DataCancelamento);
        }

    }
}
