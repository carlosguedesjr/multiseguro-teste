using System;
using System.Collections.Generic;

namespace MultiSeguroViagem.Site.Models.Site.Cotacao
{
    public class CotacaoModel
    {
        public DateTime DataIda { get; set; }
        public DateTime DataVolta { get; set; }
        public int IdDestino { get; set; }
        public ICollection<PlanoModel> Planos { get; set; }
    }
}