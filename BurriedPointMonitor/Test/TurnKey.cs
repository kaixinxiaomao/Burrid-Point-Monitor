using System;
using System.Threading;
using SumTotal.Framework.Logging;
using System.Collections.Generic;

namespace BurriedPointMonitor.Test
{
  class TurnKey
  {
    private static ResponseDurationTrackerProvider<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>> Provider = new ResponseDurationTrackerProvider<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>>(); // declare as attribute of singleton, or can be stored in cache or IoC
    private static SimpleLogger _Logger = new SimpleLogger();
    static void Main(string[] args)
    {
      var config = new ResponseDurationTrackerConfig("BurriedPointMonitor", 1, 10, 10, _Logger);
      var tracker = Provider.CreateTracker(config);

      _Logger.LogLevel = LogLevels.Traffic;
      //tracker = null;

      var tests = new List<int[]>();
      var totalTestMs = 120 * 1000;
      var slowGen = new SleepInputGenerator(totalTestMs, 200, 2 * 1000);
      var rapidGen = new SleepInputGenerator(totalTestMs, 2000, 20 * 1000);
      var totalCC = 128; // 128 is the optimum threads in typical 5 core web server
      for (var i = 0; i < totalCC / 2; i++)
      {
        tests.Add(slowGen.Generate());
        tests.Add(rapidGen.Generate());
      }

      
      _Logger.LogTestEvent("Start creating " + tests.Count + "runner threads.");
      var threads = new List<Thread>();
      for (var i = 0; i < tests.Count; i++)
      {
        var t = tests[i];
        var runner0 = new SleepRunner<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>>(
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
