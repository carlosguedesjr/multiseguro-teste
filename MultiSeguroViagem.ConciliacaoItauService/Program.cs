using System.ServiceProcess;
using Microsoft.Practices.Unity;
using MultiSeguroViagem.IoC;

namespace MultiSeguroViagem.ConciliacaoItauService
{
  static class Program
  {
    static void Main()
    {
      var container = new UnityContainer();
      DependencyResolver.ConciliacaoBoletoResolve(container);

      var servicesToRun = new ServiceBase[]
      {
        container.Resolve<ConsultaBoletosService>()
      };

      ServiceBase.Run(servicesToRun);
    }
  }
}
