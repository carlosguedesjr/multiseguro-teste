using System;

namespace MultiSeguroViagem.Infra.Models
{
    public class IntencaoModel
    {
        public IntencaoModel()
        {
            dataCriacao = DateTime.Now;
        }

        public string destino { get; set; }
        public DateTime dataIda { get; set; }
        public DateTime dataVolta { get; set; }
        public string email { get; set; }
        public string nome { get; set; }
        public string telefone { get; set; }
        public string origem { get; set; }
        public string referrer { get; set; }
        public string ip { get; set; }
        public DateTime dataCriacao { get; set; }
    }
}
