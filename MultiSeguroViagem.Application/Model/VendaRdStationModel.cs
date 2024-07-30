namespace MultiSeguroViagem.Application.Model
{
    public class VendaRdStationModel
    {
        public VendaRdStationModel(decimal value, string email)
        {
            status = "won";
            this.value = value;
            this.email = email;
        }

        public string status { get; set; }
        public decimal value { get; set; }
        public string email { get; set; }
    }
}
