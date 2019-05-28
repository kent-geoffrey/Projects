using JetBrains.Annotations;

namespace Citi.EqRisk.Shared
{
  /// <summary>
  /// Represents an immutable position data object
  /// </summary>
  public sealed class PositionData
  {
    /// <summary>
    /// Gets the position unique ID
    /// </summary>
    public int PositionId { get; }

    /// <summary>
    /// Gets the position symbol
    /// </summary>
    public string Symbol { get; }

    /// <summary>
    /// Gets the quantity of the position
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the latest live spot price
    /// </summary>
    public double Spot { get; }

    public PositionData(
      int positionId_,
      [NotNull] string symbol_,
      int quantity_,
      double spot_)
    {
      PositionId = positionId_;
      Symbol = symbol_;
      Quantity = quantity_;
      Spot = spot_;
    }
  }
}
