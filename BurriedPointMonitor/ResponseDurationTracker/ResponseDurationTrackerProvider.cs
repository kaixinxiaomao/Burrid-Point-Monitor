using System;
using System.Collections.Generic;
using System.Linq;
using SumTotal.Framework.Core.Contracts.Logging;


namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public class ResponseDurationTrackerProvider<T, T1, T2> : ResponseTrackerProvider<T, T1, T2> where T: ResponseDurationTrackerStopWatch where T1 : ResponseDurationTrackerConfig where T2: ResponseDurationTracker<T, T1>
  {
    public override T2 CreateTracker(T1 trakerConfig)
    {
      return (T2) new ResponseDurationTracker<T, T1>(trakerConfig);
    }

  }
}


