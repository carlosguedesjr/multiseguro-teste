using AutoMapper;
using MultiSeguroViagem.Common.Validations;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Attributes;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MultiSeguroViagem.Site.Models.Api;

namespace MultiSeguroViagem.Site.Controllers.API
{

    [DeflateCompression]
    [RoutePrefix("api/v1/cupomdesconto")]

    public class CupomDescontoController : ApiController
    {
        private readonly ICupomDescontoService _service;

        public CupomDescontoController(ICupomDescontoService service)
        {
            _service = service;
        }

        public Task<HttpResponseMessage> Get(string documento, string codigo, decimal valorCompra, string operadora)
        {
            HttpResponseMessage response;
            documento = documento.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            try
            {
                AssertionConcernCpfCnpj.Valida(documento);

                var cupom = _service.BuscaCupom(documento, codigo, valorCompra, operadora);

                response = Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<CupomDesconto, CupomDescontoModel>(cupom));
            }

            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(ex.Message));
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }

}
