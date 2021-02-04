using System;
using System.Collections.Generic;
using System.Linq;
using SumTotal.Framework.Core.Contracts.Logging;


namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public class ResponseMeltDownTrackerProvider<T, T1, T2> : ResponseTrackerProvider<T, T1, T2> where T: ResponseMeltDownTrackerStopWatch where T1 : ResponseMeltDownTrackerConfig where T2: ResponseMeltDownTracker<T, T1>
  {
    public override T2 CreateTracker(T1 trakerConfig)
    {
      return (T2) new ResponseMeltDownTracker<T, T1>(trakerConfig);
    }

  }
}


