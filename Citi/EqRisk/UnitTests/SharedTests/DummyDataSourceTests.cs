using System;
using System.Reactive.Concurrency;
using System.Threading;
using Citi.EqRisk.Shared;
using NUnit.Framework;

namespace Citi.EqRisk.UnitTests.SharedTests
{
  public class DummyDataSourceTests
  {

    private readonly IScheduler _testScheduler = Scheduler.Immediate;

    [TestCase(0)]
    [TestCase(5)]
    [TestCase(10)]
    public void Created_InitialSnapshotHasFullPopulation(int maxPositionId_)
    {
      int snapshotPositionDataItemCounter = 0;

      var sut = new DummyDataSource(
        maxPositionId_: maxPositionId_,
        heartbeatInterval_: 10,
        schedulerOverride_: _testScheduler);

      sut.Snapshot.Subscribe(data_ => snapshotPositionDataItemCounter++);

      Assert.AreEqual(maxPositionId_, snapshotPositionDataItemCounter);
    }

    [Test]
    public void Started_HasLiveData()
    {
      var sut = new DummyDataSource(heartbeatInterval_: 1);

      var waitHandle = new AutoResetEvent(false);
      int livePositionDataItemCounter = 0;
      sut.Live.Subscribe(data_ =>
      {
        livePositionDataItemCounter++;
        waitHandle.Set();
      });

      sut.Start();

      waitHandle.WaitOne(TimeSpan.FromMilliseconds(100));

      Assert.That(livePositionDataItemCounter > 0);
    }
  }
}
