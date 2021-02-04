using System;
using System.Collections.Generic;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;
using SumTotal.Framework.Logging;
using System.Threading;
using BurriedPointMonitor.Test.Contracts;

namespace BurriedPointMonitor.Test
{
  public abstract class TestCase<T, T1, T2> where T : ResponseTrackerStopWatch where T1 : ResponseTrackerConfig where T2 : ResponseTracker<T, T1>
  {
    public TestCase(SimpleLogger logger)
    {
      _Logger = logger;
    }


    protected static SimpleLogger _Logger;


    protected abstract IList<int[]> GenerateRunnerInput();
    protected abstract T2 ProvideTracker();
    protected abstract IRunner CreateRunner(int runnerId, int[] intervals, T2 tracker, ILogger logger);


    public void Execute()
    {
      var tracker = ProvideTracker();
      var tests = GenerateRunnerInput();

      _Logger.LogTestEvent("Start creating " + tests.Count + "runner threads.");
      var threads = new List<Thread>();
      for (var i = 0; i < tests.Count; i++)
      {
        var t = tests[i];
        var runner0 = CreateRunner(
          i,
          t,
          tracker,
          _Logger
          );
        var thread0 = new Thread(new ThreadStart(runner0.RunTest));
        threads.Add(thread0);
      }
      _Logger.LogTestEvent("Finish creating " + tests.Count + "runner threads.");
      _Logger.LogTestEvent("Performane test now begin:");
      DateTime beginTime = DateTime.Now;
      foreach (var th in threads)
      {
        th.Start();
      }
      foreach (var th in threads)
      {
        th.Join();
      }

      Console.WriteLine("Runned for " + (DateTime.Now - beginTime).TotalSeconds + " seconds, " + _Logger.Count + " is logged.");

    }
  }
}
