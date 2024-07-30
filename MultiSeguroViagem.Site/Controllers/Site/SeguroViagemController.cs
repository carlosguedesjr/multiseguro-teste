using System;
using System.Web.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Linq;
using System.Collections.Generic;
using MultiSeguroViagem.Site.Models.Site;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Domain.Interfaces.Repositories;

namespace MultiSeguroViagem.Site.Controllers.Site
{
  public class SeguroViagemController : Controller
  {
    private readonly IConfiguracaoHomeRepository _configuracaoHomeRepository;

    public SeguroViagemController( IConfiguracaoHomeRepository configuracaoHomeRepository)
    {
      _configuracaoHomeRepository = configuracaoHomeRepository;
    }
    public ActionResult Index()
    {
      try
      {
        ViewBag.Post = ObtemPostsWordpress();
      }
      catch (Exception)
      {
        ViewBag.Post = null;
      }

      return View();
    }

    public ActionResult SeguroViagemIntercambio()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }

    public ActionResult SeguroViagemPortugues()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }

    public ActionResult SeguroViagemIngles()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }

    public ActionResult SeguroViagemEspanhol()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }

    public ActionResult SeguroViagemInternacional()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }

    public ActionResult SeguroViagemEuropa()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }
    public ActionResult SeguroViagemCovid19()
    {
      var configuracoes = _configuracaoHomeRepository.Busca();
      ViewBag.Configuracoes = configuracoes;
      return View();
    }


    private static IEnumerable<SeguroViagemPostModel> ObtemPostsWordpress()
    {
      var reader = XmlReader.Create("https://blog.multiseguroviagem.com.br/?feed=rss2");
      var feed = SyndicationFeed.Load<SyndicationFeed>(reader);

      var posts = feed.Items.Select(item => new SeguroViagemPostModel(item.Title.Text, item.PublishDate.ToString("dd/MM/yyyy"), item.Id, item.Categories.FirstOrDefault().Name)).ToList();

      return posts.Any() ? posts.Take(5) : null;
    }
  }
}
