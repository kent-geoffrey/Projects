using System;
using System.Threading.Tasks;

namespace Citi.EqRisk.Shared
{
  /// <summary>
  /// Represents a client that can connect to a remote RiskFeed source
  /// </summary>
  public interface IRiskServerClient
  {
    /// <summary>
    /// Gets the connection state
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Gets an observable of position data
    /// </summary>
    IObservable<PositionData> PositionDataObservable { get; }

    /// <summary>
    /// Connect to remote server to start receiving feed
    /// </summary>
    /// <returns></returns>
    Task Connect();
  }
}
