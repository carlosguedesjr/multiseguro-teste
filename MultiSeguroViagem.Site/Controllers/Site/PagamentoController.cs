using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using MultiSeguroViagem.Common.Validations;
using static MultiSeguroViagem.Common.Helpers.Funcoes;
using static MultiSeguroViagem.Common.Helpers.Constantes;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Entities.Enum;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Models.Site;
using MultiSeguroViagem.Site.Models.Site.Pagamento;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using System.Transactions;
using System.Net;
using System.Web.Script.Serialization;

namespace MultiSeguroViagem.Site.Controllers.Site
{
    public class PagamentoController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IContatoEmergenciaService _contatoEmergenciaService;
        private readonly IPedidoService _pedidoService;
        private readonly IPagamentoService _pagamentoService;
        private readonly IEmailAgendadoService _emailAgendadoService;
        private readonly IIntencaoService _intencaoService;
        private readonly IBlackListFraudeRepository _blacklistfraudeRepository;

        public PagamentoController(IUsuarioService usuarioService, IContatoEmergenciaService contatoEmergenciaService, IPedidoService pedidoService, IPagamentoService pagamentoService, IEmailAgendadoService emailAgendadoService, IIntencaoService intencaoService, IBlackListFraudeRepository blacklistfraudeRepository)
        {
            _usuarioService = usuarioService;
            _contatoEmergenciaService = contatoEmergenciaService;
            _pedidoService = pedidoService;
            _pagamentoService = pagamentoService;
            _emailAgendadoService = emailAgendadoService;
            _intencaoService = intencaoService;
            _blacklistfraudeRepository = blacklistfraudeRepository;
        }

        #region Actions

        public ActionResult Index([Bind(Prefix = "d")] string dataIda)
        {

            var hostName = Dns.GetHostName(); // Retrive the Name of HOST  

            // Get the IP Local  
            var myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            DefineTipoPagamento("3,1", Convert.ToDateTime(dataIda));
            ViewBag.host = hostName;
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Avulso(PagamentoAvulsoModel model)
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            model.IdPagamento = int.Parse(Decrypt(Request.QueryString["i"]));
            model.IdPedido = model.p != null ? int.Parse(Decrypt(model.p)) : 0;

            var pagamento = BuscaPagamento(model);
            var pedidoItem = BuscaPedidoItem(model, pagamento);
            var dataIda = pedidoItem.DataIda;

            DefineTipoPagamento(pagamento.OperadorasAceitas, dataIda);

            ViewBag.Pagamento = pagamento;
            ViewBag.PedidoItem = pedidoItem;
            ViewBag.PagamentoVencido = _pagamentoService.ValidaVencimento(pagamento.DataVencimento);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult AvulsoSistema(PagamentoAvulsoModel model)
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            model.IdPagamento = int.Parse(Decrypt(Request.QueryString["i"]));

            var pagamento = BuscaPagamento(model);
            ViewBag.Pagamento = pagamento;

            DefineTipoPagamento(pagamento.OperadorasAceitas, null);

            ViewBag.PagamentoVencido = _pagamentoService.ValidaVencimento(pagamento.DataVencimento);

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult FinalizarCompra(PagamentoModel model)
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("erroCamposVazio", "Verifique os erros do formulário");
                return View("Index", model);
            }

            var culture = new CultureInfo("en-US");
            var jss = new JavaScriptSerializer();

            try
            {
                using (var scope = new TransactionScope())
                {
                    #region Validações

                    AssertionConcern.AssertArgumentLessThen(Convert.ToDecimal(model.ValorTotal, culture), 1, "Ocorreu um problema ao calcular o valor total do pagamento");

                    AssertionConcern.AssertArgumentNotEmpty(model.NomeCompleto, "O Nome Completo não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Email, "O Email não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.TelefonePessoal, "O Telefone Pessoal não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Documento, "O Documento não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Cep, "O Cep não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Endereco, "O Endereco não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Numero, "O Numero não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Bairro, "O Bairro não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Cidade, "O Cidade não pode ser nulo para finalizar a compra");
                    AssertionConcern.AssertArgumentNotEmpty(model.Estado, "O Estado não pode ser nulo para finalizar a compra");

                    #endregion
                    if (model.Email == "ju@gmail.com")
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    var carrinho = jss.Deserialize<List<CarrinhoModel>>(model.Carrinho);
                    var senha = TextoRandom(10);
                    var novoUsuario = false;

                    var usuario = _usuarioService.RegistraUsuarioCheckout(model.NomeCompleto,
                                                                          model.Email,
                                                                          senha,
                                                                          model.TelefonePessoal.ToNumbers(),
                                                                          model.Documento.ToNumbers(),
                                                                          model.Cep.ToNumbers(),
                                                                          model.Endereco,
                                                                          model.Numero,
                                                                          model.Complemento,
                                                                          model.Bairro,
                                                                          model.Cidade,
                                                                          model.Estado.Length > 2 ? model.Estado.Substring(2) : model.Estado,
                                                                          out novoUsuario);
                    TempData["NovoUsuario"] = novoUsuario;

                    var valorTotal = Convert.ToDecimal(model.ValorTotal, culture);
                    var valorDescontoCupom = Convert.ToDecimal(model.DescontoCupom, culture);
                    var valorDescontoBoleto = Convert.ToDecimal(model.DescontoBoleto, culture);
                    var valorDescontoTotal = Convert.ToDecimal(model.DescontoTotal, culture);
                    var valorTotalPedido = Convert.ToDecimal(model.ValorTotal, culture) - Convert.ToDecimal(model.DescontoTotal, culture);
                    var teste = model.DataNovaVolta;
                    var pedido = _pedidoService.Cadastra(ipAddress, usuario, (int)EnumPedidoStatus.AguardandoPagamento, valorTotalPedido);

                    var descricaoPlanos = new List<string>();

                    // Salva itens do pedido
                    foreach (var dados in carrinho)
                    {
                        DefineMetodosPagamento(dados.DataIda);
                        var planos = Mapper.Map<PlanoModel, Plano>(dados.Planos);
                        var destino = Mapper.Map<DestinoModel, Destino>(dados.Planos.Destino);
                        var nova_data = Convert.ToDateTime(dados.DataVolta);
                        var valorNet = dados.Planos.Preco;
                        var DescontoPlanoDia = dados.Planos.Desconto;
                        var AjustePorcentagem = dados.Planos.AjustePorcentagem;
                        var cambio = dados.Planos.Operadora.ValorDolar;
                        if (dados.Dias < dados.Planos.CotacaoDiasMinimo && dados.Planos.IdStatusMinDias == 1)
                        {
                            var dias_acrescenta = dados.Planos.CotacaoDiasMinimo - dados.Dias;
                            nova_data = nova_data.AddDays(dias_acrescenta);
                        }
                        var item = _pedidoService.CadastraItem(pedido, planos, Convert.ToDateTime(dados.DataIda), Convert.ToDateTime(dados.DataVolta), nova_data, destino, dados.Dias, dados.ValorTotal, dados.QuantidadeViajantes, valorDescontoBoleto, valorDescontoCupom, valorNet, AjustePorcentagem, cambio, DescontoPlanoDia);


                        // Adiciona descrição (Operadora + Plano)
                        descricaoPlanos.Add(string.Concat(planos.Operadora.Nome, "-", planos.Nome));

                        // Cadastra viajantes
                        var viajantes = new List<Viajante>();
                        foreach (var dadoViajante in dados.Viajantes)
                        {
                            var sexo = (dadoViajante.Sexo.Equals("M")) ? (int)EnumSexo.Masculino : (int)EnumSexo.Feminino;
                            var cpf = Regex.Replace(dadoViajante.Cpf, "[^0-9]+", "");
                            var viajante = new Viajante(item, Convert.ToDecimal(dadoViajante.ValorUnitario, culture), dadoViajante.Nome, cpf, Convert.ToDateTime(dadoViajante.DataNascimento), sexo, dadoViajante.Condicao);

                            viajantes.Add(viajante);
                        }

                        _pedidoService.CadastraViajante(viajantes);
                    }

                    var operadoraPagamento = Convert.ToInt32(model.OperadoraPagamento);

                    var pagamento = _pagamentoService.Cadastra(pedido, (int)EnumPagamentoStatus.Aberto, operadoraPagamento, model.NomeCompleto, model.Email, model.Documento.ToNumbers(), model.Cep.ToNumbers(), DateTime.Now, valorTotal, valorDescontoCupom, valorDescontoBoleto, valorDescontoTotal);

                    _pagamentoService.CadastraPagamentoFinanceiro(pagamento, VENDEDOR_PADRAO);

                    var numPedido = pedido.IdPedido.ToString().PadLeft(8, '0');
                    var nome = model.NomeCompleto.Split(' ');
                    var baseUrl = Request.Url.Authority;
                    var identificador = model.Documento.ToNumbers();

                    switch (operadoraPagamento)
                    {
                        case (int)EnumPagamentoOperadora.Itau:

                            var cripto = _pagamentoService.ProcessaPagamentoItau($"{baseUrl}/Pagamento/Voucher?P={pedido.IdPedido}&T=B&C=S",
                                                                                 identificador,
                                                                                 "3",
                                                                                 OBSERVACAO_ADICIONAL1_ITAU,
                                                                                 OBSERVACAO_ADICIONAL2_ITAU,
                                                                                 OBSERVACAO_ADICIONAL3_ITAU,
                                                                                 ObtemValorConfig(CHAVE_CODEMPRESA_ITAU),
                                                                                 ObtemValorConfig(CHAVE_CODITAU),
                                                                                 pedido.ValorTotal.ToString("0.00"),
                                                                                 pedido.IdPedido.ToString(),
                                                                                 model.NomeCompleto,
                                                                                 model.Cep.ToNumbers(),
                                                                                 model.Endereco,
                                                                                 model.Bairro,
                                                                                 model.Cidade,
                                                                                 model.Estado,
                                                                                 pagamento);


                            _pagamentoService.AtualizaStatus(pagamento.IdPagamento, pagamento.Status, pagamento.Operadora, null);

                            _pagamentoService.AtualizaCupom(pagamento.IdPagamento, model.Cupom);

#if !DEBUG
                            _pagamentoService.AlteraFunilRdStation(model.Email, ObtemValorConfig("rdStationIdentificador"), RdStationEstagioLead.Lead_Qualificado, true);
#endif

                            _contatoEmergenciaService.Cadastra(usuario, model.NomeContato, model.TelefoneContato.ToNumbers());

                            EnviaEmail(nome[0], pedido.IdPedido.ToString(), carrinho.FirstOrDefault(), descricaoPlanos, novoUsuario, model.Email, senha, pedido.Status.IdPedidoStatus.ToString());

                            TempData["CriptoItau"] = cripto;

                            CadastraIntencao(carrinho, model, pagamento);
                            scope.Complete();
                            return RedirectToAction("Voucher", new { P = pedido.IdPedido.ToString(), T = "B" });

                        case (int)EnumPagamentoOperadora.RedeCard:

                            _pagamentoService.ProcessaPagamentoRedeCard(pagamento,
                                                                        model.MesAno,
                                                                        model.CodigoSeguranca,
                                                                        Regex.Replace(model.NumeroCartao, @"\s", ""),
                                                                        model.Bandeira,
                                                                        model.NomeCartao,
                                                                        model.Parcelas,
                                                                        numPedido,
                                                                        pedido.ValorTotal.ToString("0.00", culture),
                                                                        ObtemValorConfig(CHAVE_FILIACAO_REDE),
                                                                        ObtemValorConfig(CHAVE_SENHA_REDE));

                            _pedidoService.AtualizaStatus((int)EnumPedidoStatus.PagamentoConfirmado, pedido.IdPedido);

                            _contatoEmergenciaService.Cadastra(usuario, model.NomeContato, model.TelefoneContato.ToNumbers());

                            EnviaEmail(nome[0], pedido.IdPedido.ToString(), carrinho.FirstOrDefault(), descricaoPlanos, novoUsuario, model.Email, senha, pedido.Status.IdPedidoStatus.ToString());

                            CadastraIntencao(carrinho, model, pagamento);


                            _pagamentoService.AtualizaStatus(pagamento.IdPagamento, pagamento.Status, pagamento.Operadora, (DateTime)pagamento.DataPagamento);

                            _pagamentoService.AtualizaCupom(pagamento.IdPagamento, model.Cupom);

                            _pagamentoService.AtualizaPagamentoFinanceiro(pagamento.IdPagamento, VENDEDOR_PADRAO);

#if !DEBUG
                            _pagamentoService.MarcaVendaRdStation(ObtemValorConfig("rdStationIdentificador"), valorTotalPedido, model.Email);
#endif

                            break;
                    }
                    scope.Complete();
                    return RedirectToAction("Voucher", "Pagamento", new { P = pedido.IdPedido.ToString(), T = "C", S = (int)EnumPagamentoStatus.Pago });
                }
            }

            catch (InvalidOperationException ex)
            {
                ViewBag.Erro = ex.Message;
                ViewBag.DetalhesErro = ex.Message;
            }

            catch (Exception ex)
            {
                ViewBag.Erro = "Não foi possível efetuar o pagamento! Selecione outro método de pagamento ou entre em contato conosco.";
                ViewBag.DetalhesErro = ex.Message;
            }

            DefineMetodosPagamento(jss.Deserialize<List<CarrinhoModel>>(model.Carrinho).FirstOrDefault().DataIda);

            return View("Index", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult FinalizarCompraAvulso(PagamentoAvulsoModel model)
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            string id = Encrypt(model.IdPagamento.ToString());
            string Erro = "";
            try
            {
                ValidacoesCamposObrigatorios(model);

                var tipoPagamento = RealizaPagamento(model);

                return RedirectToAction("VoucherAvulso", "Pagamento", new { Nome = model.NomeCompleto.Split(' ')[0], T = tipoPagamento });
            }


            catch (Exception ex)
            {
                if (model.IdPedido > 0)
                {
                    DefineTipoPagamento("1,3", Convert.ToDateTime(model.DataIda));
                    return View("Avulso", model);
                }

                DefineTipoPagamento(model.IdOperadorasPagamento, null);
                ViewBag.Erro = "Ocorreu um problema ao efetuar o pagamento!";
                Erro = "Ocorreu um problema ao efetuar o pagamento!";
                ViewBag.DetalhesErro = ex.Message;
                return RedirectToAction("AvulsoSistema", "Pagamento", new { i = id, e = Erro });
            }

        }

        public ActionResult Voucher()
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            return View();
        }

        public ActionResult VoucherAvulso()
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            return View();
        }

        public ActionResult Comprovante(string id)
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            var model = _pagamentoService.ObtemComprovante(Int32.Parse(Decrypt(id)));

            return View(model);
        }

        #endregion

        #region Métodos
        [ValidateInput(false)]
        private string RealizaPagamento(PagamentoAvulsoModel model)
        {
            //BLOQUEIO DO FRAUDADOR
            var blacklistfraude = _blacklistfraudeRepository.BuscaIp();
            var ipAddress = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null &&
                     System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }


            foreach (dynamic result in blacklistfraude)
            {
                if (result.strIp == ipAddress)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            var culture = new CultureInfo("en-US");

            var valorTotalBruto = Convert.ToDecimal(model.ValorTotal, culture);
            var valorDescontoCupom = Convert.ToDecimal(model.ValorDescontoCupom, culture);
            var valorDescontoBoleto = Convert.ToDecimal(model.ValorDescontoBoleto, culture);
            var valorTotalLiquido = valorTotalBruto - (valorDescontoCupom + valorDescontoBoleto);

            var operadoraPagamento = Convert.ToInt32(model.OperadoraPagamento);

            var pagamento = _pagamentoService.Busca(model.IdPagamento);
            pagamento.DefineOperadora(operadoraPagamento);

            var nome = model.NomeCompleto.Split(' ');

            switch (operadoraPagamento)
            {
                case (int)EnumPagamentoOperadora.Itau:
                    {
                        var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority).Replace("https://", "").Replace("http://", "");
                        var identificador = model.Documento.ToNumbers();

                        var cripto = _pagamentoService.ProcessaPagamentoItau($"{baseUrl}/Pagamento/VoucherAvulso?Nome={nome[0]}&T=B&C=S", // urlRetorno
                                                                             identificador,
                                                                             "3", // observacao
                                                                             OBSERVACAO_ADICIONAL1_ITAU,
                                                                             OBSERVACAO_ADICIONAL2_ITAU,
                                                                             OBSERVACAO_ADICIONAL3_ITAU,
                                                                             ObtemValorConfig(CHAVE_CODEMPRESA_ITAU),
                                                                             ObtemValorConfig(CHAVE_CODITAU),
                                                                             valorTotalLiquido.ToString("0.00"),
                                                                             "", // idPedido
                                                                             model.NomeCompleto,
                                                                             model.Cep.ToNumbers(),
                                                                             !string.IsNullOrEmpty(model.Endereco) ? model.Endereco : "-",
                                                                             !string.IsNullOrEmpty(model.Bairro) ? model.Bairro : "-",
                                                                             !string.IsNullOrEmpty(model.Cidade) ? model.Cidade : "-",
                                                                             !string.IsNullOrEmpty(model.Estado) ? model.Estado : "-",
                                                                             pagamento);

                        _pagamentoService.Atualiza(pagamento.IdPagamento, pagamento.Status, pagamento.Operadora, valorDescontoBoleto, valorDescontoCupom, null);

                        _pagamentoService.AtualizaCupom(pagamento.IdPagamento, "");

                        if (model.IdPedido > 0)
                        {
                            _pedidoService.Atualiza(model.IdPedido, (int)EnumPedidoStatus.AguardandoPagamento, valorTotalLiquido);
                        }

#if !DEBUG
                        _pagamentoService.AlteraFunilRdStation(model.Email, ObtemValorConfig("rdStationIdentificador"), RdStationEstagioLead.Lead_Qualificado, true);
#endif

                        TempData["CriptoItau"] = cripto;
                        return "B";
                    }
                case (int)EnumPagamentoOperadora.RedeCard:
                    {
                        _pagamentoService.ProcessaPagamentoRedeCard(pagamento,
                                                                    model.MesAno,
                                                                    model.CodigoSeguranca,
                                                                    Regex.Replace(model.NumeroCartao, @"\s", ""),
                                                                    model.Bandeira,
                                                                    model.NomeCartao,
                                                                    model.Parcelas,
                                                                    model.IdPedido > 0
                                                                      ? model.IdPedido.ToString().PadLeft(8, '0')
                                                                      : "AV" + pagamento.IdPagamento.ToString().PadLeft(6, '0'),
                                                                    valorTotalLiquido.ToString("0.00", culture),
                                                                    ObtemValorConfig(CHAVE_FILIACAO_REDE),
                                                                    ObtemValorConfig(CHAVE_SENHA_REDE));

                        _pagamentoService.Atualiza(pagamento.IdPagamento, pagamento.Status, pagamento.Operadora, valorDescontoBoleto,
                          valorDescontoCupom, (DateTime)pagamento.DataPagamento);

                        _pagamentoService.AtualizaPagamentoFinanceiro(pagamento.IdPagamento, VENDEDOR_PADRAO);

#if !DEBUG
                        _pagamentoService.MarcaVendaRdStation(ObtemValorConfig("rdStationIdentificador"), valorTotalLiquido, model.Email);
#endif

                        if (model.IdPedido > 0)
                        {
                            _pedidoService.Atualiza(model.IdPedido, (int)EnumPedidoStatus.PagamentoConfirmado, valorTotalLiquido);
                            //_pedidoService.AtualizaStatus((int)EnumPedidoStatus.PagamentoConfirmado, model.IdPedido);
                        }

                        return "C";
                    }
            }

            return string.Empty;
        }

        private static void ValidacoesCamposObrigatorios(PagamentoAvulsoModel model)
        {
            var validacoes = "Verifique os campos: ";

            if (string.IsNullOrEmpty(model.NomeCompleto))
                validacoes += "Nome Completo. ";

            if (string.IsNullOrEmpty(model.Documento))
                validacoes += "Documento. ";

            if (string.IsNullOrEmpty(model.Email))
                validacoes += "Email. ";

            if (string.IsNullOrEmpty(model.Cep))
                validacoes += "Cep. ";

            if (string.Equals(model.OperadoraPagamento, "3"))
            {
                if (string.IsNullOrEmpty(model.Parcelas))
                    validacoes += "Parcelas. ";

                if (string.IsNullOrEmpty(model.NumeroCartao))
                    validacoes += "Número do Cartão. ";

                if (string.IsNullOrEmpty(model.MesAno))
                    validacoes += "Validade do Cartão. ";

                if (string.IsNullOrEmpty(model.NomeCartao))
                    validacoes += "Nome do Cartão. ";

                if (string.IsNullOrEmpty(model.NomeCartao))
                    validacoes += "Código de segurança. ";
            }

            if (!string.Equals("Verifique os campos: ", validacoes))
                throw new Exception(validacoes);
        }

        private void DefineTipoPagamento(string operadorasAceitas, DateTime? dataIda)
        {
            ViewBag.MetodoPagamento = new List<SelectListItem>();

            if (operadorasAceitas.Contains("1") && (dataIda == null || VerificaDiasLimiteBoleto(dataIda)))
                ViewBag.MetodoPagamento.Add(new SelectListItem { Text = "Boleto bancário", Value = "1" });

            if (operadorasAceitas.Contains("3"))
                ViewBag.MetodoPagamento.Add(new SelectListItem { Text = "Cartão de crédito", Value = "3" });
        }

        private PedidoItem BuscaPedidoItem(PagamentoAvulsoModel model, PagamentoAvulso pagamento)
        {
            var pedidoItem = _pedidoService.Busca(model.IdPedido);
            model.NomeDestino = pedidoItem.Destino.Nome;
            model.NomePlano = pedidoItem.Plano.Nome;
            model.DataIda = pedidoItem.DataIda.ToString("dd/MM/yyyy");
            model.DataVolta = pedidoItem.DataVolta.ToString("dd/MM/yyyy");

            pagamento.OperadorasAceitas = "1,3";

            return pedidoItem;
        }

        private PagamentoAvulso BuscaPagamento(PagamentoAvulsoModel model)
        {
            var pagamento = _pagamentoService.BuscaPagamentoAvulso(model.IdPagamento);

            model.NomeCompleto = pagamento.NomeCompleto;
            model.Documento = pagamento.Documento;
            model.Email = pagamento.Email;
            model.Cep = pagamento.Cep;
            model.ValorTotal = pagamento.ValorTotal.ToString(new CultureInfo("en-US"));
            model.ValorDescontoCupom = pagamento.ValorDescontoCupom > 0 ? pagamento.ValorDescontoCupom.ToString(new CultureInfo("en-US")) : null;
            model.ValorDescontoBoleto = pagamento.ValorDescontoBoleto > 0 ? pagamento.ValorDescontoBoleto.ToString(new CultureInfo("en-US")) : null;
            model.OperadoraPagamento = pagamento.IdPagamentoOperadora;

            return pagamento;
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                    viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                    ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private void EnviaEmail(string nome, string pedido, CarrinhoModel carinho, List<string> descricaoPlanos, bool novoUsuario, string email, string senha, string status)
        {
            var planos = string.Join(",", descricaoPlanos.ToArray().Distinct());

            string emailUsuario = null, senhaUsuario = null;

            if (novoUsuario)
            {
                emailUsuario = email;
                senhaUsuario = senha;
            }

            var emailModel = new EmailModel()
            {
                Nome = nome,
                NumeroPedido = pedido,
                Destino = carinho.Planos.Destino.Nome,
                DataIda = carinho.DataIda,
                DataVolta = carinho.DataVolta,
                Planos = planos,
                EmailUsuario = emailUsuario,
                SenhaUsuario = senhaUsuario,
                Status = status
            };

            var html = RenderRazorViewToString("~/Views/Email/_Agradecimento.cshtml", emailModel);
            var texto = Regex.Replace(Regex.Replace(html, "<.*?>", string.Empty).Trim().Replace("  ", ""), @"[\r\n]{2,}", "\n\n");
            html = Regex.Replace(html, "[\r\n]{2,}", "").Trim().Replace("  ", "");

            _emailAgendadoService.InsereEmail(EMAIL_REMETENTE, EMAIL_NOME_REMETENTE, email, "Multi Seguro Viagem - Pedido " + pedido, html, texto);
        }

        private void CadastraIntencao(IReadOnlyList<CarrinhoModel> carrinho, PagamentoModel model, Pagamento pagamento)
        {
            var viajantes = carrinho[0].Viajantes.Select(
                viajante => new Viajante(
                    viajante.Nome,
                    viajante.Cpf,
                    Convert.ToDateTime(viajante.DataNascimento),
                    viajante.Sexo.Equals("M") ? 1 : 2,
                    Convert.ToDecimal(viajante.ValorUnitario, new CultureInfo("en-US")),
                    string.Concat(carrinho[0].Planos.Operadora.Nome, " - ", carrinho[0].Planos.Nome))
            ).ToList();

            _intencaoService.Cadastra(carrinho[0].Planos.Destino.IdDestino.ToString(),
                                      Convert.ToDateTime(carrinho[0].DataIda),
                                      Convert.ToDateTime(carrinho[0].DataVolta),
                                      model.EmailCotacao,
                                      model.OrigemLead,
                                      Request.UrlReferrer?.ToString(),
                                      Request.UserHostAddress,
                                      viajantes,
                                      pagamento.Email,
                                      model.TelefonePessoal,
                                      model.Cep,
                                      pagamento.Pedido.IdPedido,
                                      pagamento.IdPagamento,
                                      pagamento.Valor,
                                      pagamento.ValorDescontoTotal,
                                      model.Cupom,
                                      pagamento.Operadora == 1 ? "Boleto" : "Crédito",
                                      pagamento.Status == 2);
        }

        private void DefineMetodosPagamento(string dataIda)
        {
            var dataAtual = DateTime.Now;
            var dataViagem = Convert.ToDateTime(dataIda);

            var lstMetodoPagamento = new List<SelectListItem>
        {
           new SelectListItem  {  Text = "Cartão de crédito",  Value = "3"}
      };

            if (dataViagem.Date >= dataAtual.Add(TimeSpan.FromDays(3)).Date)
                lstMetodoPagamento.Add(new SelectListItem { Text = "Boleto bancário", Value = "1" });

            ViewBag.MetodoPagamento = lstMetodoPagamento;
        }

        private static bool VerificaDiasLimiteBoleto(DateTime? dataIda)
        {
            return dataIda?.Date >= DateTime.Now.AddDays(3).Date;
        }

        #endregion
    }
}
