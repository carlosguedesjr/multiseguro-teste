
namespace MultiSeguroViagem.Common.Helpers
{
  public static class Constantes
  {
    #region URI Pagamentos
    //PRD  https://api.userede.com.br/erede/v1/transactions
    //HOM https://scommerce.userede.com.br/Redecard.Komerci.External.WcfKomerci/KomerciWcf.svc
    public const string URI_REDECARD = "https://sandbox-erede.useredecloud.com.br/v1/transactions";

    #endregion

    #region Credenciais Itau       
    public const string CREDENCIAL_ITAU_CODEMPRESA = "J0435142780001900000037690";
    public const string CREDENCIAL_ITAU_CODITAU = "EKTOE2021SKE0E21";
    public const string URI_ITAU_CONSULTA = "https://shopline.itau.com.br/shopline/consulta.aspx";

    /*
    * formatoRetorno
    * 1- XML
    * 2- HTML
    */
    public const string FORMATO_RETORNO = "1";

    #endregion

    #region CHAVES

    public const string CHAVE_IDDESTINO = "idDestino";
    public const string CHAVE_DATAIDA = "dataIda";
    public const string CHAVE_DATAVOLTA = "dataVolta";
    public const string CHAVE_IDPLANO = "idPlano";

    public const string CHAVE_CODEMPRESA_ITAU = "CodigoEmpresaItau";
    public const string CHAVE_CODITAU = "CodigoItau";
    public const string CHAVE_FILIACAO_REDE = "FiliacaoRede";
    public const string CHAVE_SENHA_REDE = "SenhaRede";

    public const string CHAVE_URI_BASE = "UriBase";

    public const string CHAVE_URI_OBTEM_COTACAO = "UriCotacao";
    public const string CHAVE_URI_OBTEM_USUARIO = "UriUsuario";
    public const string CHAVE_URI_OBTEM_CUPOM = "UriCupom";

    public const string CHAVE_URI_DOWNLOAD_FILE = "UriDownloadFile";


    #endregion

    #region Limites

    public const int LIMITE_INICIAL = 10;
    public const int LIMITE_PARCIAL = 5;

    #endregion

    #region Observações
    public const string OBSERVACAO_ADICIONAL1_ITAU = "NÃO RECEBER APÓS A DATA DE VENCIMENTO";
    public const string OBSERVACAO_ADICIONAL2_ITAU = "Em caso de dúvidas, entre em contato conosco:";
    public const string OBSERVACAO_ADICIONAL3_ITAU = "atendimento@multiseguroviagem.com.br (19) 3201-0447";

    #endregion

    #region Email
    public const string ATENDIMENTO_REMETENTE = "atendimento@multiseguroviagem.com.br";
    public const string EMAIL_REMETENTE = "contato@multiseguroviagem.com.br";
    public const string EMAIL_NOME_REMETENTE = "Multi Seguro Viagem";

    #endregion

    #region Usuario Padrão
    public const int VENDEDOR_PADRAO = 40;
    #endregion

    #region Cookie
    public const int VERSAO_COOKIE = 1;
    public const string TOKEN_COOKIE_NOME = "TK_CK_MULTI";
    #endregion

    #region Nome API's
    public const string API_MULTIINTEGRA_NOME = "Multi Integra";

    #endregion
  }
}
