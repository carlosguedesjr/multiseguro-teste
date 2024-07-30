using System.Collections.Generic;
using System.Linq;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using Dapper;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class BannerRepository : IBannerRepository
  {
    private readonly string _cnx;

    public BannerRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }

    public List<Banner> Busca()
    {
      const string sql = @"SELECT 
                              ban.IdBanner,
                              ban.Identificador,
                              ban.Tag,
                              ban.TituloModal,
                              ban.TituloModalBloco1,
                              ban.DescricaoModalBloco1,
                              ban.TituloModalBloco2,
                              ban.DescricaoModalBloco2,
                              ban.FiltroOperadora,
                              ban.FiltroDestino,
                              CONCAT('/', ban.IdFileUploads, '/', SHA1(arq.DataCadastro)) AS CaminhoImagem    
                          FROM
                              Banners ban
                                  INNER JOIN
                              FileUploads arq ON ban.IdFileUploads = arq.IdFileUploads;

                          SELECT 
                              conf.IdConfiguracao,
                              conf.H1,
                              conf.H2,
                              conf.ExibirBanners,
                              CONCAT('/', conf.IdImagemBackground, '/', SHA1(arq.DataCadastro)) AS CaminhoImagemBackground
                          FROM
                              ConfiguracaoHome conf
                                  INNER JOIN
                              FileUploads arq ON conf.IdImagemBackground = arq.IdFileUploads;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var query = cnx.QueryMultiple(sql);

        var banners = query.Read<Banner>().ToList();

        var config = query.Read<ConfiguracaoHome>().First();
        config.SetaBanners(banners);


        MySqlConnection.ClearPool(cnx);

        return banners;
      }
    }
  }
}
