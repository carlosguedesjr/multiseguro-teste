using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MultiSeguroViagem.Common.Helpers
{
    public static class Funcoes
    {
        private static readonly byte[] _byteSalt = { 2, 1, 2, 1, 0, 0, 5, 0 };
        private const string _chave = @"1'`O(3X~QhBah}cD02rTMA_L?Bj0+T";

        public static DateTime MontaData(string data, string hora)
        {
            return DateTime.ParseExact(string.Concat(data, " ", hora),
                                   "yyyyMMdd HH:mm:ss",
                                    System.Globalization.CultureInfo.InvariantCulture);

        }

        public static string TextoRandom(int tamanho)
        {
            var input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random((int)DateTime.Now.Ticks);
            var chars = Enumerable.Range(0, tamanho)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public static string DigitosRandom(int tamanho)
        {
            var input = "0123456789";
            var random = new Random((int)DateTime.Now.Ticks);
            var chars = Enumerable.Range(0, tamanho)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public static string ToNumbers(this string val)
        {
            if (string.IsNullOrEmpty(val)) return null;

            return Regex.Replace(val, "[^0-9]+", "");
        }
        
        public static string ObtemValorConfig(string chave)
        {
            return System.Configuration.ConfigurationManager.AppSettings[chave] != null ? System.Configuration.ConfigurationManager.AppSettings[chave] : string.Empty;
        }

        /// <summary>
        /// Criptografa um valor com a classe Rijndael 
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Valor criptografado</returns>
        public static string Encrypt(string valor)
        {
            //Declara o byte derivado que será usado pada determinar a chave e o vetor no Rijndael
            var rfcDerivate = new Rfc2898DeriveBytes(_chave, _byteSalt);

            using (var myRijndael = Rijndael.Create())
            {
                myRijndael.Key = rfcDerivate.GetBytes(16);
                myRijndael.IV = rfcDerivate.GetBytes(16);

                // Criptograda a string para um arry de bytes.
                var byteEncrypted = EncryptStringToBytes(valor, myRijndael.Key, myRijndael.IV);

                //Convert para a base 64 para retorno
                return Convert.ToBase64String(byteEncrypted.ToArray());

            }
        }

        /// <summary>
        /// Descriptografa um valor com a classe Rijndael 
        /// </summary>
        /// <param name="valor">Valor</param>
        /// <returns>Valor descriptografado</returns>
        public static string Decrypt(String valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            //Declara o byte derivado que será usado pada determinar a chave e o vetor no Rijndael
            var rfcDerivate = new Rfc2898DeriveBytes(_chave, _byteSalt);

            using (var myRijndael = Rijndael.Create())
            {
                myRijndael.Key = rfcDerivate.GetBytes(16);
                myRijndael.IV = rfcDerivate.GetBytes(16);

                // Descriptografa a string.
                return DecryptStringFromBytes(Convert.FromBase64String(valor), myRijndael.Key, myRijndael.IV);

            }
        }

        /// <summary>
        /// Criptografa a senha de acordo com os parametros key e vetor
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="vetor"></param>
        /// <returns></returns>
        private static IEnumerable<byte> EncryptStringToBytes(string plainText, byte[] key, byte[] vetor)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("O parâmetro plainText está nulo ou inválido");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("O parâmetro key está nulo ou inválido");
            if (vetor == null || vetor.Length <= 0)
                throw new ArgumentNullException("O parâmetro vetor está nulo ou inválido");

            byte[] encrypted;

            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = vetor;

                // Cria o encriptador e transforma em stream 
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Cria o streams usado para criptografia
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static string GeraCpf(bool somenteDigitos = false)
        {
            int somaCalculo;
            int multiplicador;
            int restoDivisao;

            var digitosCpf = DigitosRandom(9);

            for(var i = 0; i < 2; i++)
            {
                somaCalculo = 0;
                multiplicador = 2;
                foreach (char digito in digitosCpf.ToCharArray().Reverse())
                {
                    somaCalculo += multiplicador * int.Parse(digito.ToString());
                    multiplicador++;
                }

                restoDivisao = somaCalculo % 11;

                digitosCpf += restoDivisao < 2 ? "0" : (11 - restoDivisao).ToString();
            }

            if (somenteDigitos)
                return digitosCpf;

            return $"{ digitosCpf.Substring(0, 3)}.{ digitosCpf.Substring(3, 3)}.{ digitosCpf.Substring(6, 3)}-{ digitosCpf.Substring(9, 2)}";
        }


        public static string RemoveAcentuacao(this string stringAcentuada)
        {
            if (string.IsNullOrEmpty(stringAcentuada.Trim()))
                return string.Empty;

            var array = Encoding.GetEncoding("iso-8859-8").GetBytes(stringAcentuada);
            return Encoding.UTF8.GetString(array);
        }

        public static string RemoveEspacos(this string valor)
        {
            return valor.Replace(" ", string.Empty);
        }
    }
}
