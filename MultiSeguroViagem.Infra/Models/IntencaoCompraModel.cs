using System.Collections.Generic;

namespace MultiSeguroViagem.Infra.Models
{
    public class IntencaoCompraModel : IntencaoModel
    {
        public ICollection<IntencaoViajanteModel> viajantes { get; set; }
    }
}
