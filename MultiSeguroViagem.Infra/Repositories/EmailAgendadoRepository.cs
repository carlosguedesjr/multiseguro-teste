using System.Collections.Generic;
using System.Linq;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using MultiSeguroViagem.Domain.Entities;
using Dapper;
using MultiSeguroViagem.Infra.Models;
using System.Security.Cryptography;
using System.Text;
using System;

namespace MultiSeguroViagem.Infra.Repositories
{

  public class EmailAgendadoRepository : IEmailAgendadoRepository
  {
    private readonly string _cnx;

    public EmailAgendadoRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString; ;
    }

    public string BuscaHtmlVoucher(Pagamento pagamento, IList<Viajante> viajantes, out string html, out string assunto)
    {
      var idOperadora = pagamento.Pedido.Itens.FirstOrDefault().Plano.Operadora.IdOperadora;
      var Destino = pagamento.Pedido.Itens.FirstOrDefault().Destino.Nome.ToString();
      var DataIda = pagamento.Pedido.Itens.FirstOrDefault().DataIda.ToString("dd/MM/yyyy");
      var DataVolta = pagamento.Pedido.Itens.FirstOrDefault().DataVolta.ToString("dd/MM/yyyy");
      var DataVoltaNova = pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova.ToString("dd/MM/yyyy");
      var DiasMin = pagamento.Pedido.Itens.FirstOrDefault().Plano.CotacaoDiasMinimo;


      var DataVoltaFormatada = DataVolta;
      if (pagamento.Pedido.Itens.FirstOrDefault().DataVoltaNova != pagamento.Pedido.Itens.FirstOrDefault().DataVolta)
      {
        DataVoltaFormatada = DataVolta + " Alterado para " + DataVoltaNova + "*<br><br><span style='color:red !important;'> *O plano contratado possui quantidade mínima de " + DiasMin + " dias de contratação, realizamos a alteração da data de desembarque para que sua apólice esteja de acordo com as regras da seguradora. A alteração não interfere no preço contratado. </span>";
      }

      const string query = @"SELECT 
                                 *
                             FROM
                                 multiSeguroViagem.EmailVoucherLayouts
                             WHERE
                                 IdOperadora = @idOperadora;";



      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var obj = cnx.Query<EmailVoucherLayoutModel>(query, new { idOperadora }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        assunto = obj.Assunto.Replace("{{Id_Pedido}}", pagamento.Pedido.IdPedido.ToString());

        html = obj.MensagemHtml
                          .Replace("{{Nome_Pagante}}", pagamento.Nome)
                          .Replace("{{Id_Pedido}}", pagamento.Pedido.IdPedido.ToString())
                          .Replace("{{Destino}}", Destino)
                          .Replace("{{Embarque}}", DataIda)
                          .Replace("{{Desembarque}}", DataVoltaFormatada)
                          .Replace("{{Data_Criacao_Pedido}}", pagamento.Pedido.DataCriacao.ToString("dd/MM/yyyy"))
                          .Replace("{{Link_Condicao_Geral_Operadora}}", viajantes.FirstOrDefault().UriCondicaoGeral)
                          .Replace("{{Link_Aviso_Viajante}}", @"https://sistema.multiseguroviagem.com.br/pdf/aviso_ao_viajante.pdf")
                          .Replace("{{Data_Criacao_Pedido}}", pagamento.Pedido.DataCriacao.ToString());

        var viajanteHtml = string.Empty;

        foreach (var viajante in viajantes)
        {
          if (viajante.UriVoucher.Contains("universal-assistance") || viajante.UriVoucher.Contains("intermacseguros") || viajante.UriVoucher.Contains("assist-card"))
          {
            viajanteHtml += obj.ViajanteHtml
                              .Replace("{{Nome}}", viajante.Nome)
                              .Replace("{{Codigo_Voucher}}", viajante.CodigoVoucher)
                              .Replace("{{Link_Voucher}}", viajante.UriVoucher);
          }
          else
          {
            viajanteHtml += obj.ViajanteHtml
                              .Replace("{{Nome}}", viajante.Nome)
                              .Replace("{{Codigo_Voucher}}", viajante.CodigoVoucher)
                              .Replace("{{Link_Voucher}}", "https://sistema.multiseguroviagem.com.br/confereclick?url=" + viajante.UriVoucher + "&IdPedido=" + pagamento.Pedido.IdPedido.ToString());
          }
        }

        html = html.Replace("{{Bloco_Viajante}}", viajanteHtml);
      }

      return html;
    }

    public void InsereEmail(EmailAgendado email)
    {
      const string sql = @"INSERT INTO EmailsAgendados (Remetente, NomeRemetente, Destinatario, Assunto, MensagemHtml, MensagemText, DataCriacao, NomeDestinatario) 
                                 VALUES(@Remetente, @NomeRemetente,@Destinatario, @Assunto, @MensagemHtml, @MensagemText, @DataCriacao, @NomeDestinatario);";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, email);

        MySqlConnection.ClearPool(cnx);
      }
    }
  }
}
