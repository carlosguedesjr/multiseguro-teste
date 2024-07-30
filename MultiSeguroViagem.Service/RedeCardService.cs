using System;
using MultiSeguroViagem.Common.Validations;
using MultiSeguroViagem.Domain.Interfaces.Services.Services;
using System.Collections.Generic;
using eRede;

namespace MultiSeguroViagem.Service
{
    public class RedeCardService : IRedeCardService
    {
        /// <summary>
        /// Autorização de crédito
        /// </summary>
        /// <remarks>
        /// O valor da transação sensibiliza o limite do cartão do portador, porém não gera cobrança na fatura enquanto não houver a confirmação (captura) dentro do prazo máximo de 29 dias.
        /// Para transações parceladas, informar o número mínimo de 02 e máximo de 12 parcelas. Transações à vista informar 00 ou nulo. 
        /// </remarks>
        /// <param name="ano">Ano da validade do cartão (Ex: 19 ou 2019)</param>
        /// <param name="cvc2">Código de segurança do cartão</param>
        /// <param name="filiacao">Número de filiação do estabelecimento (PV)</param>
        /// <param name="mes">Mês da validade do cartão (Ex: 03)</param>
        /// <param name="numeroCartao">Número do cartão</param>
        /// <param name="numeroPedido">Número do pedido</param>
        /// <param name="origem">Identifica a origem da transação</param>
        /// <param name="parcelas">Número de parcelas em que a transação será autorizada</param>
        /// <param name="portador">Nome do portador do cartão</param>
        /// <param name="recorrente">Se transação for uma recorrência, enviar 1. Caso contrário, enviar 0</param>
        /// <param name="senha">Token de autenticação na API do e.Rede</param>
        /// <param name="total">Valor total da compra (Ex: 34.60)</param>
        /// <param name="transacao">Tipo de Transação</param>
        public Dictionary<string, string> GetAuthorizedCredit(string ano = null, string cvc2 = null, string filiacao = null, string mes = null, string numeroCartao = null,
            string numeroPedido = null, string origem = "01", string parcelas = null, string portador = null, string recorrente = null,
            string senha = null, string total = null, int transacao = 74)
        {
            #region Validações

            AssertionConcern.AssertArgumentNotNull(ano, "O ano não pode ser nulo");
            AssertionConcern.AssertArgumentRange(ano.Length, 2, 4, "O ano deve ser enviado com 2 ou 4 caracteres (Ex: 19 ou 2019)");

            AssertionConcern.AssertArgumentRange(cvc2.Length, 1, 4, "O código de segurança do cartão deve conter até 4 caracteres");

            AssertionConcern.AssertArgumentRange(filiacao.Length, 1, 9, "O número de filiação deve conter até 9 caracteres");

            AssertionConcern.AssertArgumentNotNull(mes, "O mês não pode ser nulo");
            AssertionConcern.AssertArgumentSize(mes.Length, 2, "O mês de validade deve ser enviado com 2 caracteres (Ex: 03)");

            AssertionConcern.AssertArgumentNotNull(numeroCartao, "O número do cartão não pode ser nulo");
            AssertionConcern.AssertArgumentRange(numeroCartao.Length, 10, 19, "O número do cartão deve conter até 19 caracteres");

            AssertionConcern.AssertArgumentNotNull(numeroPedido, "O número do pedido não pode ser nulo");
            AssertionConcern.AssertArgumentRange(numeroPedido.Length, 1, 16, "O número do pedido deve conter até 16 caracteres alfanuméricos");

            AssertionConcern.AssertArgumentNotNull(origem, "O número do pedido não pode ser nulo");
            AssertionConcern.AssertArgumentSize(origem.Length, 2, "O número que identifica a origem da transação deve conter 2 caracteres");

            AssertionConcern.AssertArgumentSize(parcelas.Length, 2, "O número de parcelas deve conter 2 caracteres");

            if (!string.IsNullOrEmpty(portador)) AssertionConcern.AssertArgumentRange(portador.Length, 1, 30, "O nome do portador do cartão deve conter até 30 caracteres");

            AssertionConcern.AssertArgumentNotNull(recorrente, "O número do pedido não pode ser nulo");
            AssertionConcern.AssertArgumentSize(recorrente.Length, 1, "O número que identifica recorrência deve conter 1 caracter");

            AssertionConcern.AssertArgumentNotNull(senha, "A senha não pode ser nula");
            AssertionConcern.AssertArgumentSize(senha.Length, 32, "A senha deve conter 32 caracteres");

            AssertionConcern.AssertArgumentNotNull(total, "O valor total da compra não pode ser nulo");
            AssertionConcern.AssertArgumentRange(total.Length, 1, 10, "O valor total da compra deve conter até 10 caracteres");

            AssertionConcern.AssertArgumentNotNull(transacao, "O valor total da compra não pode ser nulo");
            AssertionConcern.AssertArgumentRange(transacao.ToString().Length, 1, 2, "O tipo de transação deve conter 2 caracteres");

            #endregion

            Dictionary<string, string> resposta = new Dictionary<string, string>();
            // NOVO MÈTODO

            // Configuração da loja
            var store = new eRede.Store("88028836", "a105fe0beecc4e08b586e23ea2dda3b4");
            var parcelasNumber = Convert.ToInt32(parcelas);

            int valorTransacaoNumericoInteiro = 0;
            var valorTransacao = Convert.ToDecimal(total);
            var valorTransacaoSemMilhares = String.Format("{0:0.##}", valorTransacao).Replace(",", "").Replace(".", "");

            valorTransacaoNumericoInteiro = Convert.ToInt32(valorTransacaoSemMilhares);

            // Transação que será autorizada
            var transaction = new eRede.Transaction
            {
                Amount = valorTransacaoNumericoInteiro,
                Reference = "PC-" + numeroPedido,
                Origin = origem,
                Installments = parcelasNumber
            }.CreditCard(
                numeroCartao,
                cvc2,
                mes,
                ano,
                portador
            );

            var response = new eRede.eRede(store).Create(transaction);

            if (response.ReturnCode == "00")
            {
                var dataOperacao = DateTime.Parse(response.DateTime);

                resposta = new Dictionary<string, string>
                {
                    {"CodRet", response.ReturnCode},
                    {"Data", dataOperacao.Date.ToString("dd/MM/yyyy")},
                    {"Hora", dataOperacao.ToString("HH:mm")},
                    {"Msgret", response.ReturnMessage },
                    {"NumAutor", "EKTO"},
                    {"NumPedido", response.Reference},
                    {"NumSqn", response.Nsu},
                    {"Tid", response.Tid}
                };

                return resposta;
            }
            else
            {
                throw new Exception("(" + response.ReturnCode + ") Falha ao autorizar a transação na rede: " + response.ReturnMessage);
            }

            /*
            if (string.Equals(filiacao, "50079557"))
            {
                var autorizacaoCredito = new RedeCardHom.GetAuthorizedCredit()
                {
                    Ano = ano,
                    Cvc2 = cvc2,
                    Filiacao = filiacao,
                    Mes = mes,
                    Nrcartao = numeroCartao,
                    NumPedido = numeroPedido,
                    Origem = origem,
                    Parcelas = parcelas,
                    Portador = portador,
                    Recorrente = recorrente,
                    Senha = senha,
                    Total = total,
                    Transacao = transacao,
                    TransacaoSpecified = true
                };

                var retorno = new RedeCardHom.KomerciWcf().GetAuthorizedCredit(autorizacaoCredito);

                if (!retorno.CodRet.Equals("00"))
                    throw new Exception(retorno.Msgret); // Erro

                var resposta = new Dictionary<string, string>
                {
                    {"CodRet", retorno.CodRet},
                    {"Data", retorno.Data},
                    {"Hora", retorno.Hora},
                    {"Msgret", retorno.Msgret},
                    {"NumAutor", retorno.NumAutor},
                    {"NumPedido", retorno.NumPedido},
                    {"NumSqn", retorno.NumSqn},
                    {"Tid", retorno.Tid}
                };

                return resposta;
            }
            else
            {
                var autorizacaoCredito = new RedeCardProd.GetAuthorizedCredit()
                {
                    Ano = ano,
                    Cvc2 = cvc2,
                    Filiacao = filiacao,
                    Mes = mes,
                    Nrcartao = numeroCartao,
                    NumPedido = numeroPedido,
                    Origem = origem,
                    Parcelas = parcelas,
                    Portador = portador,
                    Recorrente = recorrente,
                    Senha = senha,
                    Total = total,
                    Transacao = transacao,
                    TransacaoSpecified = true
                };

                var retorno = new RedeCardProd.KomerciWcf().GetAuthorizedCredit(autorizacaoCredito);

                if (!retorno.CodRet.Equals("00"))
                    throw new Exception($"RedeCard: {retorno.Msgret}");

                var resposta = new Dictionary<string, string>
                {
                    {"CodRet", retorno.CodRet},
                    {"Data", retorno.Data},
                    {"Hora", retorno.Hora},
                    {"Msgret", retorno.Msgret},
                    {"NumAutor", retorno.NumAutor},
                    {"NumPedido", retorno.NumPedido},
                    {"NumSqn", retorno.NumSqn},
                    {"Tid", retorno.Tid}
                };

                return resposta;
            }
            */
        }


        /// <summary>
        /// Confirma transação de credito
        /// </summary>
        public void ConfirmTxnTid(int idPagamento, string filiacao = null, string senha = null, string tid = null, string total = null)
        {
            //    var confirmacao = new ConfirmTxnTID()
            //    {
            //        Filiacao = filiacao,
            //        Senha = senha,
            //        Tid = tid,
            //        Total = total
            //    };

            //    var retorno = new KomerciWcf().ConfirmTxnTID(confirmacao);


            //    _pagamentoService.AtualizaAutorizacao(idPagamento, retorno.CodRet, retorno.Msgret);

            //    if (!retorno.CodRet.Equals("00")) throw new Exception(retorno.Msgret);   
        }

        /// <summary>
        /// Realiza cancelamento. O cancelamento de transações de débito e crédito capturadas via webservice só é possível até às 23:59 da data da captura e de forma integral
        /// Após este prazo deve ser executado eu dos seguintes canais Portal da Rede ou TOCA
        /// </summary>
        public Dictionary<string, string> Cancelamento(string filiacao = null, string senha = null, string dataOperacao = null, string numeroAutorizacao = null, string numeroSequencial = null, string tid = null)
        {
            Dictionary<string, string> resposta = new Dictionary<string, string>();

            var store = new eRede.Store("88028836", "a105fe0beecc4e08b586e23ea2dda3b4");
            var transaction = new eRede.Transaction
            {
                Tid = tid
            };

            var response = new eRede.eRede(store).Cancel(transaction);

            if (response.ReturnCode == "00")
            {
                var dataOperacaoRet = DateTime.Parse(response.DateTime);

                resposta = new Dictionary<string, string>
                {
                    {"CodRet", response.ReturnCode},
                    {"Data", dataOperacaoRet.Date.ToString("dd/MM/yyyy")},
                    {"Hora", dataOperacaoRet.ToString("HH:mm")},
                    {"Msgret", response.ReturnMessage },
                    {"NumAutor", "EKTO"},
                    {"NumPedido", response.Reference},
                    {"NumSqn", response.Nsu},
                    {"Tid", response.Tid}
                };

                return resposta;
            }
            else
            {
                throw new Exception("(" + response.ReturnCode + ") Falha ao autorizar a transação na rede: " + response.ReturnMessage);
            }

            /*if (string.Equals(filiacao, "50079557"))
            {

                var cancelamento = new RedeCardHom.VoidTransactionTID()
                {
                    Data = dataOperacao,
                    Filiacao = filiacao,
                    Senha = senha,
                    NumAutor = numeroAutorizacao,
                    NumSqn = numeroSequencial,
                    Tid = tid
                };

                var retorno = new RedeCardHom.KomerciWcf().VoidTransactionTID(cancelamento);

                if (!retorno.CodRet.Equals("00"))
                    throw new Exception(retorno.MsgRet); // Erro

                var resposta = new Dictionary<string, string>
                {
                    {"CodRet", retorno.CodRet},
                    {"Data", retorno.Data},
                    {"Hora", retorno.Hora},
                    {"Msgret", retorno.MsgRet},
                    {"NumSqn", retorno.NumSqn},
                    {"Tid", retorno.Tid}
                };

                return resposta;
            }
            else
            {

                var cancelamento = new RedeCardProd.VoidTransactionTID()
                {
                    Filiacao = filiacao,
                    Senha = senha,
                    Data = dataOperacao,
                    NumSqn = numeroSequencial
                };

                var retorno = new RedeCardProd.KomerciWcf().VoidTransactionTID(cancelamento);

                if (!retorno.CodRet.Equals("00"))
                    throw new Exception(retorno.MsgRet); // Erro

                var resposta = new Dictionary<string, string>
                {
                    {"CodRet", retorno.CodRet},
                    {"Data", retorno.Data},
                    {"Hora", retorno.Hora},
                    {"Msgret", retorno.MsgRet},
                    {"NumSqn", retorno.NumSqn},
                    {"Tid", retorno.Tid}
                };

                return resposta;
            }*/
        }
    }
}
