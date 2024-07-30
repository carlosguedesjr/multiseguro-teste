
namespace MultiSeguroViagem.Domain.Entities
{
    public class ArquivoUpload
    {
        public int IdPlano { get; set; }
        public int IdPedido { get; set; }
        public int IdFileUploads { get; set; }
        public string TipoArquivo { get; set; }
        public string Arquivo { get; set; }
        public string DataCadastro { get; set; }
    }
}
