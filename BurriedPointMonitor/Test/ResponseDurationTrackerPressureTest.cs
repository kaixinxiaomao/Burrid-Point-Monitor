using System;
using System.Collections.Generic;
using System.Text;
using SumTotal.Framework.Logging;

namespace BurriedPointMonitor.Test
{
  class ResponseDurationTrackerPressureTest : TestCase<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>>
  {
    public ResponseDurationTrackerPressureTest(SimpleLogger logger) : base(logger) { }
    private static ResponseDurationTrackerProvider<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>> Provider = new ResponseDurationTrackerProvider<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig, ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig>>(); // declare as attribute of singleton, or can be stored in cache or IoC

    protected override IList<int[]> GenerateRunnerInput()
    {
      var tests = new List<int[]>();
      var totalTestMs = 90 * 1000;
      var rapidGen = new SleepInputGenerator(totalTestMs, 10, 15);  // this is to reveal the CPU/MEM consumption when 10K QPS
      var totalCC = 128; // 128 is the optimum threads in typical 5 core web server
      for (var i = 0; i < totalCC; i++)
      {
        tests.Add(rapidGen.Generate());
      }
      return tests;
    }

    protected override ResponseDurationTracker<ResponseDurationTrackerStopWatch, ResponseDurationTrackerConfig> ProvideTracker()
    {
      //return null;
      var config = new ResponseDurationTrackerConfig("BurriedPointMonitor", 1, 10, 10, _Logger);
      var tracker = Provider.CreateTracker(config);
      return tracker;
    }
  }
}
