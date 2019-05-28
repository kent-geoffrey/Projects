using Citi.EqRisk.Shared;
using JetBrains.Annotations;

namespace Citi.EqRisk.RiskServer
{
  internal sealed class HardcodedConfigSource : IRiskServerConfigSource
  {
    public string ServerHost { get; }

    public int ServerPort { get; }

    public IPositionDataSource DataSource { get; }

    [UsedImplicitly]
    public HardcodedConfigSource([NotNull] IPositionDataSource positionDataSource_)
    {
      ServerHost = Constants.SERVER_HOST;
      ServerPort = Constants.SERVER_PORT;
      DataSource = positionDataSource_;
    }
  }
}
