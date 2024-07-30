using MultiSeguroViagem.Common.Resources;

namespace MultiSeguroViagem.Common.Validations
{
    public class SenhaAssertionConcern
    {
        public static void AssertIsValid(string password)
        {
            AssertionConcern.AssertArgumentNotNull(password, UsuarioErros.SenhaInvalida);
        }
    }
}