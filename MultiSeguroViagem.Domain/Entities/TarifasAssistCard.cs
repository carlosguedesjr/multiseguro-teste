
namespace MultiSeguroViagem.Domain.Entities
{
    public class TarifasAssistCard
    {
        #region Propriedades

        public int IdTarifa { get; private set; }
        public int IdPlano { get; private set; }
        public int DiaDe { get; private set; }
        public int DiaAte { get; private set; }
        public string Codigo { get; private set; }


        #endregion
    }
}
