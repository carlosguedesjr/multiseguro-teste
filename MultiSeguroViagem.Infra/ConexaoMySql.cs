using MySql.Data.MySqlClient;

namespace MultiSeguroViagem.Infra
{
    public class ConexaoMySql
    {
        private readonly MySqlConnection _cnx;

        public ConexaoMySql()
        {
            //var cnx = System.Configuration.ConfigurationManager.ConnectionStrings["MultiSeguro"].ConnectionString;
            //_cnx = new MySqlConnection(cnx);
            //_cnx.Open();
        }

        public MySqlConnection Get()
        {
            return _cnx;
        }
    }
}
