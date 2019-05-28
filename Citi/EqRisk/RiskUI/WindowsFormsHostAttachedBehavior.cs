using System.Windows;
using System.Windows.Forms.Integration;

namespace Citi.EqRisk.RiskUI
{
  /// <summary>
  /// Hosts the WinForm control of the assigned DataGrid View Controller
  /// </summary>
  public static class WindowsFormsHostAttachedBehavior
  {
    public static readonly DependencyProperty DataGridViewControllerProperty =
      DependencyProperty.RegisterAttached(
        "DataGridViewController",
        typeof(IDataGridViewController),
        typeof(WindowsFormsHostAttachedBehavior),
        new UIPropertyMetadata(null, OnDataGridViewControllerChanged));

    public static IDataGridViewController GetDataGridViewController(WindowsFormsHost host_)
    {
      return (IDataGridViewController)host_.GetValue(DataGridViewControllerProperty);
    }

    public static void SetDataGridViewController(
      WindowsFormsHost host_, IDataGridViewController value_)
    {
      host_.SetValue(DataGridViewControllerProperty, value_);
    }

    static void OnDataGridViewControllerChanged(
      DependencyObject obj_,
      DependencyPropertyChangedEventArgs e_)
    {
      WindowsFormsHost host = obj_ as WindowsFormsHost;
      if (host == null) return;

      var dataGridViewController = e_.NewValue as IDataGridViewController;
      if (dataGridViewController == null) return;

      host.Child = dataGridViewController.View;
    }

  }
}
