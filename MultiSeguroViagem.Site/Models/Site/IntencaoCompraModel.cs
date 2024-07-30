using System.Collections.Generic;
using MultiSeguroViagem.Site.Models.Site.Viajante;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class IntencaoCompraModel
    {
        public string Destino { get; set; }
        public string DataIda { get; set; }
        public string DataVolta { get; set; }
        public string Email { get; set; }
        public string Origem { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Referrer { get; set; }
        public string Ip { get; set; }
        public List<ViajanteIntencaoModel> Viajantes { get; set; }
    }
}