namespace MultiSeguroViagem.Site.Models.Api
{
    public class CupomDescontoModel
    {
        public string TipoDesconto { get; set; }
        public decimal ValorDesconto { get; set; }
        public bool ZerarComissao { get; set; }
    }
}