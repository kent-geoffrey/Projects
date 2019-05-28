namespace Citi.EqRisk.RiskUI
{
  public sealed class MainWindowViewModel : ViewModelBase
  {
    private string _title;
    /// <summary>
    /// Gets the window title text
    /// </summary>
    public string Title
    {
      get => _title;
      set
      {
        if (value == _title) return;

        _title = value;
        OnPropertyChanged();
      }
    }

    private IDataGridViewController _dataGridViewController;
    public IDataGridViewController DataGridViewController
    {
      get => _dataGridViewController;
      set
      {
        if (Equals(value, _dataGridViewController)) return;

        _dataGridViewController = value;
        OnPropertyChanged();
      }
    }
  }
}
