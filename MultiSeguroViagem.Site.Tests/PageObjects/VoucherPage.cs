
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace MultiSeguroViagem.Site.Tests.PageObjects
{
  public class VoucherPage
  {
    private readonly ChromeDriver _driver;

    public VoucherPage(ChromeDriver driver)
    {
      _driver = driver;
    }

    public ItauPage GeraBoleto()
    {
      Assert.IsFalse(string.IsNullOrEmpty(_driver.FindElement(By.Id("pedido")).Text));
      Assert.IsTrue(_driver.FindElement(By.ClassName("boleto")).Displayed);

      _driver.FindElement(By.ClassName("btn-boleto")).Click();

      return new ItauPage(_driver);
    }

    public void AguardaVoucher()
    {
      var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(40));

      Assert.AreEqual(true, wait.Until(d => d.FindElement(By.ClassName("boleto")).Displayed));
      Assert.IsTrue(_driver.FindElement(By.Id("spViajante")).Displayed);
      Assert.IsTrue(_driver.FindElement(By.ClassName("btn-boleto")).Displayed);
      Assert.IsFalse(string.IsNullOrEmpty(_driver.FindElement(By.Id("pedido")).Text));

      _driver.Close();
      _driver.Quit();
    }
  }
}
