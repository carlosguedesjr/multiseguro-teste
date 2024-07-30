using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MultiSeguroViagem.Site.Tests.PageObjects
{
    public class ItauPage
    {
        private readonly ChromeDriver _driver;

        public ItauPage(ChromeDriver driver)
        {
            _driver = driver;
        }

        public void ImprimirBoleto()
        {
            _driver.FindElement(By.Id("rdbOpcao")).Click();

            _driver.FindElement(By.Id("boleto")).Click();
        }

        public void FechaGuiaBoleto()
        {
            var guiaBoleto = _driver.WindowHandles[1];

            Assert.IsTrue(!string.IsNullOrEmpty(guiaBoleto));
            Assert.AreEqual(_driver.SwitchTo().Window(guiaBoleto).Url, "https://shopline.itau.com.br/shopline/Impressao.aspx");

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));

            _driver.SwitchTo().Window(_driver.WindowHandles[1]).Close();
            _driver.SwitchTo().Window(_driver.WindowHandles[0]);
        }
    }
}
