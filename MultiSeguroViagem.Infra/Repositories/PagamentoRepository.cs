using System;
using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;

namespace MultiSeguroViagem.Infra.Repositories
{
  public class PagamentoRepository : IPagamentoRepository
  {
    private readonly string _cnx;

    public PagamentoRepository()
    {
      _cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
    }

    public Pagamento Busca(int idPagamento)
    {
      const string sql = @"SELECT pag.* FROM Pagamentos pag WHERE pag.IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var pagamento = cnx.Query<Pagamento>(sql, new { IdPagamento = idPagamento }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);


        return pagamento;
      }
    }

    public PagamentoAvulso BuscaPagamentoAvulso(int idPagamento)
    {
      const string sql = @"SELECT 
                                PG.IdPagamento,
                                PG.IdPedido,
                                PG.IdPagamentoOperadora,
                                PG.Status,
                                PG.Nome AS NomeCompleto,
                                PG.Email,
                                PG.Documento,
                                PG.Cep,
                                PG.Valor AS ValorTotal,
                                PG.ValorDesconto,
                                PG.ValorDescontoBoleto,
                                PG.ValorDescontoCupom,
                                PG.CupomDesconto,
                                PG.DataVencimento,
                                GROUP_CONCAT(PA.IdPagamentoOperadora SEPARATOR ',') AS OperadorasAceitas
                            FROM
                                Pagamentos PG
                                    INNER JOIN
                                PagamentosAvulso PA ON PG.IdPagamento = PA.IdPagamento
                            WHERE
                                PG.IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var pagamento = cnx.Query<PagamentoAvulso>(sql, new { IdPagamento = idPagamento }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);


        return pagamento;
      }
    }

    public Comprovante ObtemComprovante(int idPagamento)
    {
      const string sql = @"SELECT pag.* FROM Pagamentos pag WHERE pag.IdPagamento = @IdPagamento AND Status = 2;
								
								 SELECT rede.* FROM PagamentosRedeCard rede WHERE rede.IdPagamento = @IdPagamento";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var resultSet = cnx.QueryMultiple(sql, new { IdPagamento = idPagamento });

        MySqlConnection.ClearPool(cnx);

        var pagamento = resultSet.Read<Pagamento>().Single();
        var pagamentoRede = resultSet.Read<PagamentoRedeCard>().FirstOrDefault();

        var comprovante = new Comprovante(pagamento, pagamentoRede);

        return comprovante;
      }
    }

    public Pagamento ObtemPagamentoPedido(int idPedido)
    {
      const string sql = @"SELECT pag.* FROM Pagamentos pag WHERE pag.IdPedido = @IdPedido;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var pagamento = cnx.Query<Pagamento>(sql, new { IdPedido = idPedido }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);


        return pagamento;
      }
    }

    public PagamentoItau ObtemPagamentoItau(int idPagamento)
    {
      const string sql = @"SELECT pag.* FROM PagamentosItau pag WHERE pag.IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var pagamento = cnx.Query<PagamentoItau>(sql, new { IdPagamento = idPagamento }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        return pagamento;
      }
    }

    public IList<PagamentoItau> ObtemBoletosAbertos()
    {
      const string sql = @"SELECT 
                               PagItau.IdPagamentoItau,
                               PagItau.Pedido,
                               PagItau.Valor,
                               Pag.IdPagamento,
                               Pag.Email,
                               Pag.IdPedido
                           FROM
                               Pagamentos Pag
                                   INNER JOIN
                               PagamentosItau PagItau ON PagItau.IdPagamento = Pag.IdPagamento
                           WHERE
                               Pag.IdPagamentoOperadora = 1
                                   AND (Pag.DataCriacao BETWEEN DATE_SUB(CURDATE(), INTERVAL 3 DAY) AND NOW())                                   
                                   AND Pag.Status IN (1 , 5)
                           LIMIT 40;";
      //AND (PagItau.DataConsultaBoleto IS NULL OR (DATE_ADD(PagItau.DataConsultaBoleto, INTERVAL + 6 HOUR) < NOW()))

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var pag = cnx.QueryMultiple(sql);

        var pagamentos = pag.Read<PagamentoItau, Pagamento, Pedido, PagamentoItau>((itau, pagamento, pedido) =>
            {
              itau.DefinePagamento(pagamento);
              pagamento.DefinePedido(pedido);

              return itau;
            },
            splitOn: "IdPagamento, IdPedido"
        ).ToList();

        MySqlConnection.ClearPool(cnx);

        return pagamentos;
      }
    }

    public Pagamento BuscaPagamentoPago()
    {
      const string sql = @"SELECT 
									pag.IdPagamento,
									pag.IdPagamentoOperadora as Operadora,
									pag.Nome,
									pag.Email,
									pag.Documento,
									pag.Cep,
									pag.Valor,
                                    pag.DataCriacao,
									pag.DataPagamento,
									ped.IdPedido,
                                    ped.BitTentativa,
									ped.ValorTotal,
									ped.DataCriacao,
									us.IdUsuario,
									us.Endereco,
									us.Cep,
									us.Numero,
									us.Bairro,
									us.Complemento,
									us.Cidade,
									us.Estado,
									us.Telefone
								FROM
									Pedidos ped
										INNER JOIN
									Pagamentos pag ON pag.IdPedido = ped.IdPedido
										INNER JOIN
									Usuarios us ON us.IdUsuario = ped.IdUsuario
								WHERE
									pag.Status = 2
									AND ped.Status= 1
								ORDER BY pag.DataPagamento
								LIMIT 1;  
				   

								SELECT                                    
										itens.IdPedidoItens,
										itens.DataIda, 
										itens.DataVolta,
                    itens.DataVoltaNova, 	
										itens.Dias,
										itens.Viajantes,  									 
										ped.IdPedido,
                                        ped.BitTentativa,
										dest.IdDestino,
										dest.Nome,
										pl.IdPlano,
										pl.IdPlanoExterno,
										pl.Nome,
										pl.CodigoTarifaExterno,
                      pl.CotacaoDiasMinimo,
                              pl.CotacaoDiasMaximo,
										op.IdOperadora,
										op.Nome
									FROM
										Pedidos ped
											INNER JOIN
										PedidosItens itens ON itens.IdPedido = ped.IdPedido
											INNER JOIN
										Planos pl ON pl.IdPlano = itens.IdPlano  
											INNER JOIN
										Operadoras op ON op.IdOperadora = pl.IdOperadora  
											INNER JOIN
										Destinos dest ON dest.IdDestino = itens.IdDestino;  


								SELECT 
									pu.IdPlano,
									pu.IdFileUploads,
									pu.TipoArquivo,
									fu.Arquivo,
									SHA1(fu.DataCadastro) DataCadastro
								FROM
									PlanoUploads pu
										INNER JOIN
									FileUploads fu ON fu.IdFileUploads = pu.IdFileUploads;";


      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var multi = cnx.QueryMultiple(sql);

        var pagamento = multi.Read<Pagamento, Pedido, Usuario, Pagamento>((pag, pedido, usuario) =>
        {
          pag.DefinePedido(pedido);
          pag.Pedido.DefineUsuario(usuario);
          return pag;
        },
        splitOn: "IdPedido,IdUsuario"
        ).FirstOrDefault();

        if (pagamento != null)
        {

          var itens = multi.Read<PedidoItem, Pedido, Destino, Plano, Operadora, PedidoItem>((item, pedido, destino, plano, operadora) =>
          {
            item.DefinePedido(pedido);
            item.DefineDestino(destino);
            plano.DefineOperadora(operadora);
            item.DefinePlano(plano);

            return item;
          },
          splitOn: "IdPedido,IdDestino,IdPlano,IdOperadora"
          ).ToList();

          var arquivos = multi.Read<ArquivoUpload>().ToList();

          pagamento.Pedido.DefinePedidoItem(itens.Where(x => x.Pedido.IdPedido == pagamento.Pedido.IdPedido).ToList());
          pagamento.Pedido.DefinePedidoItem(itens.Where(x => x.Pedido.IdPedido == pagamento.Pedido.IdPedido).ToList());
          pagamento.Pedido.Itens.FirstOrDefault().Plano?.DefineArquivos(arquivos.Where(x => x.IdPlano == pagamento.Pedido.Itens.FirstOrDefault().Plano.IdPlano).ToList());
        }


        MySqlConnection.ClearPool(cnx);

        return pagamento;
      }
    }

    public IEnumerable<string> CuponsUtilizados(string documento)
    {
      const string sql = "SELECT pag.CupomDesconto FROM Pagamentos pag WHERE Documento = @Documento";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var cupons = cnx.Query<string>(sql, new { Documento = documento }).ToList();

        MySqlConnection.ClearPool(cnx);
        return cupons;
      }
    }

    public Pagamento Cadastra(Pagamento pagamento)
    {
      const string sql = @"INSERT INTO Pagamentos (IdPedido,
                                                   IdPagamentoOperadora,
                                                   Status,
                                                   Nome,
                                                   Email,
                                                   Documento,
                                                   Cep,
                                                   Valor,
                                                   ValorDesconto,
                                                   ValorDescontoBoleto,
                                                   ValorDescontoCupom,                                                   
                                                   DataCriacao,
                                                   DataVencimento)
								                            VALUES(@IdPedido,
                                                   @IdPagamentoOperadora, 
                                                   @IdPedidoStatus, 
                                                   @Nome, 
                                                   @Email,
                                                   @Documento,
                                                   @Cep, 
                                                   @Valor,
                                                   @ValorDesconto,
                                                   @ValorDescontoBoleto,
                                                   @ValorDescontoCupom,                                                   
                                                   @DataCriacao,
                                                   @DataVencimento); 
							   
								 SELECT LAST_INSERT_ID();";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var id = cnx.Query<int>(sql, new
        {
          IdPedido = pagamento.Pedido.IdPedido,
          IdPagamentoOperadora = pagamento.Operadora,
          IdPedidoStatus = pagamento.Status,
          Nome = pagamento.Nome,
          Email = pagamento.Email,
          Documento = pagamento.Documento,
          Cep = pagamento.Cep,
          Valor = pagamento.Valor,
          ValorDesconto = pagamento.ValorDescontoTotal,
          ValorDescontoBoleto = pagamento.ValorDescontoBoleto,
          ValorDescontoCupom = pagamento.ValorDescontoCupom,
          DataCriacao = pagamento.DataCriacao,
          DataVencimento = pagamento.DataVencimento

        }).Single();

        pagamento.DefineIdPagamento(id);

        MySqlConnection.ClearPool(cnx);

        return pagamento;
      }
    }

    public PagamentoRedeCard CadastraAutorizacaoCreditoRedeCard(PagamentoRedeCard rede)
    {
      const string sql = @"INSERT INTO PagamentosRedeCard (IdPagamento, NomeCartao, NumeroCartao, NumeroPedido, QuantidadeParcelas, Bandeira,
																 TID, DataTransacao,CodigoRetorno,MensagemRetorno,NumeroAutorizacao, NumeroSequencial)
								VALUES (@IdPagamento, @NomeCartao, @NumeroCartao, @NumeroPedido, @QuantidadeParcelas, @Bandeira, @TID, @DataTransacao, @CodigoRetorno, @MensagemRetorno,@NumeroAutorizacao,@NumeroSequencial);";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = rede.Pagamento.IdPagamento,
          NomeCartao = rede.NomeCartao,
          NumeroCartao = rede.NumeroCartao,
          NumeroPedido = rede.NumeroPedido,
          QuantidadeParcelas = rede.QuantidadeParcelas,
          Bandeira = rede.Bandeira,
          TID = rede.Tid,
          DataTransacao = rede.DataTransacao,
          CodigoRetorno = rede.CodigoRetorno,
          MensagemRetorno = rede.MensagemRetorno,
          NumeroAutorizacao = rede.NumeroAutorizacao,
          NumeroSequencial = rede.NumeroSequencial
        });

        MySqlConnection.ClearPool(cnx);

        return rede;
      }
    }

    public PagamentoItau CadastraDadosItau(PagamentoItau itau)
    {
      const string sql = @"INSERT INTO PagamentosItau(IdPagamento,Nome,CodigoInscricao,NumeroInscricao,Valor,DataVencimento, Cep,Observacao,ObsAdicional1,ObsAdicional2,ObsAdicional3,UrlRetorna) 
								VALUES(@IdPagamento, @Nome, @CodigoInscricao, @NumeroInscricao, @Valor, @DataVencimento, @Cep, @Observacao, @ObsAdicional1, @ObsAdicional2, @ObsAdicional3, @UrlRetorna); 
								
								UPDATE PagamentosItau SET Pedido =  LPAD((SELECT LAST_INSERT_ID()), 8, '0')
								WHERE IdPagamentoItau = (SELECT LAST_INSERT_ID()); 

								SELECT LAST_INSERT_ID();";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var id = cnx.Query<int>(sql, new
        {
          IdPagamento = itau.Pagamento.IdPagamento,
          Nome = itau.Nome,
          CodigoInscricao = itau.CodigoInscricao,
          NumeroInscricao = itau.NumeroInscricao,
          Valor = itau.Valor,
          DataVencimento = itau.DataVencimento,
          Cep = itau.Cep,
          Observacao = itau.Observacao,
          ObsAdicional1 = itau.ObsAdicional1,
          ObsAdicional2 = itau.ObsAdicional2,
          ObsAdicional3 = itau.ObsAdicional3,
          UrlRetorna = itau.UrlRetorno
        }).Single();

        itau.DefineIdItau(id);

        MySqlConnection.ClearPool(cnx);

        return itau;
      }
    }

    public void CadastraPagamentoFinanceiro(Pagamento pagamento, int idVendedorResponsavel)
    {
      const string sql = @"INSERT INTO PagamentosFinanceiro (IdPagamento, IdVendedorResponsavel) 
								VALUES (@IdPagamento, @IdVendedorResponsavel);";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = pagamento.IdPagamento,
          IdVendedorResponsavel = idVendedorResponsavel
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void Atualiza(int idPagamento, int status, int operadora, decimal valorDescontoBoleto, decimal valorDescontoCupom, DateTime? dataPagamento)
    {
      const string sql = @"UPDATE Pagamentos
								           SET
                             Status = @Status,
                             DataPagamento = @DataPagamento,
                             IdPagamentoOperadora = @IdPagamentoOperadora,
                             ValorDescontoBoleto = @ValorDescontoBoleto,
                             ValorDescontoCupom = @ValorDescontoCupom,
                             ValorDesconto = @ValorDesconto
								           WHERE
                             IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = idPagamento,
          Status = status,
          DataPagamento = dataPagamento,
          IdPagamentoOperadora = operadora,
          ValorDescontoBoleto = valorDescontoBoleto,
          ValorDescontoCupom = valorDescontoCupom,
          ValorDesconto = valorDescontoBoleto + valorDescontoCupom
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaPagamentoFinanceiro(int idPagamento, int idFinanceiroResponsavel)
    {
      const string sql = @"UPDATE PagamentosFinanceiro
								           SET
                              IdFinanceiroResponsavel = @IdFinanceiroResponsavel
								           WHERE
                              IdPagamento = @IdPagamento";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdFinanceiroResponsavel = idFinanceiroResponsavel,
          IdPagamento = idPagamento
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaAutorizacao(int idPagamento, string codigoRetorno, string mensagem)
    {
      const string sql = @"UPDATE PagamentosRedeCard
								 SET CodigoRetorno = @Codigo, MensagemRetorno = @Mensagem 
								 WHERE IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = idPagamento,
          Codigo = codigoRetorno,
          Mensagem = mensagem
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaDataConsultaBoleto(int idPagamentoItau, DateTime dataConsultaBoleto)
    {
      const string sql = @"UPDATE PagamentosItau
								 SET DataConsultaBoleto = @DataConsultaBoleto
								 WHERE IdPagamentoItau = @IdPagamentoItau;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamentoItau = idPagamentoItau,
          DataConsultaBoleto = dataConsultaBoleto
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaCupom(int idPagamento, string cupom)
    {
      const string sql = @"UPDATE Pagamentos
								 SET CupomDesconto = @Cupom
								 WHERE IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = idPagamento,
          Cupom = cupom
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaStatus(int idPagamento, int status, int operadora, DateTime? dataPagamento)
    {
      const string sql = @"UPDATE Pagamentos
								           SET
                             Status = @Status, DataPagamento = @DataPagamento, IdPagamentoOperadora = @IdPagamentoOperadora
								           WHERE
                             IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = idPagamento,
          Status = status,
          DataPagamento = dataPagamento,
          IdPagamentoOperadora = operadora
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaStatus(int idPagamento, int status)
    {
      const string sql = @"UPDATE Pagamentos
								           SET
                             Status = @Status
								           WHERE
                             IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = idPagamento,
          Status = status
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public void AtualizaCancelamentoRedeCard(int idPagamento, string mensagemRetorno, string motivoEstorno, DateTime? dataCancelamento)
    {
      const string sql = @"UPDATE PagamentosRedeCard 
                          SET           
                              MensagemRetorno = @MensagemRetorno,                            
                              MotivoEstorno = @MotivoEstorno,
                              DataCancelamento = @DataCancelamento
                          WHERE
                              IdPagamento = @IdPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        cnx.Execute(sql, new
        {
          IdPagamento = idPagamento,
          MensagemRetorno = mensagemRetorno,
          MotivoEstorno = motivoEstorno,
          DataCancelamento = dataCancelamento
        });

        MySqlConnection.ClearPool(cnx);
      }
    }

    public PagamentoRedeCard BuscaPagamentoRedeCard(int idPagamento)
    {
      const string sql = @"SELECT 
                              *
                          FROM
                              multiSeguroViagem.PagamentosRedeCard
                          WHERE
                              idPagamento = @idPagamento;";

      using (var cnx = new MySqlConnection(_cnx))
      {
        cnx.Open();

        var rede = cnx.Query<PagamentoRedeCard>(sql, new { idPagamento }).FirstOrDefault();

        MySqlConnection.ClearPool(cnx);

        return rede;
      }
    }


  }
}
