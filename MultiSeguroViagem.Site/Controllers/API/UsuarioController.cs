using AutoMapper;
using MultiSeguroViagem.Common.Helpers;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;
using MultiSeguroViagem.Site.Attributes;
using MultiSeguroViagem.Site.Models.Api;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MultiSeguroViagem.Site.Controllers.API
{
    //[Authorize]
    [DeflateCompression]
    [RoutePrefix("api/v1/usuario")]
    public class UsuarioController : ApiController
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém usuário
        /// </summary>
        /// <remarks>
        /// Obtém o usuário referente ao e-mail informado
        /// </remarks>
        /// <param name="email">E-mail</param>
        /// <returns>Retorna usuário</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Nenhum usuário encontrado</response>
        [ResponseType(typeof(UsuarioModel))]
        public Task<HttpResponseMessage> Get(string email)
        {
            HttpResponseMessage response;

            try
            {
                var usuario = _service.BuscaPorEmail(email);

                response = Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Usuario, UsuarioModel>(usuario));

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, usuario.IdUsuario, EnumStatusTransacao.Normal);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Erro);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Cadastra usuário
        /// </summary>
        /// <remarks>
        /// Cadastrar um novo usuário
        /// </remarks>
        /// <param name="model">Usuário</param>
        /// <response code="200">OK</response>
        /// <response code="400">Não foi possível cadastrar o usuário</response>
        [ResponseType(typeof(UsuarioModel))]
        public Task<HttpResponseMessage> Post(UsuarioPostModel model)
        {
            HttpResponseMessage response;

            try
            {
                _service.Cadastra(model.Nome, model.Email, model.Senha, model.Telefone, model.Documento, model.Cep, model.Endereco, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado);

                response = Request.CreateResponse(HttpStatusCode.Created, new { nome = model.Nome, email = model.Email });

                var location = Url.Link("DefaultApi", new { controller = $"Usuario", email = model.Email });
                response.Headers.Location = new Uri(location);

                // _transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Normal);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Erro);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Atualiza usuário
        /// </summary>
        /// <remarks>
        /// Atualiza usuário
        /// </remarks>
        /// <param name="model">Usuário</param>
        /// <response code="200">OK</response>
        /// <response code="400">Não foi possível atualizar o usuário</response>
        [ResponseType(typeof(UsuarioModel))]
        public Task<HttpResponseMessage> Put(UsuarioPostModel model)
        {
            HttpResponseMessage response;

            try
            {
                _service.Atualiza(model.IdUsuario, model.Nome, model.Email, model.Senha, model.Telefone, model.Documento, model.Cep, model.Endereco, model.Numero, model.Complemento, model.Bairro, model.Cidade, model.Estado);

                response = Request.CreateResponse(HttpStatusCode.OK);

                var location = Url.Link("DefaultApi", new { controller = $"Usuario", email = model.Email });
                response.Headers.Location = new Uri(location);

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Normal);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                // _transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Erro);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Deleta usuário
        /// </summary>
        /// <remarks>
        /// Deleta usuário de acordo com o id informado
        /// </remarks>
        /// <param name="id">Id usuário</param>
        /// <response code="200">OK</response>
        /// <response code="400">Não foi possível deletar o usuário</response>
        public Task<HttpResponseMessage> Delete(int id)
        {
            HttpResponseMessage response;

            try
            {
                _service.Deleta(id);

                response = Request.CreateResponse(HttpStatusCode.OK);

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Normal);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Erro);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Reseta senha
        /// </summary>
        /// <remarks>
        /// Reseta a senha do usuário e envia a nova via e-mail
        /// </remarks>
        /// <param name="email">E-mail</param>
        /// <response code="200">OK</response>
        /// <response code="400">Nenhum usuário encontrado</response>
        /// <response code="500">Erro ao buscar o usuário</response>
        [HttpGet]
        [Route("resetasenha")]
        public Task<HttpResponseMessage> ResetaSenha(string email)
        {
            HttpResponseMessage response;

            try
            {
                var senha = _service.ResetaSenha(email);

                Email.EnviaEmail("contato@multiseguroviagem.com.br", email, "Multi Seguro Viagem - Senha resetada",
                    $"Sua senha foi resetada, utilize a senha {senha} para acessar o sistema.");

                response = Request.CreateResponse(HttpStatusCode.OK);

                var location = Url.Link("DefaultApi", new { controller = $"Usuario", email });
                response.Headers.Location = new Uri(location);

                //_transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Normal);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                // _transacaoService.CadastraTransacao(User.Identity.Name, 0, 0, EnumStatusTransacao.Erro);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
