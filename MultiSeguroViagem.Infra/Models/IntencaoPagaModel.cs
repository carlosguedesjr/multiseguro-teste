using MongoDB.Bson;

namespace MultiSeguroViagem.Infra.Models
{
    public class IntencaoPagaModel : IntencaoCompraModel
    {
        public ObjectId _id { get; set; }
        public string emailPagamento { get; set; }
        public string telefone { get; set; }
        public string cep { get; set; }
        public int idPedido { get; set; }
        public int idPagamento { get; set; }
        public float valor { get; set; }
        public float valorDesconto { get; set; }
        public string cupomDesconto { get; set; }
        public string tipoPagamento { get; set; }
        public bool pago { get; set; }
    }
}
