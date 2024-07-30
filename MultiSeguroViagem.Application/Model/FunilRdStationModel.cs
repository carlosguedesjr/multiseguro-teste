using MultiSeguroViagem.Domain.Entities.Enum;

namespace MultiSeguroViagem.Application.Model
{
    public class FunilRdStationModel
    {
        public string auth_token { get; set; }
        public FunilLeadRdStationModel lead { get; set; }
    }

    public class FunilLeadRdStationModel
    {
        public FunilLeadRdStationModel(RdStationEstagioLead estagio, bool oportunidade)
        {
            lifecycle_stage = estagio;
            opportunity = oportunidade;
        }

        public RdStationEstagioLead lifecycle_stage { get; set; }
        public bool opportunity { get; set; }
    }
}
