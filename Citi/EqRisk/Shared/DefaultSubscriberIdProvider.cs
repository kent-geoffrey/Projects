using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Citi.EqRisk.Shared
{
  [UsedImplicitly]
  public sealed class DefaultSubscriberIdProvider : ISubscriberIdProvider
  {
    public string SubscriberId { get; }

    public DefaultSubscriberIdProvider()
    {
      SubscriberId = $"{Environment.MachineName}:{Process.GetCurrentProcess().Id}";
    }
  }
}
