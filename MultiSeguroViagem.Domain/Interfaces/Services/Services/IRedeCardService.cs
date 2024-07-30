using System.Collections.Generic;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Services
{
  public interface IRedeCardService
  {
    Dictionary<string, string> GetAuthorizedCredit(string ano = null, string cvc2 = null, string filiacao = null, string mes = null,
        string numeroCartao = null, string numeroPedido = null, string origem = null, string parcelas = null,
        string portador = null, string recorrente = null, string senha = null, string total = null,
        int transacao = 74);

    Dictionary<string, string> Cancelamento(string filiacao = null, string senha = null, string dataOperacao = null, string numeroAutorizacao = null, string numeroSequencial = null, string tid = null);
    //void ConfirmTxnTid(int idPagamento, string filiacao = null, string senha = null, string tid = null, string total = null);


    /*
     * * ConfirmTxnTID
     * Ao realizar uma autorização com sucesso, é necessária a confirmação desta transação (captura). Nesse momento é gerada a cobrança na fatura do portador do cartão.
     * A autorização deverá ser captura no período máximo de até 29 dias.
     * Para capturar uma transação é necessário enviar uma das três combinações: “TID”; ou “NUMSQN” e “DATA”; ou “NUMAUTOR” e “DATA”.

     * * GetAuthorizedCredit
     * À vista: Para transações de autorização com captura automática à vista, o tipo de transação deve ser enviado com o valor “04”. 
     * Parcelado sem juros: Para transações de autorização com captura automática parcelado sem juros, o tipo de transação deve ser enviado com o valor “08”. 

     * * GetAuthorizedDebit
     * Transação de débito autenticada

     * * Query
     * A consulta é utilizada para visualizar todas as informações de uma transação especifica. Ela pode ser feita pelo número identificador da transação (TID) ou pelo numero do pedido (NUMPEDIDO).

     * *  VoidTransactionTID
     * Cancelamento
     */
  }
}
