using System;
using System.Collections.Generic;
using System.Text;
using SumTotal.Framework.Logging;
using System.Linq;
using BurriedPointMonitor.Test.Contracts;
using SumTotal.Framework.Core.Contracts.Logging;

namespace BurriedPointMonitor.Test
{
  class ResponseMeltDownTrackerPressureTest : TestCase<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig, ResponseMeltDownTracker<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig>>
  {
    public ResponseMeltDownTrackerPressureTest(SimpleLogger logger) : base(logger) 
    {
    }
    private static ResponseMeltDownTrackerProvider<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig, ResponseMeltDownTracker<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig>> Provider = new ResponseMeltDownTrackerProvider<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig, ResponseMeltDownTracker<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig>>(); // declare as attribute of singleton, or can be stored in cache or IoC

    protected override IList<int[]> GenerateRunnerInput()
    {
      var totalCC = 128; // 128 is the optimum threads in typical 5 core web server
      var testByRunner = new List<List<int>>(totalCC);
      for (var i = 0; i < totalCC; i++)
      {
        var test = new List<int>();
        test.AddRange(new SleepInputGenerator(2 * 1000, 10, 30).Generate());
        test.AddRange(new SleepInputGenerator(2000 * 1000, 13 * 1000, 15 * 1000).Generate());
        test.AddRange(new SleepInputGenerator(360 * 1000, 10, 30).Generate());
        testByRunner.Add(test);
      }

      return testByRunner.Select(v => v.ToArray()).ToList();
    }


    protected override IRunner CreateRunner(int runnerId, int[] intervals, ResponseMeltDownTracker<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig> tracker, ILogger logger)
    {
      return new MeltDownSleepRunner(runnerId, intervals, tracker, logger);
    }

    protected override ResponseMeltDownTracker<ResponseMeltDownTrackerStopWatch, ResponseMeltDownTrackerConfig> ProvideTracker()
    {
      //return null;
      var config = new ResponseMeltDownTrackerConfig("BurriedPointMonitor", 
        1, // log once per second
        10, // bucket size 
        10, // timeout in second
        10, // melt down threshold
        2, // initial meltdown step
        60, // Longest allowed melt down time
        _Logger);
      var tracker = Provider.CreateTracker(config);
      return tracker;
    }
  }
}
