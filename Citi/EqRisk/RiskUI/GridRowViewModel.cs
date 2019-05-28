namespace Citi.EqRisk.RiskUI
{
  public class GridRowViewModel : ViewModelBase
  {
    private string _symbol;
    private int _quantity;
    private double _spot;
    public int PositionId { get; }

    public string Symbol
    {
      get => _symbol;
      set
      {
        if (value == _symbol) return;
        _symbol = value;
        OnPropertyChanged();
      }
    }

    public int Quantity
    {
      get => _quantity;
      set
      {
        if (value == _quantity) return;
        _quantity = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(Delta));
      }
    }

    public double Spot
    {
      get => _spot;
      set
      {
        if (value.Equals(_spot)) return;
        _spot = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(Delta));
      }
    }

    public double Position => Spot * Quantity;

    public double Delta => Position * 0.01d;

    public GridRowViewModel(int positionId_)
    {
      PositionId = positionId_;
    }
  }
}
