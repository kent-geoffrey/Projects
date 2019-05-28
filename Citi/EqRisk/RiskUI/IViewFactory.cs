using System.Windows;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace Citi.EqRisk.RiskUI
{
  /// <summary>
  /// Represents an instance of a view factory
  /// </summary>
  public interface IViewFactory
  {
    [NotNull]
    Window CreateMainWindow();

    [NotNull]
    DataGridView CreateDataGridView();
  }
}
