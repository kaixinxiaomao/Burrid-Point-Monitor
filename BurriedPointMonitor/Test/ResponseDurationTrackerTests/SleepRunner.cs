using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SumTotal.Framework.Core.Contracts.Logging;
using SumTotal.Framework.Logging;
using BurriedPointMonitor.Test.Contracts;

namespace BurriedPointMonitor.Test
{
  public class SleepRunner : Runner<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>> 
  {
    public SleepRunner(int runnerId, int[] intervals, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig> tracker, ILogger logger) : base(runnerId, intervals, tracker, logger)
    {
    }
    override public void RunTest()
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
      Logger.LogTestEvent("Runner " + RunnerID + " finished its test.");
    }

  }
}
