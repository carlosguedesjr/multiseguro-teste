using System;
using System.Linq;
using MongoDB.Driver;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Infra.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiSeguroViagem.Infra.Repositories
{
    public class IntencaoRepository : IIntencaoRepository
    {
        public void Cadastra(Intencao intencao)
        {
            try
            {
                var db = GetDatabase();

                var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var intencaoMongo = JsonConvert.DeserializeObject<IntencaoModel>(JsonConvert.SerializeObject(intencao, jsonSettings));

                var collection = db.GetCollection<IntencaoModel>("intencoes");
                collection.InsertOne(intencaoMongo);
            }
            catch (Exception e) { }
        }

        public void Cadastra(IntencaoCompra intencao)
        {
            try
            {
                var db = GetDatabase();
                var collection = db.GetCollection<IntencaoCompraModel>("intencoesCompra");

                var dbModel = PopulaIntecao(new IntencaoCompraModel(), intencao);

                collection.InsertOne(dbModel);
            }
            catch (Exception e) { }
        }

        public void Cadastra(IntencaoPaga intencao)
        {
            try
            {
                var dbModel = PopulaIntecao(new IntencaoPagaModel(), intencao);

                dbModel.emailPagamento = intencao.EmailPagamento;
                dbModel.telefone = intencao.Telefone;
                dbModel.cep = intencao.Cep;
                dbModel.idPedido = intencao.IdPedido;
                dbModel.idPagamento = intencao.IdPagamento;
                dbModel.valor = (float)intencao.Valor;
                dbModel.valorDesconto = (float)intencao.ValorDesconto;
                dbModel.cupomDesconto = intencao.CupomDesconto;
                dbModel.tipoPagamento = intencao.TipoPagamento;
                dbModel.pago = intencao.Pago;

                var db = GetDatabase();
                var collection = db.GetCollection<IntencaoPagaModel>("intencoesPaga");

                collection.InsertOne(dbModel);
            }
            catch (Exception e) { }
        }

        public void AtualizaStatusPagamento(int idPagamento)
        {
            var db = GetDatabase();
            var collection = db.GetCollection<IntencaoPagaModel>("intencoesPaga");
            var filter = Builders<IntencaoPagaModel>.Filter.Eq(x => x.idPagamento, idPagamento);
            var update = Builders<IntencaoPagaModel>.Update.Set(x => x.pago, true);

            collection.UpdateOne(filter, update);
        }

        private static IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
            return client.GetDatabase("MultiSeguroViagem");
        }

        private T PopulaIntecao<T>(T model, IntencaoCompra intencao) where T : class
        {
            model.GetType().GetProperty("destino")?.SetValue(model, intencao.Destino, null);
            model.GetType().GetProperty("dataIda")?.SetValue(model, intencao.DataIda, null);
            model.GetType().GetProperty("dataVolta")?.SetValue(model, intencao.DataVolta, null);
            model.GetType().GetProperty("email")?.SetValue(model, intencao.Email, null);
            model.GetType().GetProperty("origem")?.SetValue(model, intencao.Origem, null);
            model.GetType().GetProperty("referrer")?.SetValue(model, intencao.Referrer, null);
            model.GetType().GetProperty("ip")?.SetValue(model, intencao.Ip, null);

            var listaViajantes = intencao.Viajantes.Select(viajante => new IntencaoViajanteModel
            {
                nome = viajante.Nome,
                cpf = viajante.Cpf,
                valorUnitario = (float)viajante.ValorUnitario,
                dataNascimento = viajante.DataNascimento,
                sexo = viajante.Sexo == 1 ? "Masculino" : "Feminino",
                operadoraPlano = viajante.NomePlano
            }).ToList();

            model.GetType().GetProperty("viajantes")?.SetValue(model, listaViajantes, null);

            return model;
        }
    }
}
