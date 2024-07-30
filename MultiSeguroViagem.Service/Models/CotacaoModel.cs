using System.Collections.Generic;

namespace MultiSeguroViagem.Service.Models
{
    public class CotacaoModel
    {
        public string IdPlano { get; set; }
        public string DataIda { get; set; }
        public string DataVolta { get; set; }
      
        public string Destino { get; set; }
        public bool Familiar { get; set; }
        public string CodigoTarifa { get; set; }           
        public string Moeda { get; set; } // USD | R$
        public string ValorPlano { get; set; } // valorTarifa
        public string Markup { get; set; }
        public string Desconto { get; set; }
        public string TipoTarifa { get; set; }
        public ICollection<ViajanteModel> Viajantes { get; set; }
    }
}
