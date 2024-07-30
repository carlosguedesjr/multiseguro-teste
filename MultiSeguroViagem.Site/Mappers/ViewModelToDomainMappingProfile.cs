using AutoMapper;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Site.Models.Site.Viajante;

namespace MultiSeguroViagem.Site.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName => "ViewModelToDomainMappings";

        public ViewModelToDomainMappingProfile()
        {
            // Site
            CreateMap<Models.Site.PlanoModel, Plano>();
            CreateMap<Models.Site.OperadoraModel, Operadora>();
            CreateMap<Models.Site.DestinoModel, Destino>();
            CreateMap<Models.Site.CoberturaModel, Cobertura>();
            CreateMap<Models.Site.RealSegurosModel, RealSeguros>();
            CreateMap<Models.Site.Pagamento.PagamentoModel, Pagamento>();
            CreateMap<ViajanteModel, Viajante>();
            CreateMap<Models.Site.ArquivoUploadModel, ArquivoUpload>();
            CreateMap<Models.Site.UsuarioModel, Usuario>();
            CreateMap<Models.Site.PedidoModel, Pedido>();
            CreateMap<Models.Site.PedidoItemModel, PedidoItem>();
            CreateMap<Models.Site.PedidoStatusModel, PedidoStatus>();

            // API
            CreateMap<Models.Api.PlanoModel, Plano>();
            CreateMap<Models.Api.CotacaoModel, Cotacao>();
            CreateMap<Models.Api.PrecoModel, Preco>();
            CreateMap<Models.Api.CupomDescontoModel, CupomDesconto>();
            CreateMap<Models.Api.UsuarioModel, Usuario>();
            CreateMap<Models.Api.OperadoraModel, Operadora>();
            CreateMap<Models.Api.PlanoStatusModel, PlanoStatus>();
            CreateMap<Models.Api.DestinoModel, Destino>();
            CreateMap<Models.Api.CoberturaModel, Cobertura>();
            CreateMap<Models.Api.ArquivoUploadModel, ArquivoUpload>();

        }
    }
}