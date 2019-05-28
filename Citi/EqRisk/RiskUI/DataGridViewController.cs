using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Forms;
using Citi.EqRisk.Shared;
using JetBrains.Annotations;

namespace Citi.EqRisk.RiskUI
{
  [UsedImplicitly]
  public sealed class DataGridViewController : IDataGridViewController
  {
    [NotNull] private readonly IRiskServerClient _riskServerClient;
    [NotNull] private readonly object _gridRowViewModelsLocker;
    [NotNull] private readonly BindingList<GridRowViewModel> _gridDataSource;
    [NotNull] private readonly IDictionary<int, GridRowViewModel> _gridRowViewModels;
    [NotNull] private readonly SerialDisposable _clientConnection;

    [NotNull]
    private readonly DataGridView _dataGridView;

    [NotNull]
    public Control View => _dataGridView;

    private void SetupDataGridView()
    {
      _dataGridView.AutoGenerateColumns = true;
      _dataGridView.DataSource = _gridDataSource;

      _clientConnection.Disposable = _riskServerClient.PositionDataObservable.Subscribe(OnNextPositionData, OnError);
      _riskServerClient.Connect();
    }

    private void OnNextPositionData(PositionData positionData_)
    {
      lock (_gridRowViewModelsLocker)
      {
        var positionId = positionData_.PositionId;
        GridRowViewModel existingGridRowViewModel;
        if (!_gridRowViewModels.TryGetValue(positionId, out existingGridRowViewModel))
        { // new
          var gridRowViewModel = new GridRowViewModel(positionId)
          {
            Symbol = positionData_.Symbol,
            Quantity = positionData_.Quantity,
            Spot = positionData_.Spot
          };

          _gridRowViewModels.Add(positionId, gridRowViewModel);
          _gridDataSource.Add(gridRowViewModel);
          return;
        };

        existingGridRowViewModel.Symbol = positionData_.Symbol;
        existingGridRowViewModel.Quantity = positionData_.Quantity;
        existingGridRowViewModel.Spot = positionData_.Spot;
      }
    }

    private void OnError(Exception e_) { /* TODO: handle connection error */ }

    public DataGridViewController(
      [NotNull] IViewFactory viewFactory_,
      [NotNull] IRiskServerClient riskServerClient_)
    {
      _clientConnection = new SerialDisposable();
      _riskServerClient = riskServerClient_;
      _gridDataSource = new BindingList<GridRowViewModel>();

      _gridRowViewModelsLocker = new object();
      _gridRowViewModels = new Dictionary<int, GridRowViewModel>();

      _dataGridView = viewFactory_.CreateDataGridView();
      SetupDataGridView();
    }
  }
}
