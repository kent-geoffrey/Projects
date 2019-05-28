using JetBrains.Annotations;

namespace Citi.EqRisk.RiskUI
{
  [UsedImplicitly]
  public sealed class MainWindowPresenter
  {
    [NotNull]
    private readonly MainWindowViewModel _viewModel;

    /// <summary>
    /// Gets the view model of the presenter
    /// </summary>
    public object ViewModel => _viewModel;

    public MainWindowPresenter(IDataGridViewController dataGridViewController_)
    {
      _viewModel = new MainWindowViewModel
      {
        Title = "RiskUI Application",
        DataGridViewController = dataGridViewController_
      };
    }
  }
}
