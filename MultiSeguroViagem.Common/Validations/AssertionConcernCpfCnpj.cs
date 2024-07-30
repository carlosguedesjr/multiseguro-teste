using System;

namespace MultiSeguroViagem.Common.Validations
{
    public class AssertionConcernCpfCnpj
    {
        public static void Valida(string documento)
        {
            documento = documento.Trim();
            documento = documento.Replace(".", "").Replace("-", "").Replace("/", "");

            if (documento.Length < 12)
            {
                ValidaCpf(documento);

                return;
            }

            ValidaCnpj(documento);
        }

        private static void ValidaCpf(string cpf)
        {
            var valor = cpf.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                throw new InvalidOperationException("CPF não contém 11 dígitos numéricos");

            var igual = true;

            for (var i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                throw new InvalidOperationException("CPF inválido");

            var numeros = new int[11];
            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    throw new InvalidOperationException("CPF inválido");
            }

            else if (numeros[9] != 11 - resultado)
                throw new InvalidOperationException("CPF inválido");

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    throw new InvalidOperationException("CPF inválido");
            }

            else
            if (numeros[10] != 11 - resultado)
                throw new InvalidOperationException("CPF inválido");
        }

        private static void ValidaCnpj(string cnpj)
        {
            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            //cnpj = cnpj.Trim();
            //cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                throw new InvalidOperationException("CNPJ não contém 14 dígitos numéricos");

            var tempCnpj = cnpj.Substring(0, 12);

            var soma = 0;
            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            var resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto;

            if (!cnpj.EndsWith(digito))
                throw new InvalidOperationException("CNPJ inválido");
        }
    }
}