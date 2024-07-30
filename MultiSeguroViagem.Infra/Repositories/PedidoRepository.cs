using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace MultiSeguroViagem.Infra.Repositories
{
	public class PedidoRepository : IPedidoRepository
	{
		private readonly string _cnx;

		public PedidoRepository()
		{
			_cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString; ;
		}

	  public PedidoItem Busca(int idPedido)
	  {
	    const string sql = @"SELECT	
                              PI.IdPedidoItens,                              
                              PI.DataIda,
                              PI.DataVolta,
                              PI.DataVoltaNova,
                              PI.Dias,
                              PI.ValorTotal,
                              PI.Viajantes,
                              PI.DataCriacao,
                              PE.IdPedido,
                              PE.IdUsuario,
                              PE.IdPedidoOrigem,
                              PE.Responsavel,                              
                              PE.ValorTotal,
                              PE.DataCriacao,
                              PE.CancelamentoSolicitado,
                              PE.DataCancelamento,
                              PE.ProtocoloCancelamento,
                              PE.ObservacaoCancelamento,
                              PE.BitTentativa,
                              PL.IdPlano,
                              PL.Nome,
                             PL.CotacaoDiasMinimo,
                              PL.CotacaoDiasMaximo,
                              DE.IdDestino,
                              DE.Nome
                          FROM
                              PedidosItens PI
                                  INNER JOIN
                              Pedidos PE ON PI.IdPedido = PE.IdPedido
                                  INNER JOIN
                              Planos PL ON PI.IdPlano = PL.IdPlano
                                  INNER JOIN
                              Destinos DE ON PI.IdDestino = DE.IdDestino
                          WHERE
                              PI.IdPedido = @idPedido;";

	    using (var cnx = new MySqlConnection(_cnx))
	    {
	      cnx.Open();

	      var pedidoItem = cnx.Query<PedidoItem, Pedido, Plano, Destino, PedidoItem>(sql, (pedItem, pedido, plano, destino) =>
	      {
	        pedItem.DefinePedido(pedido);
	        pedItem.DefinePlano(plano);
          pedItem.DefineDestino(destino);

          return pedItem;
	      },
        new { idPedido },
        splitOn: "IdPedido,IdPlano,IdDestino").FirstOrDefault();
        
        MySqlConnection.ClearPool(cnx);

	      return pedidoItem;
	    }
    }

    public IList<Pedido> ObtemPedidoDetalhes(int idPedido)
    {
      const string sql = @"SELECT 
									ped.IdPedido,
									ped.IdUsuario,
									ped.IdPedidoOrigem,
									ped.Responsavel,
									ped.ValorTotal,
									ped.DataCriacao,
									ped.CancelamentoSolicitado,
									ped.DataCancelamento,
                                    ped.BitTentativa,
									ped.ProtocoloCancelamento,
									ped.ObservacaoCancelamento,
									GROUP_CONCAT(CONCAT(op.Nome, ' ', plano.Nome)) NomePlano,
									pStat.IdPedidoStatus,                                   
									pStat.Status
								FROM
									Pedidos ped
										INNER JOIN
									PedidosStatus pStat ON pStat.IdPedidoStatus = ped.Status
										INNER JOIN
									PedidosItens pItem ON pItem.IdPedido = ped.IdPedido
										INNER JOIN
									Planos plano ON plano.IdPlano = pItem.IdPlano
										INNER JOIN
									Operadoras op ON op.IdOperadora = plano.IdOperadora
								WHERE
									ped.IdPedido = @idPedido
								GROUP BY ped.IdPedido
								ORDER BY ped.IdPedido DESC;

SELECT                                    
									itens.IdPedidoItens,
									itens.DataIda, 
									itens.DataVolta,
                  itens.DataVoltaNova,
									itens.IdDestino, 
									itens.Dias, 
									itens.ValorTotal, 
									itens.Viajantes, 
									itens.DataCriacao,
									ped.IdPedido,
                                    ped.BitTentativa,
									pl.IdPlano,
                  pl.CotacaoDiasMinimo,
                  pl.CotacaoDiasMaximo
								FROM
									Pedidos ped
										INNER JOIN
									PedidosItens itens ON itens.IdPedido = ped.IdPedido
										INNER JOIN
									Planos pl ON pl.IdPlano = itens.IdPlano;                        

								SELECT 
									pu.IdPedido,
									pu.IdFileUploads,                                 
									fu.Arquivo,
									SHA1(fu.DataCadastro) DataCadastro
									FROM
									PedidosFileUploads pu
										INNER JOIN
									FileUploads fu ON fu.IdFileUploads = pu.IdFileUploads;
";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var multi = cnx.QueryMultiple(sql, new { IdPedido = idPedido });

        IList<Pedido> pedidos = multi.Read<Pedido, PedidoStatus, Pedido>((pedido, status) =>
        {
          pedido.DefineStatus(status);
          return pedido;
        },
        splitOn: "IdPedidoStatus"
        ).ToList();

        var itens = multi.Read<PedidoItem, Pedido, Plano, PedidoItem>((item, pedido, plano) =>
        {
          item.DefinePedido(pedido);
          item.DefinePlano(plano);

          return item;
        },
        splitOn: "IdPedido,IdPlano"
        ).ToList();

        var arquivos = multi.Read<ArquivoUpload>().ToList();
        foreach (var pedido in pedidos)
        {
          pedido.DefinePedidoItem(itens.Where(x => x.Pedido.IdPedido == pedido.IdPedido).ToList());
          pedido.DefineArquivos(arquivos.Where(x => x.IdPedido == pedido.IdPedido).ToList());
        }

        MySqlConnection.ClearPool(cnx);

        return pedidos;
      }
    }

    public IList<Pedido> ObtemPedidos(int idUsuario)
    {
      const string sql = @"SELECT 
									ped.IdPedido,
									ped.IdUsuario,
									ped.IdPedidoOrigem,
									ped.Responsavel,
									ped.ValorTotal,
									ped.DataCriacao,
									ped.CancelamentoSolicitado,
									ped.DataCancelamento,
                                    ped.BitTentativa,
									ped.ProtocoloCancelamento,
									ped.ObservacaoCancelamento,
									GROUP_CONCAT(CONCAT(op.Nome, ' ', plano.Nome)) NomePlano,
									pStat.IdPedidoStatus,                                   
									pStat.Status
								FROM
									Pedidos ped
										INNER JOIN
									PedidosStatus pStat ON pStat.IdPedidoStatus = ped.Status
										INNER JOIN
									PedidosItens pItem ON pItem.IdPedido = ped.IdPedido
										INNER JOIN
									Planos plano ON plano.IdPlano = pItem.IdPlano
										INNER JOIN
									Operadoras op ON op.IdOperadora = plano.IdOperadora
								WHERE
									ped.IdUsuario = @IdUsuario
								GROUP BY ped.IdPedido
								ORDER BY ped.IdPedido DESC;

								SELECT                                    
									itens.IdPedidoItens,
									itens.DataIda, 
									itens.DataVolta,
                  itens.DataVoltaNova,
									itens.IdDestino, 
									itens.Dias, 
									itens.ValorTotal, 
									itens.Viajantes, 
									itens.DataCriacao,
									ped.IdPedido,
                                    ped.BitTentativa,
									pl.IdPlano,
                  pl.CotacaoDiasMinimo,
                  pl.CotacaoDiasMaximo
								FROM
									Pedidos ped
										INNER JOIN
									PedidosItens itens ON itens.IdPedido = ped.IdPedido
										INNER JOIN
									Planos pl ON pl.IdPlano = itens.IdPlano;                        

								SELECT 
									pu.IdPedido,
									pu.IdFileUploads,                                 
									fu.Arquivo,
									SHA1(fu.DataCadastro) DataCadastro
									FROM
									PedidosFileUploads pu
										INNER JOIN
									FileUploads fu ON fu.IdFileUploads = pu.IdFileUploads;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var multi = cnx.QueryMultiple(sql, new { IdUsuario = idUsuario });

        IList<Pedido> pedidos = multi.Read<Pedido, PedidoStatus, Pedido>((pedido, status) =>
        {
          pedido.DefineStatus(status);
          return pedido;
        },
        splitOn: "IdPedidoStatus"
        ).ToList();

        var itens = multi.Read<PedidoItem, Pedido, Plano, PedidoItem>((item, pedido, plano) =>
        {
          item.DefinePedido(pedido);
          item.DefinePlano(plano);

          return item;
        },
        splitOn: "IdPedido,IdPlano"
        ).ToList();

        var arquivos = multi.Read<ArquivoUpload>().ToList();
        foreach (var pedido in pedidos)
        {
          pedido.DefinePedidoItem(itens.Where(x => x.Pedido.IdPedido == pedido.IdPedido).ToList());
          pedido.DefineArquivos(arquivos.Where(x => x.IdPedido == pedido.IdPedido).ToList());
        }

        MySqlConnection.ClearPool(cnx);

        return pedidos;
      }

    }

    public IList<Viajante> ObtemPedidoViajantes(int idPedido)
    {
      const string sql = @"SELECT 
                              CONCAT(ope.Nome, ' ', pla.Nome) AS NomePlano,
                              viaj.IdViajante,
                              viaj.IdPedidoItens,
                              viaj.ValorUnitario,
                              viaj.Nome,
                              viaj.Cpf,
                              viaj.DataNascimento,
                              viaj.IdSexo AS Sexo,
                              viaj.Condicao,
                              viaj.UriVoucher,
                              viaj.CodigoVoucher
                          FROM
                              Pedidos ped
                                  INNER JOIN
                              PedidosItens pedItem ON pedItem.IdPedido = ped.IdPedido
                                  INNER JOIN
                              Planos pla ON pla.IdPlano = pedItem.IdPlano
                                  INNER JOIN
                              Operadoras ope ON ope.IdOperadora = pla.IdOperadora
                                  INNER JOIN
                              Viajantes viaj ON viaj.IdPedidoItens = pedItem.IdPedidoItens
                          WHERE
                              ped.IdPedido = @IdPedido";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var viajantes = cnx.Query<Viajante>(sql, new { IdPedido = idPedido }).ToList();

        MySqlConnection.ClearPool(cnx);

        return viajantes;
      }
    }
    

	  public void Atualiza(int idPedido, int idStatus, decimal valorTotal)
	  {
	    const string sql = @"UPDATE Pedidos
								           SET
                             Status = @Status,
                             ValorTotal = @ValorTotal
								           WHERE
                             IdPedido = @IdPedido;";

      using (var cnx = new MySqlConnection(_cnx))
	    {
	      cnx.Open();

	      cnx.Execute(sql, new
	      {
	        IdPedido = idPedido,
	        Status = idStatus,
	        ValorTotal = valorTotal
	      });

	      MySqlConnection.ClearPool(cnx);
      }
    }

	  public void AtualizaStatus(int status, int idPedido)
		{
			const string sql = @"UPDATE Pedidos
								           SET
                              Status = @Status
								           WHERE
                              IdPedido = @IdPedido";

			using (var cnx = new MySqlConnection(_cnx))
			{
				cnx.Open();

				cnx.Execute(sql, new
				{
					Status = status,
					IdPedido = idPedido
				});

				MySqlConnection.ClearPool(cnx);

			}
		}

	  public void AtualizaViajante(IEnumerable<Viajante> viajantes)
	  {
	    const string sql = @"UPDATE Viajantes 
								           SET 
                             IdVoucher = @IdVoucher,
									           CodigoBilhete = @CodigoBilhete,
									           CodigoVoucher = @CodigoVoucher,
									           UriVoucher = @UriVoucher,
									           UriCondicaoGeral = @UriCondicaoGeral
								           WHERE
									           IdViajante = @IdViajante;";

	    using (var cnx = new MySqlConnection(_cnx))
	    {
	      cnx.Open();

	      cnx.Execute(sql, viajantes);

	      MySqlConnection.ClearPool(cnx);
	    }
	  }


    public Pedido Cadastra(string strIp,Pedido pedido)
		{
			const string sql = @"INSERT INTO Pedidos (IdUsuario, Status, ValorTotal, DataCriacao, strIp) 
								 VALUES (@IdUsuario, @IdPedidoStatus, @ValorTotal, @DataCriacao , @strIp); 
								 
								 SELECT LAST_INSERT_ID();";

			using (var cnx = new MySqlConnection(_cnx))
			{
				cnx.Open();

				var id = cnx.Query<int>(sql, new
				{
					IdUsuario = pedido.Usuario.IdUsuario,
					IdPedidoStatus = pedido.Status.IdPedidoStatus,
					ValorTotal = pedido.ValorTotal,
					DataCriacao = pedido.DataCriacao,
          strIp = strIp

        }).Single();

				pedido.DefineIdPedido(id);

				MySqlConnection.ClearPool(cnx);
				return pedido;
			}
		}

		public PedidoItem CadastraItem(PedidoItem item)
		{
			const string sql = @"INSERT INTO PedidosItens (IdPedido, IdPlano, DataIda, DataVolta, DataVoltaNova, IdDestino, Dias, ValorTotal, Viajantes,  DescontoBoletoDia, DescontoCupomDia, ValorNetDia, AjustePorcentagemDia, DataCriacao, CambioDia, DescontoPlanoDia)
								 VALUES (@IdPedido, @IdPlano, @DataIda, @DataVolta, @DataVoltaNova, @IdDestino, @Dias, @ValorTotal, @Viajantes,  @DescontoBoletoDia, @DescontoCupomDia, @ValorNetDia, @AjustePorcentagemDia, @DataCriacao, @CambioDia, @DescontoPlanoDia); 
							   
								 SELECT LAST_INSERT_ID();";

			using (var cnx = new MySqlConnection(_cnx))
			{
				cnx.Open();

				var id = cnx.Query<int>(sql, new
				{
					IdPedido = item.Pedido.IdPedido,
					IdPlano = item.Plano.IdPlano,
					DataIda = item.DataIda,
					DataVolta = item.DataVolta,
          DataVoltaNova = item.DataVoltaNova,
          IdDestino = item.Destino.IdDestino,
					Dias = item.Dias,
					ValorTotal = item.ValorTotal,
					Viajantes = item.Viajantes,
          DescontoBoletoDia = item.DescontoBoletoDia,
          DescontoCupomDia = item.DescontoCupomDia,
          ValorNetDia = item.ValorNetDia,
          AjustePorcentagemDia = item.AjustePorcentagemDia,
          CambioDia = item.CambioDia,
          DescontoPlanoDia = item.DescontoPlanoDia,
          DataCriacao = item.DataCriacao

				}).Single();

				item.DefineIdItem(id);

				MySqlConnection.ClearPool(cnx);
				return item;
			}
		}

		public void CadastraViajante(IEnumerable<Viajante> viajantes)
		{
			const string sql = @"INSERT INTO Viajantes(IdPedidoItens, ValorUnitario, Nome, Cpf, DataNascimento, IdSexo, Condicao)
								 VALUES (@IdPedidoItens, @ValorUnitario, @Nome, @Cpf, @DataNascimento, @IdSexo, @Condicao); 
							   
								 SELECT LAST_INSERT_ID();";

			using (var cnx = new MySqlConnection(_cnx))
			{
				cnx.Open();

				foreach (var item in viajantes)
				{
					var id = cnx.Query<int>(sql, new
					{
						IdPedidoItens = item.Item.IdPedidoItens,
						ValorUnitario = item.ValorUnitario,
						Nome = item.Nome,
						Cpf = item.Cpf,
						DataNascimento = item.DataNascimento,
						IdSexo = item.Sexo,
						Condicao = item.Condicao
					}).Single();

				}

				MySqlConnection.ClearPool(cnx);
			}
		}

		
	}
}
