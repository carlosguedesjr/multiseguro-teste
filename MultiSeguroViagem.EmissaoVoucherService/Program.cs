using Microsoft.Practices.Unity;
using MultiSeguroViagem.IoC;
using System.Diagnostics;
using System.ServiceProcess;

namespace MultiSeguroViagem.EmissaoVoucherService
{
  static class Program
  {
    private static void Main()
    {
      Debugger.Break();
      var container = new UnityContainer();
      DependencyResolver.ServicoResolve(container);

      var servicesToRun = new ServiceBase[]
      {
            container.Resolve<EmissaoVoucherService>()
      };
      ServiceBase.Run(servicesToRun);
    }
  }
}
