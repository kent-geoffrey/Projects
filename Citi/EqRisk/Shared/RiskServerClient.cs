using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Grpc.Core;
using JetBrains.Annotations;
using Riskfeed;

namespace Citi.EqRisk.Shared
{
  [UsedImplicitly]
  public sealed class RiskServerClient : IRiskServerClient
  {
    private readonly ISubscriberIdProvider _subscriberIdProvider;
    private readonly RiskFeed.RiskFeedClient _client;

    private readonly ISubject<PositionData> _positionDataSubject;

    public bool IsConnected { get; private set; }

    public IObservable<PositionData> PositionDataObservable => _positionDataSubject.AsObservable();

    public async Task Connect()
    {
      if (IsConnected) return;

      try
      {
        var feedRequest = new FeedRequest { SubscriberId = _subscriberIdProvider.SubscriberId };
        using (AsyncServerStreamingCall<RiskData> call = _client.SubscribeFeed(feedRequest))
        {
          IsConnected = true;
          IAsyncStreamReader<RiskData> responseStream = call.ResponseStream;

          while (await responseStream.MoveNext())
          {
            RiskData riskFeedDataItem = responseStream.Current;
            PositionData positionData = new PositionData(
              riskFeedDataItem.PositionId,
              riskFeedDataItem.Symbol,
              riskFeedDataItem.Quantity,
              riskFeedDataItem.Spot);
            _positionDataSubject.OnNext(positionData);
          }
        }
      }

      catch (RpcException e)
      {
        _positionDataSubject.OnError(e);
      }

      finally
      {
        IsConnected = false;
      }
    }

    public RiskServerClient(ISubscriberIdProvider subscriberIdProvider_)
    {
      _subscriberIdProvider = subscriberIdProvider_;

      var channel = new Channel(
        $"{Constants.SERVER_HOST_IP}:{Constants.SERVER_PORT}",
        ChannelCredentials.Insecure);

      _client = new RiskFeed.RiskFeedClient(channel);
      _positionDataSubject = new Subject<PositionData>();
    }
  }
}
