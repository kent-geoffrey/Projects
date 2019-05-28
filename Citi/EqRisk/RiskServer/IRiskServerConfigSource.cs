using Citi.EqRisk.Shared;
using JetBrains.Annotations;

namespace Citi.EqRisk.RiskServer
{
  /// <summary>
  /// Represents a source from which RiskServer Configuration information can be obtained
  /// </summary>
  public interface IRiskServerConfigSource
  {
    /// <summary>
    /// Gets the host name
    /// </summary>
    [NotNull]
    string ServerHost { get; }

    /// <summary>
    /// Gets the host port
    /// </summary>
    int ServerPort { get; }

    /// <summary>
    /// Gets the configured source for the position risk data
    /// </summary>
    [NotNull]
    IPositionDataSource DataSource { get; }
  }
}
