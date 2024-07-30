using System;
using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using MultiIntegra.Common.Models.Affinity;
using MultiIntegra.Common.Models.April;
using MultiIntegra.Common.Models.AssistCard;
using MultiIntegra.Common.Models.AssistMed;
using MultiIntegra.Common.Models.Gta;
using MultiIntegra.Common.Models.Intermac;
using MultiSeguroViagem.Application.Model;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Entities.Enum;
using RestSharp;
using MultiIntegra.Common.Models.TravelAce;
using MultiIntegra.Common.Models.VitalCard;
using MultiSeguroViagem.Domain.Interfaces.Repositories;

namespace MultiSeguroViagem.Application.Services
{
  public class EmissaoVoucherService : IEmissaoVoucherService
  {
    private readonly ITokenService _tokenService;
    private readonly IPedidoService _pedidoService;
    private readonly IEmailAgendadoService _emailAgendado;
    private readonly IPlanoService _planoService;
    private readonly IDiasUteisService _diasUteisService;
    private readonly IPagamentoService _pagamentoService;
    private readonly ILogRepository _logRepository;


    public EmissaoVoucherService(ITokenService tokenService, IPedidoService pedidoService, IEmailAgendadoService emailAgendado, IPlanoService planoService, IDiasUteisService diasUteisService, IPagamentoService pagamentoService, ILogRepository logRepository)
    {
      _tokenService = tokenService;
      _pedidoService = pedidoService;
      _emailAgendado = emailAgendado;
      _planoService = planoService;
      _diasUteisService = diasUteisService;
      _pagamentoService = pagamentoService;
      _logRepository = logRepository;
    }

    public void EmissaoVoucher(Pagamento pagamento, IList<Viajante> viajantes, ContatoEmergencia contatoEmergencia)
    {
      string json = string.Empty, nomeOperadora = string.Empty;

      try
      {
        nomeOperadora = pagamento.Pedido.Itens.FirstOrDefault().Plano.Operadora.Nome.RemoveAcentuacao().RemoveEspacos().ToLower();

        switch (nomeOperadora)
        {
          case "affinity":
            {
              var viajantesModel = viajantes.Select(viajante => new AffinityViajantesModel
              {
                Nome = viajante.Nome.Replace("'"," "),
                Sexo = viajante.Sexo == 1 ? "M" : "F",
                DataNascimento = viajante.DataNascimento,
                TipoDocumento = "CPF",
                NumeroDocumento = viajante.Cpf,
                Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                Numero = pagamento.Pedido.Usuario.Numero,
                Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                Complemento = pagamento.Pedido.Usuario.Complemento,
                Cep = pagamento.Pedido.Usuario.Cep,
                Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                Uf = pagamento.Pedido.Usuario.Estado,
                Email = pagamento.Email,
                Telefone = pagamento.Pedido.Usuario.Telefone.Length == 10 ? pagamento.Pedido.Usuario.Telefone : string.Empty,
                Celular = pagamento.Pedido.Usuario.Telefone.Length == 11 ? pagamento.Pedido.Usuario.Telefone : string.Empty
              }).ToList();

              var model = new AffinityEmissaoModel
              {
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "gta":
            {
              var viajantesModel = viajantes.Select(viajante => new GtaViajantesModel
              {
                Nome = viajante.Nome.Replace("'", " "),
                DataNascimento = viajante.DataNascimento,
                TipoDocumento = "CPF",
                NumeroDocumento = viajante.Cpf,
                Cep = pagamento.Pedido.Usuario.Cep,
                Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                Numero = pagamento.Pedido.Usuario.Numero,
                Complemento = pagamento.Pedido.Usuario.Complemento,
                Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                Uf = pagamento.Pedido.Usuario.Estado,
                Telefone = pagamento.Pedido.Usuario.Telefone.Length == 10 ? pagamento.Pedido.Usuario.Telefone : string.Empty,
                Email = pagamento.Email
              }).ToList();

              var model = new GtaEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "intermac":
            {
              var viajantesModel = viajantes.Select(viajante => new IntermacViajantesModel
              {
                Nome = viajante.Nome.Replace("'", " "),
                Sexo = viajante.Sexo == 1 ? "M" : "F",
                DataNascimento = viajante.DataNascimento,
                TipoDocumento = "CPF",
                NumeroDocumento = viajante.Cpf,
                Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                Numero = pagamento.Pedido.Usuario.Numero,
                Cep = pagamento.Pedido.Usuario.Cep,
                Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                Complemento = pagamento.Pedido.Usuario.Complemento,
                Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                Uf = pagamento.Pedido.Usuario.Estado,
                Email = pagamento.Email,
                Telefone = pagamento.Pedido.Usuario.Telefone.Length == 10 ? pagamento.Pedido.Usuario.Telefone : string.Empty,
                Celular = pagamento.Pedido.Usuario.Telefone.Length == 11 ? pagamento.Pedido.Usuario.Telefone : string.Empty
              }).ToList();

              var model = new IntermacEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "assistseguroviagem":
            {
              var viajantesModel = viajantes.Select(viajante => new AssistMedViajantesModel
              {
                Nome = viajante.Nome.Replace("'", " "),
                Sexo = viajante.Sexo == 1 ? "M" : "F",
                DataNascimento = viajante.DataNascimento,
                TipoDocumento = "CPF",
                NumeroDocumento = viajante.Cpf,
                Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                Numero = pagamento.Pedido.Usuario.Numero,
                Cep = pagamento.Pedido.Usuario.Cep,
                Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                Complemento = pagamento.Pedido.Usuario.Complemento,
                Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                Uf = pagamento.Pedido.Usuario.Estado,
                Email = pagamento.Email
              }).ToList();

              var model = new AssistMedEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "assistmed":
            {
              var viajantesModel = viajantes.Select(viajante => new AssistMedViajantesModel
              {
                Nome = viajante.Nome.Replace("'", " "),
                Sexo = viajante.Sexo == 1 ? "M" : "F",
                DataNascimento = viajante.DataNascimento,
                TipoDocumento = "CPF",
                NumeroDocumento = viajante.Cpf,
                Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                Numero = pagamento.Pedido.Usuario.Numero,
                Cep = pagamento.Pedido.Usuario.Cep,
                Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                Complemento = pagamento.Pedido.Usuario.Complemento,
                Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                Uf = pagamento.Pedido.Usuario.Estado,
                Email = pagamento.Email
              }).ToList();

              var model = new AssistMedEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, "assistseguroviagem", json);
            }
            break;

            /*

          case "assistseguroviagem":
            {
              var viajantesModel = viajantes.Select(viajante => new AssistMedViajantesModel
              {
                Nome = viajante.Nome.Replace("'", " "),
                Sexo = viajante.Sexo == 1 ? "M" : "F",
                DataNascimento = viajante.DataNascimento,
                TipoDocumento = "CPF",
                NumeroDocumento = viajante.Cpf,
                Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                Numero = pagamento.Pedido.Usuario.Numero,
                Cep = pagamento.Pedido.Usuario.Cep,
                Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                Complemento = pagamento.Pedido.Usuario.Complemento,
                Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                Uf = pagamento.Pedido.Usuario.Estado,
                Email = pagamento.Email
              }).ToList();

              var model = new AssistMedEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, "assistseguroviagem", json);
            }
            break;
            */

          case "assistcard":
            {
              var viajantesModel = viajantes.Select(viajante =>
              {
                var nomeArray = viajante.Nome.Split(' ');

                return new AssistCardViajanteModel
                {
                  Nome = nomeArray[0].Replace("'", " "),
                  Sobrenome = string.Join(" ", nomeArray.Skip(1)).Replace("'", " "),
                  DataNascimento = viajante.DataNascimento,
                  TipoDocumento = "CPF",
                  NumeroDocumento = viajante.Cpf,
                  Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                  Cep = pagamento.Pedido.Usuario.Cep,
                  Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                  Uf = pagamento.Pedido.Usuario.Estado,
                  Email = pagamento.Email,
                  Telefone = pagamento.Pedido.Usuario.Telefone,
                  PaisDomicilio = string.Empty,
                  ContatoEmergenciaNome = contatoEmergencia.Nome.Replace("'", " "),
                  ContatoEmergenciaTelefone = contatoEmergencia.Telefone
                };

              }).ToList();
              /*

              var codigo = _planoService.DefineCodigoTarifaAssisCard(pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlano,
                                                                     pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                                                                     pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova);
                                                                     */
              var model = new AssistCardEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                CodigoTarifa = pagamento.Pedido.Itens.FirstOrDefault().Plano.CodigoTarifaExterno,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Moeda = "R$",
                Valor = pagamento.Valor.ToString().Replace(",", "."),
                Markup = "50.00",
                Desconto = "00.00",
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "april":
            {
              var viajantesModel = viajantes.Select(viajante =>
              {
                var nomeArray = viajante.Nome.Split(' ');

                return new AprilViajanteModel
                {
                  Nome = nomeArray[0].Replace("'", " "),
                  Sobrenome = string.Join(" ", nomeArray.Skip(1)).Replace("'", " "),
                  Sexo = viajante.Sexo == 1 ? "M" : "F",
                  DataNascimento = viajante.DataNascimento,
                  TipoDocumento = "CPF",
                  NumeroDocumento = viajante.Cpf,
                  Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                  Cep = pagamento.Pedido.Usuario.Cep,
                  Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                  Uf = pagamento.Pedido.Usuario.Estado,
                  Telefone = pagamento.Pedido.Usuario.Telefone,
                  Email = pagamento.Email,
                  ContatoEmergenciaNome = contatoEmergencia.Nome.Replace("'", " "),
                  ContatoEmergenciaTelefone = contatoEmergencia.Telefone,
                  ContatoEmergenciaEndereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " ")
                };

              }).ToList();

              var model = new AprilEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Familiar = false,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "travelace":
            {
              var viajantesModel = viajantes.Select(viajante =>
              {
                var nomeArray = viajante.Nome.Split(' ');
                var tipoDocumento = "CPF";
                if (viajante.Cpf.Length < 11) {
                  tipoDocumento = "PAS";
                }
                return new TravelAceViajantesModel
                {
                  Nome = nomeArray[0].Replace("'", " "),
                  Sobrenome = string.Join(" ", nomeArray.Skip(1)).Replace("'", " "),
                  Sexo = viajante.Sexo == 1 ? "M" : "F",
                  DataNascimento = viajante.DataNascimento,
                  TipoDocumento = tipoDocumento,
                  NumeroDocumento = viajante.Cpf,
                  Endereco = pagamento.Pedido.Usuario.Endereco.Replace("'", " "),
                  Numero = pagamento.Pedido.Usuario.Numero,
                  Cep = pagamento.Pedido.Usuario.Cep,
                  Bairro = pagamento.Pedido.Usuario.Bairro.Replace("'", " "),
                  Complemento = pagamento.Pedido.Usuario.Complemento,
                  Cidade = pagamento.Pedido.Usuario.Cidade.Replace("'", " "),
                  Uf = pagamento.Pedido.Usuario.Estado,
                  Email = pagamento.Email,
                  Telefone = pagamento.Pedido.Usuario.Telefone.Length == 10 ? pagamento.Pedido.Usuario.Telefone : string.Empty,
                  Celular = pagamento.Pedido.Usuario.Telefone.Length == 11 ? pagamento.Pedido.Usuario.Telefone : string.Empty,
                  ContatoEmergenciaNome = contatoEmergencia.Nome.Replace("'", " "),
                  ContatoEmergenciaTelefone = contatoEmergencia.Telefone,
                  ContatoEmergenciaCelular = contatoEmergencia.Telefone.Length == 11 ? contatoEmergencia.Telefone : string.Empty
                };

              }).ToList();

              var codigoTarifa = pagamento.Pedido.Itens.FirstOrDefault().Plano.CodigoTarifaExterno;

              var model = new TravelAceEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                CodigoTarifa = codigoTarifa ?? "",
                Valor = pagamento.Valor.ToString().Replace(",", "."),
                TipoTarifa = string.IsNullOrEmpty(codigoTarifa) ? "Folheto" : "Acordo",
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;

          case "vitalcard":
            {
              var viajantesModel = viajantes.Select(viajante =>
              {
                var nomeArray = viajante.Nome.Split(' ');

                return new VitalCardViajanteModel
                {
                  Nome = nomeArray[0].Replace("'", " "),
                  Sobrenome = string.Join(" ", nomeArray.Skip(1)).Replace("'", " "),
                  Sexo = viajante.Sexo == 1 ? "M" : "F",
                  DataNascimento = viajante.DataNascimento,
                  Numero = pagamento.Pedido.Usuario.Numero,
                  Cep = pagamento.Pedido.Usuario.Cep,
                  Email = pagamento.Email,
                  Telefone = pagamento.Pedido.Usuario.Telefone,
                  Cpf = viajante.Cpf,
                  Rg = string.Empty
                };

              }).ToList();

              var model = new VitalCardEmissaoModel
              {
                IdPlano = pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlanoExterno,
                DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda,
                DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova,
                Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome,
                Viajantes = viajantesModel
              };

              if (string.IsNullOrEmpty(model.IdPlano))
                throw new ArgumentException("O Id do plano está nulo ou vazio");

              json = new JavaScriptSerializer().Serialize(model);

              EmissaoRequest(pagamento, viajantes, nomeOperadora, json);
            }
            break;
        }
      }
      catch (Exception ex)
      {
        _pedidoService.AtualizaStatus((int)EnumPedidoStatus.EmitirVoucherManualmente, pagamento.Pedido.IdPedido);
        if (pagamento.Pedido.BitTentativa >= 7)
        {
          VerificaPagamentoDiaUtil(pagamento);
        }

        _logRepository.FalhaEmissaoVoucher(pagamento, viajantes, nomeOperadora, json, ex.Message);
      }
    }

    public void VerificaPagamentoDiaUtil(Pagamento pagamento)
    {
      try
      {
        if ((int)EnumPagamentoOperadora.RedeCard != pagamento.Operadora)
          return;

        var diasUteisSemana = _diasUteisService.BuscaDiasSemana();
        var diasUteisExcecoes = _diasUteisService.BuscaDiasExcecoes();

        var existeDiaUtil = pagamento.ValidaDiaUtilEntreCompraEmbarque(pagamento.DataCriacao, pagamento.Pedido.Itens.FirstOrDefault().DataIda, diasUteisSemana.ToList(), diasUteisExcecoes.ToList());

        if (existeDiaUtil)
          return;

        _pagamentoService.ProcessaCancelamentoRedeCard(pagamento,
                                                       System.Configuration.ConfigurationManager.AppSettings[Constantes.CHAVE_FILIACAO_REDE],
                                                       System.Configuration.ConfigurationManager.AppSettings[Constantes.CHAVE_SENHA_REDE],
                                                       string.Empty);

        if (pagamento.Status == (int)EnumPagamentoStatus.Estornado)
        {
          _pedidoService.AtualizaStatus((int)EnumPedidoStatus.PedidoCancelado, pagamento.Pedido.IdPedido);

          _emailAgendado.EmailEstornoRedeCard(Constantes.EMAIL_REMETENTE,
                                              Constantes.EMAIL_NOME_REMETENTE,
                                              $"{pagamento.Email};{System.Configuration.ConfigurationManager.AppSettings["emailsAlertas"]}",
                                              pagamento.Pedido.IdPedido.ToString(),
                                              pagamento.Nome,
                                              pagamento.Pedido.Itens.FirstOrDefault().Plano.Operadora.Nome,
                                              pagamento.Pedido.ValorTotal.ToString(),
                                              true);
        }
        else
        {
          _emailAgendado.EmailEstornoRedeCard(Constantes.EMAIL_REMETENTE,
                                              Constantes.EMAIL_NOME_REMETENTE,
                                              System.Configuration.ConfigurationManager.AppSettings["emailsAlertas"],
                                              pagamento.Pedido.IdPedido.ToString(),
                                              pagamento.Nome,
                                              pagamento.Pedido.Itens.FirstOrDefault().Plano.Operadora.Nome,
                                              pagamento.Pedido.ValorTotal.ToString(),
                                              false);
        }
      }
      catch (Exception ex)
      {
        _logRepository.FalhaEmissaoVoucher(pagamento, null, $"IdOperadora: {pagamento.Operadora}", "Método: VerificaPagamentoDiaUtil", ex.Message);
      }
    }


    private void EmissaoRequest(Pagamento pagamento, IList<Viajante> viajantes, string nomeOperadora, string json)
    {
      while (true)
      {
        var token = _tokenService.BuscaToken(Constantes.API_MULTIINTEGRA_NOME);

        if (string.IsNullOrEmpty(token))
          throw new InvalidOperationException("Não foi possível gerar o token na API MultiIntegra");

        var url = $"{System.Configuration.ConfigurationManager.AppSettings["uriApiMultiIntegra"]}/voucher/{nomeOperadora}";

        var client = new RestClient(url);
        var request = new RestRequest(Method.POST);
        request.AddHeader("authorization", $"Bearer {token}");
        request.AddHeader("content-type", "application/json");
        request.AddParameter("application/json", json, ParameterType.RequestBody);

        var response = client.Execute(request);

        switch (response.StatusCode)
        {
          case HttpStatusCode.OK:
            {
              _pedidoService.AtualizaStatus((int)EnumPedidoStatus.VoucherEmitidoAutomaticamente, pagamento.Pedido.IdPedido);

              var vouchersModel = new JavaScriptSerializer().Deserialize<ICollection<VoucherModel>>(response.Content);

              if (vouchersModel != null && vouchersModel.Count > 0)
              {
                foreach (var voucher in vouchersModel)
                {
                  var viajante = viajantes.FirstOrDefault(x => x.Nome.RemoveAcentuacao().RemoveEspacos().ToLower() == voucher.NomeViajante.RemoveEspacos().ToLower());

                  if (viajante == null)
                    continue;

                  viajante.AtualizaIdVoucher(voucher.IdVoucher);
                  viajante.AtualizaCodigoBilhete(voucher.CodigoBilhete);
                  viajante.AtualizaVoucher(voucher.CodigoVoucher);
                  viajante.AtualizaUriVoucher(voucher.UriVoucher);
                  viajante.AtualizaUriCondicaoGeral(voucher.UriCondicaoGeral);
                }

                _pedidoService.AtualizaViajante(viajantes);

                var emails = $"{pagamento.Email};{System.Configuration.ConfigurationManager.AppSettings["emailsVoucher"]}";

                _emailAgendado.EmailVoucher(Constantes.EMAIL_REMETENTE, Constantes.EMAIL_NOME_REMETENTE, emails, pagamento, viajantes);
              }
              else
              {
                _logRepository.FalhaEmissaoVoucher(pagamento, viajantes, nomeOperadora, response.Content, "Voucher emitido na operadora, mas não foi possível serializar o retorno da MultiIntegra (coluna JsonRequest) e iterar sobre os viajantes");
              }

              break;
            }

          case HttpStatusCode.Unauthorized:
            {
              _tokenService.AtualizaToken();

              continue;
            }

          default:
            throw new InvalidOperationException(response.Content);
        }

        break;
      }
    }
  }
}
