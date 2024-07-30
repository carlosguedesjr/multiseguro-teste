using Microsoft.VisualStudio.TestTools.UnitTesting;
using static MultiSeguroViagem.Common.Helpers.Funcoes;
using MultiSeguroViagem.Site.Tests.PageObjects;
using OpenQA.Selenium.Chrome;

namespace MultiSeguroViagem.Site.Tests
{
    /// <summary>
    /// Testa o processo de checkout do site
    /// </summary>
    [TestClass]
    public class CheckoutTests
    {
        private readonly HomePage _home;

        public CheckoutTests()
        {
            _home = new HomePage(new ChromeDriver());
        }

        [TestMethod]
        public void CheckoutComBoleto()
        {
            var pagamento = CotarEFinalizarCompra("Boleto bancário");

            var voucher = pagamento.FinalizaCompra();

            var itau = voucher.GeraBoleto();
            
            itau.ImprimirBoleto();

            itau.FechaGuiaBoleto();

            voucher.AguardaVoucher();
           
        }

        [TestMethod]
        public void CheckoutComCartaoCredito()
        {
            var pagamento = CotarEFinalizarCompra("Cartão de crédito");

            pagamento.FinalizaCompraCartao();
        }

        private PagamentoPage CotarEFinalizarCompra(string formaPagamento)
        {
            _home.Visita();

            _home.BuscaPlanos();

            var cotador = _home.VerResultados();

            var viajante = cotador.SelecionaPlano("441");

            viajante.Adiciona("Viajante Um", GeraCpf(), "13/04/1991", "Masculino", "1");

            viajante.AdicionaNovoCampo();

            viajante.Adiciona("Viajante Dois", GeraCpf(), "13/04/1937", "Feminino", "2");

            var pagamento = viajante.CompletaCompra();

            pagamento.ConsultaCep("13106-000");

            pagamento.InformacoesPessoais("Teste teste", GeraCpf(), "(19) 99999-9999", "Nome Contato", "(19) 9999-9999", formaPagamento);

            return pagamento;
        }
    }
}