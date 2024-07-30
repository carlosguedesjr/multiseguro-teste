namespace MultiSeguroViagem.Site.Models.Site.Login
{
    public class LoginCadastroModel
    {
        public LoginCadastroModel()
        {
            Cadastro = new CadastroModel();
            Login = new LoginModel();
        }

        public CadastroModel Cadastro { get; set; }
        public LoginModel Login { get; set; }
    }
}