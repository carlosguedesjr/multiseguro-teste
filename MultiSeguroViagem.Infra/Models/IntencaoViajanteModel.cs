using System;

namespace MultiSeguroViagem.Infra.Models
{
    public class IntencaoViajanteModel
    {
        public string nome { get; set; }
        public string cpf { get; set; }
        public float valorUnitario { get; set; }
        public DateTime dataNascimento { get; set; }
        public string sexo { get; set; }
        public string operadoraPlano { get; set; }
    }
}
