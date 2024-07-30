using MultiSeguroViagem.Common.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MultiSeguroViagem.Site.Helpers
{
    public class Cookie : Controller
    {
        /// <summary>
        ///  Cria Cookie
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="valor"></param>
        /// <param name="segundos"></param>
        /// <param name="response"></param>
        public void SetCookie(string nome, string valor, int segundos, HttpResponseBase response)
        {

            var ticket = new FormsAuthenticationTicket(Constantes.VERSAO_COOKIE,
                                                         valor,
                                                         DateTime.Now,
                                                         DateTime.Now.AddSeconds(segundos),
                                                         false,
                                                         string.Empty,
                                                         FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
           var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            response.Cookies.Add(new HttpCookie(nome, encTicket));
        }



        /// <summary>
        ///  Remove Cookie
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="response"></param>
        public void RemoveCookie(string nome, HttpResponseBase response)
        {
            if (response.Cookies[nome] != null)
            {
                var cookie = response.Cookies[nome];
                response.Cookies.Remove(nome);
                cookie.Expires = DateTime.Now.AddDays(-10);
                cookie.Value = null;
                response.Cookies.Add(cookie);
            }          
        }
    }
}