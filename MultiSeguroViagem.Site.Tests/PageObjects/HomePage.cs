using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

namespace MultiSeguroViagem.Site.Tests.PageObjects
{
  public class HomePage
  {
    private readonly ChromeDriver _driver;

    public HomePage(ChromeDriver driver)
    {
      _driver = driver;
    }

    public void Visita()
    {
      _driver.Navigate().GoToUrl("https://hom.multiseguroviagem.com.br");
      _driver.Manage().Window.Maximize();
    }

    public void BuscaPlanos()
    {
      var destino = new SelectElement(_driver.FindElement(By.Id("Destino")));
      destino.SelectByText("América Central");

      var dataIda = _driver.FindElement(By.Id("DataIda"));
      _driver.ExecuteScript($"$('#DataIda').val('{ DateTime.Now.Date.AddDays(11):dd/MM/yyyy}')", dataIda);
      //dataIda.Clear();
      //dataIda.SendKeys(DateTime.Now.Date.AddDays(11).ToString("dd/MM/yyyy"));

      var dataVolta = _driver.FindElement(By.Id("DataVolta"));
      _driver.ExecuteScript($"$('#DataVolta').val('{ DateTime.Now.Date.AddDays(11):dd/MM/yyyy}')", dataVolta);
      // dataVolta.Clear();
      // dataVolta.SendKeys(DateTime.Now.Date.AddDays(11).ToString("dd/MM/yyyy"));

      _driver.FindElement(By.Id("btnCotar")).Click();

      Assert.IsTrue(Valida());
    }

    public CotadorPage BuscaPlanosPromocoesHome()
    {
      _driver.FindElement(By.ClassName("promo-home")).FindElement(By.ClassName("btn")).Click();

      Assert.AreEqual(true, new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(d => d.FindElement(By.Id("DataIdaBanner")).Displayed));

      var DataIdaBanner = _driver.FindElement(By.Id("DataIdaBanner"));
      DataIdaBanner.Clear();
      DataIdaBanner.SendKeys(DateTime.Now.Date.AddDays(11).ToString("dd/MM/yyyy"));

      var DataVoltaBanner = _driver.FindElement(By.Id("DataVoltaBanner"));
      DataVoltaBanner.Clear();
      DataVoltaBanner.SendKeys(DateTime.Now.Date.AddDays(11).ToString("dd/MM/yyyy"));

      var EmailBanner = _driver.FindElement(By.Id("EmailBanner"));
      EmailBanner.SendKeys("desenvolvimento@cenariocapital.com.br");

      _driver.FindElement(By.Id("frmCotacaoModal")).Submit();

      Assert.IsTrue(Valida());

      _driver.Close();
      _driver.Quit();

      return new CotadorPage(_driver);
    }

    public CotadorPage VerResultados()
    {
      var email = _driver.FindElement(By.Id("Email"));
      email.SendKeys("desenvolvimento@cenariocapital.com.br");

      _driver.FindElement(By.Id("frmCotacao")).Submit();

      Assert.IsTrue(Valida());

      return new CotadorPage(_driver);
    }

    public bool Valida()
    {
      string[] erros = { "<span class=\"control-label\" data-valmsg-for=\"Destino\" data-valmsg-replace=\"true\">Selecione um destino</span>",
                               "<span class=\"control-label\" data-valmsg-for=\"DataIda\" data-valmsg-replace=\"true\">Selecione a data de ida</span>",
                               "<span class=\"control-label\" data-valmsg-for=\"DataVolta\" data-valmsg-replace=\"true\">Selecione a data de volta</span>",
                               "<span class=\"control-label\" data-valmsg-for=\"Email\" data-valmsg-replace=\"true\">Informe seu e-mail</span>",
                               "<span class=\"control-label\" data-valmsg-for=\"Email\" data-valmsg-replace=\"true\">The Email field is not a valid e-mail address.</span>" };

      foreach (var erro in erros)
      {
        var temErro = _driver.PageSource.Contains(erro);

        if (temErro) return false;
      }

      return true;
    }
  }
}
