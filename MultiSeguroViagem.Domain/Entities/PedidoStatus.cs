
namespace MultiSeguroViagem.Domain.Entities
{
    public class PedidoStatus
    {
        #region properties

        public int IdPedidoStatus { get; set; }
        public string Status { get; set; }
        public bool Excluido { get; set; }

        #endregion
    }
}
