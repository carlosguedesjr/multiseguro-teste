
namespace MultiSeguroViagem.Service.Models
{
    public class ViajanteModel
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Sexo { get; set; }
        public string DataNascimento { get; set; }
        public string TipoDocumento { get; set; } // Codigo 1 = Passaporte | Codigo 9 = CPF
        public string NumeroDocumento { get; set; }
        public string Endereco { get; set; }       
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string PaisDomicilio { get; set; } // 550 = Brasil
        public string ContatoEmergencia { get; set; }
        public string ContatoEmergenciaTelefone { get; set; }
        public string ContatoEmergenciaEndereco { get; set; }
     
    }
}
