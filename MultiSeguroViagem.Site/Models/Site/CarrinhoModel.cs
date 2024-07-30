using System;
using System.Collections.Generic;
using MultiSeguroViagem.Site.Models.Site.Viajante;

namespace MultiSeguroViagem.Site.Models.Site
{
    public class CarrinhoModel
    {
        public int Dias { get; set; }
        public int QuantidadeViajantes { get; set; }
        public string DataIda { get; set; }
        public string DataVolta { get; set; }

        public Decimal ValorTotal { get; set; }
        public PlanoModel Planos { get; set; }
        public IEnumerable<ViajanteModel> Viajantes { get; set; }

    }
}
