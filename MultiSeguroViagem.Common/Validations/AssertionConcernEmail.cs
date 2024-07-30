using System;
using System.Text.RegularExpressions;
using MultiSeguroViagem.Common.Resources;

namespace MultiSeguroViagem.Common.Validations
{
    public class AssertionConcernEmail
    {
        public static void AssertEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new InvalidOperationException(UsuarioErros.EmailNaoPodeSerNulo);

            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                throw new InvalidOperationException(UsuarioErros.EmailInvalido);
        }
    }
}