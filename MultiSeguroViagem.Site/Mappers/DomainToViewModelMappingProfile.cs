using AutoMapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Site.Models.Site.Viajante;

namespace MultiSeguroViagem.Site.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName => "DomainToViewModelMappings";

        public DomainToViewModelMappingProfile()
        {
            // Site
            CreateMap<Plano, Models.Site.PlanoModel>();
            CreateMap<Operadora, Models.Site.OperadoraModel>();
            CreateMap<Destino, Models.Site.DestinoModel>();
            CreateMap<Cobertura, Models.Site.CoberturaModel>();
            CreateMap<RealSeguros, Models.Site.RealSegurosModel>();
            CreateMap<Pagamento, Models.Site.Pagamento.PagamentoModel>();
            CreateMap<Viajante, ViajanteModel>();
            CreateMap<ArquivoUpload, Models.Site.ArquivoUploadModel>();
            CreateMap<Usuario, Models.Site.UsuarioModel>();
            CreateMap<Pedido, Models.Site.PedidoModel>();
            CreateMap<PedidoItem, Models.Site.PedidoItemModel>();
            CreateMap<PedidoStatus, Models.Site.PedidoStatusModel>();

            // API
            CreateMap<Plano, Models.Api.PlanoModel>();
            CreateMap<Cotacao, Models.Api.CotacaoModel>();         
            CreateMap<Preco, Models.Api.PrecoModel>();      
            CreateMap<CupomDesconto, Models.Api.CupomDescontoModel>();
            CreateMap<Usuario, Models.Api.UsuarioModel>();
            CreateMap<Operadora, Models.Api.OperadoraModel>();
            CreateMap<PlanoStatus, Models.Api.PlanoStatusModel>();
            CreateMap<Destino, Models.Api.DestinoModel>();
            CreateMap<Cobertura, Models.Api.CoberturaModel>();
            CreateMap<ArquivoUpload, Models.Api.ArquivoUploadModel>();
        }
    }
}