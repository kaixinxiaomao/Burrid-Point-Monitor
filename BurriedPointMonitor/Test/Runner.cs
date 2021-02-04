using System;
using System.Collections.Generic;
using System.Text;
using BurriedPointMonitor.Test.Contracts;
using SumTotal.Framework.Core.Contracts.Logging;
using SumTotal.Framework.Logging;

namespace BurriedPointMonitor.Test
{
  public abstract class Runner<T, T1, T2> : IRunner where T : ResponseTrackerStopWatch where T1 : ResponseTrackerConfig where T2 : ResponseTracker<T, T1>
  {
    public Runner(int runnerId, int[] intervals, T2 tracker, ILogger logger)
    {
      RunnerID = runnerId;
      Intervals = intervals;
      Tracker = tracker;
      Logger = logger;
    }

    protected int[] Intervals;
    protected static ILogger Logger;
    protected int RunnerID { get; set; }
    protected T2 Tracker;

    abstract public void RunTest(); 
  }
}
