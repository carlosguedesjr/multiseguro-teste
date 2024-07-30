
namespace MultiSeguroViagem.Domain.Entities
{
    public class Preco
    {

        public int Dias { get; set; }

        public Plano Plano { get; set; }

        public decimal Valor { get; set; }

    }
}
