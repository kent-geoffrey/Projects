using Citi.EqRisk.RiskUI;
using NUnit.Framework;

namespace Citi.EqRisk.UnitTests.RiskUITests
{
  public class GridRowViewModelTests
  {
    [TestCase(20, 2d, 40d)]
    [TestCase(4, 25d, 100d)]
    public void PositionPropertyTests(int quantity_, double spot_, double expectedPosition_)
    {
      var sut = new GridRowViewModel(3)
      {
        Symbol = "AAPL",
        Quantity = quantity_,
        Spot = spot_
      };

      Assert.AreEqual(expectedPosition_, sut.Position);
    }

    [TestCase(20, 2d, .40d)]
    [TestCase(4, 25d, 1.00d)]
    public void DeltaPropertyTests(int quantity_, double spot_, double expectedDelta_)
    {
      var sut = new GridRowViewModel(3)
      {
        Symbol = "AAPL",
        Quantity = quantity_,
        Spot = spot_
      };

      Assert.AreEqual(expectedDelta_, sut.Delta);
    }
  }
}
