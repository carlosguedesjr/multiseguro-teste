using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Models.Site;

namespace MultiSeguroViagem.Site.Controllers.Site
{
    public class ViajanteController : Controller
    {
        private readonly IIntencaoService _intencaoService;

        public ViajanteController(IIntencaoService intencaoService)
        {
            _intencaoService = intencaoService;
        }

        public ActionResult Viajantes()
        {
            return View();
        }

        [HttpPost]
        public void RegistraIntencaoCompra(IntencaoCompraModel model)
        {
            var culture = new CultureInfo("en-US");

            var viajantes =  new List<Viajante>();

            foreach (var viajante in model.Viajantes)
            {
                var viaj = new Viajante(viajante.Nome,
                                        viajante.Cpf,
                                        Convert.ToDateTime(viajante.DataNascimento),
                                        viajante.Sexo.Equals("M") ? 1 : 2,
                                        Convert.ToDecimal(viajante.ValorUnitario, culture),
                                        viajante.Plano);

                viajantes.Add(viaj);
            }
            
            _intencaoService.Cadastra(model.Destino, Convert.ToDateTime(model.DataIda), Convert.ToDateTime(model.DataVolta), model.Email, model.Nome, model.Telefone, model.Origem, model.Referrer, model.Ip, viajantes);
        }
    }
}