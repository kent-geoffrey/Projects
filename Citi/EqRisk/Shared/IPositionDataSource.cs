using System;
using JetBrains.Annotations;

namespace Citi.EqRisk.Shared
{
  public interface IPositionDataSource
  {
    /// <summary>
    /// Gets live position data feed
    /// </summary>
    [NotNull]
    IObservable<PositionData> Live { get; }

    /// <summary>
    /// Gets the latest position data snapshot
    /// </summary>
    [NotNull]
    IObservable<PositionData> Snapshot { get; }

    /// <summary>
    /// Start producing live data
    /// </summary>
    void Start();

    /// <summary>
    /// Stop producing live data
    /// </summary>
    void Stop();
  }
}
