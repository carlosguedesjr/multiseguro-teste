using AutoMapper;
using MultiSeguroViagem.Common.Resources;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MultiSeguroViagem.Site.Models.Api;

namespace MultiSeguroViagem.Site.Controllers.API
{
    //[Authorize]
    [DeflateCompression]
    [RoutePrefix("api/v1/cotacao")]
    public class CotacaoController : ApiController
    {
        private readonly ICotacaoService _cotacaoService;
        private readonly IDestinoService _destinoService;
        private readonly ICoberturaService _coberturaService;

        public CotacaoController(IDestinoService destinoService, ICoberturaService coberturaService, ICotacaoService cotacaoService)
        {
            _destinoService = destinoService;
            _coberturaService = coberturaService;
            _cotacaoService = cotacaoService;
        }

        /// <summary>
        /// Cotação
        /// </summary>
        /// <remarks>
        /// Realiza cotação de acordo com os parâmetros informados
        /// </remarks>
        /// <param name="idDestino">Destino da viagem</param>
        /// <param name="dataIda">Data de ida</param>
        /// <param name="dataVolta">Data de volta</param>
        /// <returns>Cotação</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Não foi possível cotar</response>
        [ResponseType(typeof(PlanoModel))]
        public Task<HttpResponseMessage> Get(int idDestino, DateTime dataIda, DateTime dataVolta)
        {
            HttpResponseMessage response;

            try
            {
                var cotacao = _cotacaoService.RealizaCotacao(idDestino, string.Empty, dataIda, dataVolta);

                if (cotacao == null)
                    throw new Exception("Não foi possível realizar esta cotação");

                response = Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Cotacao, CotacaoModel>(cotacao));
            }

            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(ex.Message));
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Destinos
        /// </summary>
        /// <remarks>
        /// Obtém os destinos
        /// </remarks>
        /// <returns>Retorna uma lista de destinos</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Nenhum destino encontrado</response>
        [Route("destino")]
        [ResponseType(typeof(DestinoModel))]
        public Task<HttpResponseMessage> Get()
        {
            HttpResponseMessage response;

            try
            {
                var destino = _destinoService.ObtemDestinos();

                if (destino == null) throw new Exception(DestinoErros.DestinoNaoEncontrado);

                response = Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ICollection<Destino>, ICollection<DestinoModel>>(destino));
            }

            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(ex.Message));
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }


        /// <summary>
        /// Coberturas
        /// </summary>
        /// <remarks>
        /// Obtém as coberturas
        /// </remarks>
        /// <param name="idPlano"></param>
        /// <returns>Retorna uma lista de coberturas</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Nenhum destino encontrado</response>
        [Route("cobertura")]
        [ResponseType(typeof(CoberturaModel))]
        public Task<HttpResponseMessage> Get(int idPlano)
        {
            HttpResponseMessage response;

            try
            {
                var coberturas = _coberturaService.BuscaCoberturas(idPlano);

                if (coberturas == null) throw new Exception(CoberturaErros.CoberturaNaoEncontrada);

                response = Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ICollection<Cobertura>, ICollection<CoberturaModel>>(coberturas));
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
