using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using JetBrains.Annotations;

namespace Citi.EqRisk.Shared
{
  [UsedImplicitly]
  public sealed class DummyDataSource : IPositionDataSource
  {
    private const int DefaultHeartbeatIntervalMs = 1000;
    private const int DefaultMaxPositionId = 10000;
    private const int DefaultMaxQuantity = 9999;
    private const double DefaultMaxSpot = 100d;

    private readonly int _heartbeatIntervalMs;
    private readonly int _maxPositionId;
    private readonly int _maxQuantity;

    private readonly string[] _symbols;

    [CanBeNull]
    private readonly IScheduler _schedulerOverride;

    [NotNull]
    private readonly Random _randomGenerator;

    [NotNull]
    private readonly object _positionDataStoreLocker;

    [NotNull]
    private readonly IDictionary<int, PositionState> _positionDataStore;

    [NotNull]
    private readonly object _symbolSpotsLocker;

    [NotNull]
    private readonly IDictionary<string, double> _symbolSpots;

    [NotNull]
    private readonly SerialDisposable _generatorHeartbeatSubscription;

    [NotNull]
    private readonly Subject<PositionData> _positionDataSubject;

    [NotNull]
    public IObservable<PositionData> Live => _positionDataSubject.AsObservable();

    [NotNull]
    public IObservable<PositionData> Snapshot
    {
      get
      {
        lock (_positionDataStoreLocker)
        {
          return _positionDataStore.Values
            .Select(ToPositionData)
            .ToObservable();
        }
      }
    }

    [NotNull]
    private PositionData ToPositionData(PositionState positionState_)
    {
      string positionSymbol = positionState_.Symbol;
      double positionSymbolSpot = _symbolSpots[positionSymbol];

      return new PositionData(
        positionState_.PositionId,
        positionSymbol,
        positionState_.Quantity,
        positionSymbolSpot);
    }

    public void Start()
    {
      _generatorHeartbeatSubscription.Disposable =
        Observable.Interval(
            TimeSpan.FromMilliseconds(_heartbeatIntervalMs),
            _schedulerOverride ?? TaskPoolScheduler.Default)
        .Subscribe(OnNextHeartbeat);
    }

    public void Stop()
    {
      _generatorHeartbeatSubscription.Dispose();
    }

    private void OnNextHeartbeat(long counter_)
    {
      UpdateSymbolSpots();

      ISet<int> positionIdSet = GeneratePositionChangeSet();
      foreach (int positionId in positionIdSet)
      {
        PositionState thisPositionState;
        lock (_positionDataStoreLocker)
        {
          if (!_positionDataStore.TryGetValue(positionId, out thisPositionState)) return;
        }

        string positionSymbol = thisPositionState.Symbol;
        double positionSymbolSpot = _symbolSpots[positionSymbol];

        thisPositionState.Quantity = _randomGenerator.Next(_maxQuantity);

        var positionData = new PositionData(
          thisPositionState.PositionId,
          thisPositionState.Symbol,
          thisPositionState.Quantity,
          positionSymbolSpot);

        _positionDataSubject.OnNext(positionData);
      }
    }

    private ISet<int> GeneratePositionChangeSet()
    {
      int setSize = _randomGenerator.Next(1000, 1500);

      var positionIdSet = new HashSet<int>();
      while (positionIdSet.Count < setSize)
      {
        int nextPositionId = _randomGenerator.Next(_maxPositionId);
        if (positionIdSet.Contains(nextPositionId)) continue;

        positionIdSet.Add(nextPositionId);
      }

      return positionIdSet;
    }

    private void InitializeSOW()
    {
      lock (_positionDataStoreLocker)
      {
        _positionDataStore.Clear();

        for (int positionId = 0; positionId < _maxPositionId; positionId++)
        {
          _positionDataStore.Add(positionId, GeneratePositionState(positionId));
        }
      }
    }

    private PositionState GeneratePositionState(int positionId_)
    {
      int symbolIndex = _randomGenerator.Next(0, _symbols.Length);
      string positionSymbol = _symbols[symbolIndex];
      int quantity = _randomGenerator.Next(_maxQuantity);

      return new PositionState(positionId_, positionSymbol, quantity);
    }

    private void UpdateSymbolSpots()
    {
      lock (_symbolSpotsLocker)
      {
        for (int i = 0; i < _symbols.Length; i++)
        {

          double spotValue = default(double);
          while (!(spotValue > 0d))
          {
            spotValue = _randomGenerator.NextDouble() * 100;
          }

          string symbol = _symbols[i];
          if (_symbolSpots.ContainsKey(symbol)) _symbolSpots[symbol] = spotValue;
          else _symbolSpots.Add(symbol, spotValue);
        }
      }
    }

    public DummyDataSource() :
      this(DefaultHeartbeatIntervalMs)
    { }

    public DummyDataSource(
      int heartbeatInterval_ = DefaultHeartbeatIntervalMs,
      int maxPositionId_ = DefaultMaxPositionId,
      int maxQuantity_ = DefaultMaxQuantity,
      double maxSpot_ = DefaultMaxSpot,
      IScheduler schedulerOverride_ = null)
    {
      _randomGenerator = new Random();
      _generatorHeartbeatSubscription = new SerialDisposable();
      _positionDataSubject = new Subject<PositionData>();

      _symbols = new[] { "AAPL", "MSFT", "SPX", "GOOG", "FB" };

      _symbolSpotsLocker = new object();
      _symbolSpots = new Dictionary<string, double>();

      _positionDataStoreLocker = new object();
      _positionDataStore = new Dictionary<int, PositionState>();

      _heartbeatIntervalMs = heartbeatInterval_;
      _maxPositionId = maxPositionId_;
      _maxQuantity = maxQuantity_;

      _schedulerOverride = schedulerOverride_;

      UpdateSymbolSpots();

      InitializeSOW();
    }

    /// <summary>
    /// Represents the state of a position
    /// </summary>
    private sealed class PositionState
    {
      public int PositionId { get; }

      public string Symbol { get; }

      public int Quantity { get; set; }

      public PositionState(
        int positionId_,
        [NotNull] string symbol_,
        int quantity_)
      {
        PositionId = positionId_;
        Symbol = symbol_;
        Quantity = quantity_;
      }
    }
  }
}
