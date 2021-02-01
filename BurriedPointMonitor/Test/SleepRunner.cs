using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SumTotal.Framework.Core.Contracts.Logging;
using SumTotal.Framework.Logging;

namespace BurriedPointMonitor.Test
{
  class SleepRunner<T, T1, T2> where T : ResponseTrackerStopWatch where T1 : ResponseTrackerConfig where T2 : ResponseTracker<T, T1>
  {
    public SleepRunner(int runnerId, int[] intervals, T2 tracker, ILogger logger)
    {
      RunnerID = runnerId;
      Intervals = intervals;
      Tracker = tracker;
      Logger = logger;
    }
    private static ILogger Logger;

    public void RunTest()
    {
      Thread.Sleep(Intervals[0]);
      for (var i = 1; i < Intervals.Length; i++)
      {
        Logger.LogTraffic("------>Sending No. " + i + " taking " + Intervals[i] + " ms to respond.");
        if (Tracker != null)
        {
          var watcher = Tracker.Start();
          watcher?.SetRequestDetail("runnerId", RunnerID);
          watcher?.SetRequestDetail("index", i);
          watcher?.SetRequestDetail("StartTime", watcher.StartTime.ToString("hh:mm:ss.fff"));
          Thread.Sleep(Intervals[i]);
          Tracker.Finish(watcher);
        }
        else // so that we can pass a null tracker to compare performance
        {
          Thread.Sleep(Intervals[i]);
        }
        Logger.LogTraffic("Runner " + RunnerID + "'s request No. " + i + "  received its response.");
      }
      Logger.LogTestEvent("Runner " + RunnerID + "finished its test.");
    }


    private int[] Intervals;
    public int RunnerID;
    private T2 Tracker;
  }
}
