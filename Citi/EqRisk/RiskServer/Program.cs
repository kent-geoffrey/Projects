using Grpc.Core;
using System;
using Citi.EqRisk.Shared;
using JetBrains.Annotations;
using Unity;

namespace Citi.EqRisk.RiskServer
{
  class Program
  {
    [NotNull]
    private static readonly IUnityContainer _container;

    static Program()
    {
      _container = new UnityContainer();

      SetUpContainer();
    }

    private static void SetUpContainer()
    {
      _container.RegisterType<IPositionDataSource, DummyDataSource>();
      _container.RegisterType<IRiskServerConfigSource, HardcodedConfigSource>();

    }

    static void Main(string[] args)
    {
      var configSource = _container.Resolve<IRiskServerConfigSource>();

      IPositionDataSource positionDataSource = configSource.DataSource;
      positionDataSource.Start();

      var riskDataFeedService = new RiskDataFeedService(configSource.DataSource);

      ServerServiceDefinition riskDataFeedServiceDefinition =
        Riskfeed.RiskFeed.BindService(riskDataFeedService);

      int riskServerPort = configSource.ServerPort;
      var server = new Server
      {
        Services = { riskDataFeedServiceDefinition },
        Ports =
        {
          new ServerPort(
            configSource.ServerHost,
            riskServerPort,
            ServerCredentials.Insecure)
        }
      };

      server.Start();

      Console.WriteLine("RiskServer listening on port " + riskServerPort);
      Console.WriteLine("Press any key to stop the server...");
      Console.ReadKey();

      positionDataSource.Stop();
      server.ShutdownAsync().Wait();
    }
  }
}
