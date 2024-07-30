using System;
using MultiSeguroViagem.Domain.Interfaces.Services.Services;
using MultiSeguroViagem.Common.Helpers;
using Itaucripto_NET;

namespace MultiSeguroViagem.Service
{
    public class ItauService : IItauService
    {
        public string GeraCripto(string idPedido, string codigoEmpresa, string valor, string observacao, string chaveItau, string nome,
            string codigoInscricao, string numeroInscricao, string endereco, string bairro, string cep, string cidade, string estado,
            string dataVencimento, string urlRetorno, string obsAdicional1, string obsAdicional2, string obsAdicional3)
        {
            var itau = new Itaucripto();

            var result = itau.geraDados(codigoEmpresa,
                                        idPedido,
                                        valor,
                                        observacao.Length > 40 ? observacao.Substring(0, 40) : observacao,
                                        chaveItau,
                                        nome.Length > 30 ? nome.Substring(0, 30) : nome,
                                        codigoInscricao,
                                        numeroInscricao,
                                        endereco.Length > 40 ? endereco.Substring(0, 40) : endereco,
                                        bairro.Length > 15 ? bairro.Substring(0, 15) : bairro,
                                        cep,
                                        cidade.Length > 15 ? cidade.Substring(0, 15) : cidade,
                                        estado,
                                        dataVencimento,
                                        urlRetorno,
                                        obsAdicional1.Length > 60 ? obsAdicional1.Substring(0, 60) : obsAdicional1,
                                        obsAdicional2.Length > 60 ? obsAdicional2.Substring(0, 60) : obsAdicional2,
                                        obsAdicional3.Length > 60 ? obsAdicional3.Substring(0, 60) : obsAdicional3);

            if (result.Contains("Erro:"))
                throw new Exception(result);

            return result;
        }

        public string SituacaoPagamento(string codigoEmpresa, string chaveItau, string pedido)
        {
            var itau = new Itaucripto();

            var result = itau.geraConsulta(codigoEmpresa, pedido, Constantes.FORMATO_RETORNO, chaveItau);

            if (result.Contains("Erro:"))
                throw new Exception(result);

            return result;
        }
    }
}
