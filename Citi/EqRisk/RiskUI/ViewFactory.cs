using System.Windows;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace Citi.EqRisk.RiskUI
{
  /// <summary>
  /// Represents a factory that can prepare various views for the application
  /// </summary>
  [UsedImplicitly]
  public sealed class ViewFactory : IViewFactory
  {
    public Window CreateMainWindow()
    {
      return new MainWindow();
    }

    public DataGridView CreateDataGridView()
    {
      return new DataGridView();
    }
  }
}
