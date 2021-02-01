using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public class ResponseDurationTracker<T, T1> : ResponseTracker<T, T1> where T : ResponseDurationTrackerStopWatch where T1 : ResponseDurationTrackerConfig
  {    

    public ResponseDurationTracker(T1 config) : base(config)
    {
    }

    public override T CreateStopWatch(T1 config)
    {
      return (T) new ResponseDurationTrackerStopWatch(config.Logger);
    }
  }
}
