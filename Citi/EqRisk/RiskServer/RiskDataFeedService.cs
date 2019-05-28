using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Citi.EqRisk.Shared;
using Grpc.Core;
using Riskfeed;

namespace Citi.EqRisk.RiskServer
{
  public sealed class RiskDataFeedService : RiskFeed.RiskFeedBase
  {
    private readonly IPositionDataSource _positionDataSource;

    public override async Task SubscribeFeed(FeedRequest request_, IServerStreamWriter<RiskData> responseStream_, ServerCallContext context_)
    {
      IObservable<PositionData> riskDataObservable =
        _positionDataSource.Snapshot.Concat(_positionDataSource.Live);

      IEnumerable<RiskData> riskDataSequence =
        riskDataObservable
          .ToEnumerable()
          .Select(positionData_ => new RiskData
          {
            PositionId = positionData_.PositionId,
            Symbol = positionData_.Symbol,
            Quantity = positionData_.Quantity,
            Spot = positionData_.Spot
          });

      foreach (RiskData riskData in riskDataSequence)
      {
        await responseStream_.WriteAsync(riskData);
      }
    }

    public RiskDataFeedService(IPositionDataSource positionDataSource_)
    {
      _positionDataSource = positionDataSource_;
    }
  }
}
