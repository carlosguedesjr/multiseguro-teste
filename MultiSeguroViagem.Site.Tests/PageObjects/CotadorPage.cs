using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MultiSeguroViagem.Site.Tests.PageObjects
{
    public class CotadorPage
    {
        private readonly ChromeDriver _driver;

        public CotadorPage(ChromeDriver driver)
        {
            _driver = driver;
        }

        public ViajantePage SelecionaPlano(string idPlano)
        {
            var todosPlanos = _driver.FindElement(By.XPath("//*[@id=\"listaCotacoes\"]"));
            _driver.ExecuteScript($"$(\'body .btn-comprar[value=\"{idPlano}\"]\').trigger(\"click\")", todosPlanos);

            return new ViajantePage(_driver);
        }
    }
}
