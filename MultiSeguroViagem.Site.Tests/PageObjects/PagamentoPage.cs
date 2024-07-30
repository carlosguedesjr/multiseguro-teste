using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace MultiSeguroViagem.Site.Tests.PageObjects
{
    public class PagamentoPage
    {
        private readonly ChromeDriver _driver;

        public PagamentoPage(ChromeDriver driver)
        {
            _driver = driver;
        }

        public void ConsultaCep(string cep)
        {
            _driver.FindElement(By.Id("Cep")).SendKeys(cep);
            _driver.FindElement(By.Id("Numero")).SendKeys("1");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10000));
            Assert.AreEqual(true, wait.Until(d => d.FindElement(By.Id("Endereco")).Displayed));

            Assert.IsTrue(ValidaCep());
        }

        public void InformacoesPessoais(string nomeCompleto, string documento, string telefonePessoal, string nomeContato, string telefoneContato, string metodoPagamento)
        {
            _driver.ExecuteScript("window.scrollTo(158, 115)");

            var cbMetodoPagamento = new SelectElement(_driver.FindElement(By.Id("OperadoraPagamento")));

            cbMetodoPagamento.SelectByText(metodoPagamento);
            _driver.FindElement(By.Id("NomeCompleto")).SendKeys(nomeCompleto);
            _driver.FindElement(By.Id("Documento")).SendKeys(documento);
            _driver.FindElement(By.Id("TelefonePessoal")).SendKeys(telefonePessoal);
            _driver.FindElement(By.Id("NomeContato")).SendKeys(nomeContato);
            _driver.FindElement(By.Id("TelefoneContato")).SendKeys(telefoneContato);
        }

        public void FinalizaCompraCartao()
        {
            var cbParcelas = new SelectElement(_driver.FindElement(By.Id("Parcelas")));
            cbParcelas.SelectByValue("00");

            _driver.FindElement(By.Id("NumeroCartao")).SendKeys("4002 4791 9957 0736");
            _driver.FindElement(By.Id("NomeCartao")).SendKeys("Teste Teste");
            _driver.FindElement(By.Id("MesAno")).SendKeys("02/19");
            _driver.FindElement(By.Id("CodigoSeguranca")).SendKeys("132");

            _driver.ExecuteScript("$('#hdValorTotal').val(arguments[0])", "5111.00");

            FinalizaCompra();

            _driver.Close();
            _driver.Quit();
        }

        public VoucherPage FinalizaCompra()
        {
            var checks = _driver.FindElementsByClassName("check");
            checks[0].Click();

            _driver.ExecuteScript("$('body #btnFinalizar').trigger('click')");

            Assert.IsTrue(Valida());

            return new VoucherPage(_driver);
        }
        
        public bool Valida()
        {
            string[] erros =
            {
                "<span class=\"control-label\" data-valmsg-for=\"NomeCompleto\" data-valmsg-replace=\"true\">Informe o nome</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Documento\" data-valmsg-replace=\"true\">Informe CPF/CNPJ</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Email\" data-valmsg-replace=\"true\">Informe o e-mail</span>",
                "<span class=\"control-label\" data-valmsg-for=\"TelefonePessoal\" data-valmsg-replace=\"true\">Informe o telefone</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Cep\" data-valmsg-replace=\"true\">Informe o CEP</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Endereco\" data-valmsg-replace=\"true\">Informe o endereço</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Numero\" data-valmsg-replace=\"true\">Informe o número</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Bairro\" data-valmsg-replace=\"true\">Informe o bairro</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Estado\" data-valmsg-replace=\"true\">Informe o estado</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Cidade\" data-valmsg-replace=\"true\">Informe a cidade</span>",
                "<span class=\"control-label\" data-valmsg-for=\"NomeContato\" data-valmsg-replace=\"true\">Informe o nome de contato</span>",
                "<span class=\"control-label\" type=\"tel\" data-valmsg-for=\"TelefoneContato\" data-valmsg-replace=\"true\">Informe o número de contato</span>",
                "<span class=\"control-label\" data-valmsg-for=\"OperadoraPagamento\" data-valmsg-replace=\"true\">Informe o método de pagamento</span>",
                "<span class=\"control-label\" data-valmsg-for=\"cbkViajantesBrasil\" data-valmsg-replace=\"true\">É necessário concordar com o termo</span>",
                "<span class=\"control-label\" data-valmsg-for=\"cbkTermos\" data-valmsg-replace=\"true\">É necessário concordar com os termos</span>",
                "<span class=\"control-label\" type=\"tel\" data-valmsg-for=\"TelefoneContato\" data-valmsg-replace=\"true\">O telefone do contato deve ser diferente do telefone pessoal</span>",
                "<span class=\"control-label\" data-valmsg-for=\"NomeCompleto\" data-valmsg-replace=\"true\">Digite o nome completo</span>",
                "<span class=\"control-label\" data-valmsg-for=\"NomeContato\" data-valmsg-replace=\"true\">Digite o nome completo</span>",
                "Falha na comunicacao. Tente novamente.",
                "Transacao nao autorizada. Contate o emissor",
                "Formato do campo Nrcartao inv"
            };

            foreach (var erro in erros)
            {
                var temErro = _driver.PageSource.Contains(erro);

                if (temErro) return false;
            }

            return true;
        }

        public bool ValidaCep()
        {
            string[] erros =
            {
                "<span class=\"control-label\" data-valmsg-for=\"Cep\" data-valmsg-replace=\"true\">Informe o CEP</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Endereco\" data-valmsg-replace=\"true\">Informe o endereço</span>",
                //"<span class=\"control-label\" data-valmsg-for=\"Numero\" data-valmsg-replace=\"true\">Informe o número</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Bairro\" data-valmsg-replace=\"true\">Informe o bairro</span>",
                "<span class=\"control-label\" data-valmsg-for=\"Estado\" data-valmsg-replace=\"true\">Selecione o estado </span>",
                "<span class=\"control-label\" data-valmsg-for=\"Cidade\" data-valmsg-replace=\"true\">Selecione uma cidade</span>"
            };

            foreach (var erro in erros)
            {
                var temErro = _driver.PageSource.Contains(erro);

                if (temErro)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
