using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace MultiSeguroViagem.Site.Tests.PageObjects
{
    public class ViajantePage
    {
        private readonly ChromeDriver _driver;

        public ViajantePage(ChromeDriver driver)
        {
            _driver = driver;
        }

        public void Adiciona(string nome, string documento, string dataNascimento, string sexo, string contador)
        {
            var txtViajante = _driver.FindElement(By.Id($"Nome{contador}"));
            var txtCpf = _driver.FindElement(By.Id($"Cpf{contador}"));
            var txtDataNascimento = _driver.FindElement(By.Id($"DataNascimento{contador}"));
            var cbSexo = new SelectElement(_driver.FindElement(By.Id($"Sexo{contador}")));

            txtViajante.SendKeys(nome);
            txtDataNascimento.SendKeys(dataNascimento);
            txtCpf.SendKeys(documento);
            cbSexo.SelectByText(sexo);
        }

        public void AdicionaNovoCampo()
        {
            _driver.FindElement(By.Id("adicionarViajante")).Click();
        }

        public PagamentoPage CompletaCompra()
        {
            var completarCompra = _driver.FindElement(By.Id("CompletarCompra"));
            _driver.ExecuteScript("arguments[0].click()", completarCompra);

            Assert.IsTrue(Valida());

            return new PagamentoPage(_driver);
        }

        public bool Valida()
        {
            string[] erros =
            {
                "<span data-valmsg-for=\"Nome1\" class=\"control-label\" data-valmsg-replace=\"true\">Informe o nome</span>",
                "<span data-valmsg-for=\"DataNascimento1\" class=\"control-label\" data-valmsg-replace=\"true\">Informe a data de nascimento</span>",
                "<span data-valmsg-for=\"Cpf1\" class=\"control-label\" data-valmsg-replace=\"true\">Informe o CPF</span>",
                "<span data-valmsg-for=\"Sexo1\" class=\"control-label\" data-valmsg-replace=\"true\">Informe o sexo</span>"
            };

            foreach (var erro in erros)
            {
                var temErro = _driver.PageSource.Contains(erro);

                if (temErro) return false;
            }

            return true;
        }
    }
}
