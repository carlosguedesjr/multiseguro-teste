

namespace MultiSeguroViagem.Service.Models
{
    public class VoucherModel
    {
        public string IdVoucher { get; set; }
        public string CodigoEmissao { get; set; }
        public string CodigoBilhete { get; set; }
        public string CodigoVoucher { get; set; }
        public string NomeViajante { get; set; }
        public string InicioVigencia { get; set; }
        public string FimVigencia { get; set; }
        public string Dias { get; set; }
        public string Cambio { get; set; }
        public string Total { get; set; }
        public string UriVoucher { get; set; }
        public string UriBilhete { get; set; }
        public string UriCondicaoGeral { get; set; }
        public string Status { get; set; }
    }
}
