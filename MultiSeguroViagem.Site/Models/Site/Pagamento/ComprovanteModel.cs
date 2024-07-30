namespace MultiSeguroViagem.Site.Models.Site.Pagamento
{
    public class ComprovanteModel
    {
        //Pagamento
        public int IdPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorDesconto { get; set; }
        public System.DateTime DataCriacao { get; set; }

        //Rede
        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public int QuantidadeParcelas { get; set; }
        public string Bandeira { get; set; }

        //Usuario
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }
}