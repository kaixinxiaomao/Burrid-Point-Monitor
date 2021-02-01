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
      //new ResponseDurationTrackerCorrectNessTest(_Logger).Execute();
      new ResponseDurationTrackerPressureTest(_Logger).Execute();
    }
  }
}
