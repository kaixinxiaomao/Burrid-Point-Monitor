using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;



namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public class ResponseMeltDownTrackerConfig : ResponseTrackerConfig
  {
    public ResponseMeltDownTrackerConfig(string key, float qps, int bucketSize, int timeOutInSecond, int timeOutCountThreshold, int meltDownStepInSecond, int maxiMeltdownInSecond, ILogger Logger) : base(key, qps, bucketSize, timeOutInSecond, Logger)
    {
      TimeOutCountThreshold = timeOutCountThreshold;
      DefaultMeltDownStepInSecond = meltDownStepInSecond;
      MaxiMeltdownInSecond = maxiMeltdownInSecond;
    }

    public int TimeOutCountThreshold { get; set; }
    public int DefaultMeltDownStepInSecond { get; set; }
    public int MaxiMeltdownInSecond { get; set; }
  }
}
