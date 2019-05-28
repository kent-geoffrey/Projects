using System.Windows;
using Citi.EqRisk.Shared;
using Unity;
using Unity.Lifetime;

namespace Citi.EqRisk.RiskUI
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private readonly IUnityContainer _container;

    private void InitializeContainer()
    {
      _container.RegisterType<IViewFactory, ViewFactory>();
      _container.RegisterType<IDataGridViewController, DataGridViewController>();
      _container.RegisterType<ISubscriberIdProvider, DefaultSubscriberIdProvider>();
      _container.RegisterType<IRiskServerClient, RiskServerClient>(new ContainerControlledLifetimeManager());
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      var mainWindowPresenter = _container.Resolve<MainWindowPresenter>();

      var viewFactory = _container.Resolve<IViewFactory>();
      MainWindow = viewFactory.CreateMainWindow();
      MainWindow.DataContext = mainWindowPresenter.ViewModel;
      MainWindow.Show();
    }

    public App()
    {
      _container = new UnityContainer();

      InitializeContainer();
    }
  }
}
