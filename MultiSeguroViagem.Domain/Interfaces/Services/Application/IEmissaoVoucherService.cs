using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;

namespace MultiSeguroViagem.Domain.Interfaces.Services.Application
{
  public interface IEmissaoVoucherService
  {
    void EmissaoVoucher(Pagamento pagamento, IList<Viajante> viajantes, ContatoEmergencia contatoEmergencia);

    void VerificaPagamentoDiaUtil(Pagamento pagamento);
  }
}
