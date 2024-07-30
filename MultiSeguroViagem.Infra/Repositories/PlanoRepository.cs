
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using System;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class PlanoRepository : IPlanoRepository
  {
    private readonly string _cnx;

    public PlanoRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }

    public Plano Busca(string nome)
    {
      const string sql = @"SELECT 
                              pl.IdPlano,
                              pl.Nome,
                              pl.IdadeMinima,
                              pl.IdadeMaxima,
                              pl.MelhorIdadeMinima,
                              pl.MelhorIdadeMaxima,
                              pl.MelhorIdadePorcentagemAcrescimo,
                              pl.Desconto,
                              pl.AjustePorcentagem,
                              pl.Observacao,
                              pl.Gestante,
                              pl.PraticaEsporte,
                              pl.Estudante,
                              pl.MaisVendido,
                              pl.CotacaoDiasMinimo,
                              pl.CotacaoDiasMaximo,
                              IF(pl.Dolar, (pc.Valor * op.ValorDolar), pc.Valor) AS Preco,
                              pl.UrlImagemSeo AS Imagem,
                              COUNT(pa.IdAvalicacao) AS QuantidadeAvaliacoes,
                              ROUND(SUM(pa.Pontos) / COUNT(pa.IdAvalicacao), 1) AS MediaAvaliacoes,
                              op.IdOperadora,
                              op.Nome,
                              op.ImagemLogo,
                              op.ValorDolar,
                              op.Site,
                              op.SiteBlog,
                              op.SiteReclameAqui,
                              dt.IdDestino,
                              dt.Nome
                          FROM
                              Planos pl
                                  INNER JOIN
                              Operadoras op ON op.IdOperadora = pl.IdOperadora
                                  INNER JOIN
                              PlanosDestinos pd ON pd.IdPlano = pl.IdPlano
                                  INNER JOIN
                              Destinos dt ON dt.IdDestino = pd.IdDestino
                                  INNER JOIN
                              Precos pc ON pc.IdPlano = pl.IdPlano                                        
                                  LEFT JOIN
                              PlanosAvaliacoes pa ON pa.IdPlano = pl.IdPlano
                          WHERE LOWER(REPLACE(pl.Nome, ' ', '')) = @nome
                          GROUP BY pl.IdPlano
                          ORDER BY pl.MaisVendido DESC , Preco - Desconto ASC;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var dados = cnx.Query<Plano, Operadora, Destino, Plano>(sql, (plano, operadora, destino) =>
        {
          plano.DefineOperadora(operadora);
          plano.DefineDestino(destino);

          decimal.Round(plano.Preco, 4);
          decimal.Round(plano.PrecoComercial, 4);
          decimal.Round(plano.PrecoMelhorIdade, 4);

          return plano;
        }, new {nome}, splitOn: "IdOperadora,IdDestino").FirstOrDefault();
        
        MySqlConnection.ClearPool(cnx);

        return dados;
      }
    }

    public ICollection<Plano> BuscaPlanos(int idDestino, int quantidadeDias)
    {
      const string sql = @"SELECT 
                              pl.IdPlano,
                              pl.IdStatusMinDias,
                              pl.Nome,
                              pl.IdadeMinima,
                              pl.IdadeMaxima,
                              pl.MelhorIdadeMinima,
                              pl.MelhorIdadeMaxima,
                              pl.MelhorIdadePorcentagemAcrescimo,
                              pl.Desconto,
                              pl.AjustePorcentagem,
                              pl.Observacao,
                              pl.Gestante,
                              pl.PraticaEsporte,
                              pl.Estudante,
                              pl.MaisVendido,
                              pl.CotacaoDiasMinimo,
                              pl.CotacaoDiasMaximo,
                              IF(pl.Dolar, (pc.Valor * op.ValorDolar), pc.Valor) AS Preco,
                              pl.UrlImagemSeo AS Imagem,
                              COUNT(pa.IdAvalicacao) AS QuantidadeAvaliacoes,
                              ROUND(SUM(pa.Pontos) / COUNT(pa.IdAvalicacao), 1) AS MediaAvaliacoes,
                              op.IdOperadora,
                              op.Nome,
                              op.ImagemLogo,
                              op.ValorDolar,
                              op.Site,
                              op.SiteBlog,
                              op.SiteReclameAqui,
                              dt.IdDestino,
                              dt.Nome
                          FROM
                              Planos pl
                                  INNER JOIN
                              Operadoras op ON op.IdOperadora = pl.IdOperadora
                                  INNER JOIN
                              PlanosDestinos pd ON pd.IdPlano = pl.IdPlano
                                  INNER JOIN
                              Destinos dt ON dt.IdDestino = pd.IdDestino
                                  INNER JOIN
                              Precos pc ON pc.IdPlano = pl.IdPlano                                        
                                  LEFT JOIN
                              PlanosAvaliacoes pa ON pa.IdPlano = pl.IdPlano
                          WHERE pl.IdPlanoStatus = 1
                                  AND dt.IdDestino = @Destino
                                  AND pc.Dia = @QuantidadeDias
                                  AND op.Status = TRUE
                          GROUP BY pl.IdPlano
                          ORDER BY pl.MaisVendido DESC , IF(pl.Dolar, (pc.Valor * op.ValorDolar), pc.Valor) - Desconto ASC;

                          SELECT                               
                              cob.IdCobertura,          
                              cob.Nome,
                              pc.IdPlano,
                              pc.Incluso,
                              pc.ValorCobertura,
                              pc.MoedaCobertura,
                              pc.ValorAcrescimo,
                              pc.MoedaAcrescimo,
                              pc.Ativo,
                              pc.MensagemPlano
                          FROM
                              Coberturas cob
                                  INNER JOIN
                              PlanoCoberturas pc ON pc.IdCobertura = cob.IdCobertura
                          ORDER BY cob.IdCobertura;

                          SELECT 
                              pu.IdPlano,
                              pu.IdFileUploads,
                              pu.TipoArquivo,
                              fu.Arquivo,
                              SHA1(fu.DataCadastro) DataCadastro
                          FROM
                              PlanoUploads pu
                                  INNER JOIN
                              FileUploads fu ON fu.IdFileUploads = pu.IdFileUploads;

                            SELECT 
                                    rs.*,bc.strEmail,bc.strTelefone
                                FROM
                                    multiSeguroViagem.RealSegurosConfig rs
                                JOIN Blacklist bc;";


      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var multi = cnx.QueryMultiple(sql, new { Destino = idDestino, QuantidadeDias = quantidadeDias });

        IEnumerable<Plano> planos = multi.Read<Plano, Operadora, Destino, Plano>((plano, operadora, destino) =>
        {
          plano.DefineOperadora(operadora);
          plano.DefineDestino(destino);

          Decimal.Round(plano.Preco, 4);
          Decimal.Round(plano.PrecoComercial, 4);
          Decimal.Round(plano.PrecoMelhorIdade, 4);

          return plano;
        }, splitOn: "IdOperadora,IdDestino"
        ).AsQueryable();

        var coberturas = multi.Read<Cobertura>().ToList();
        var arquivos = multi.Read<ArquivoUpload>().ToList();
        var realSeguros = multi.Read<RealSeguros>().ToList();
       
        foreach (var plano in planos)
        {
          plano.RealSeguros = realSeguros.ToList();
          plano.DefineCoberturas(coberturas.Where(x => x.IdPlano == plano.IdPlano).ToList());
          plano.DefineArquivos(arquivos.Where(x => x.IdPlano == plano.IdPlano).ToList());
        }

        MySqlConnection.ClearPool(cnx);
      
        return planos.ToList();
      }
    }

    public IList<ArquivoUpload> ObtemArquivosUploadPlanos(int idPlano)
    {
      const string sql = @"SELECT 
                                    pu.IdPlano,
                                    pu.IdFileUploads,
                                    pu.TipoArquivo,
                                    fu.Arquivo,
                                    SHA1(fu.DataCadastro) DataCadastro
                                FROM
                                    PlanoUploads pu
                                        INNER JOIN
                                    FileUploads fu ON fu.IdFileUploads = pu.IdFileUploads
                                    WHERE IdPlano = @IdPlano;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var arquivoUpload = cnx.Query<ArquivoUpload>(sql, new { IdPlano = idPlano }).ToList();



        MySqlConnection.ClearPool(cnx);

        return arquivoUpload;
      }
    }


    public IList<TarifasAssistCard> ObtemTarifasAssisCard(int idPlano)
    {
      const string sql = @"SELECT 
                                    *
                                FROM
                                    multiSeguroViagem.TarifasAssistCard
                                WHERE
                                    IdPlano = @IdPlano;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var tarifas = cnx.Query<TarifasAssistCard>(sql, new { IdPlano = idPlano }).ToList();

        MySqlConnection.ClearPool(cnx);

        return tarifas;
      }
    }
  }
}
