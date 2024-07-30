using AutoMapper;

namespace MultiSeguroViagem.Site.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(conf =>
            {
                conf.AddProfile<DomainToViewModelMappingProfile>();
                conf.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }
}