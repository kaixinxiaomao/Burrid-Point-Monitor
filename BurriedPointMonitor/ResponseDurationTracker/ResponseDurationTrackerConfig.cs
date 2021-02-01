using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;



namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public class ResponseDurationTrackerConfig : ResponseTrackerConfig
  {
    public ResponseDurationTrackerConfig(string key, float qps, int bucketSize, int timeOutInSecond, ILogger Logger) : base(key, qps, bucketSize, timeOutInSecond, Logger)
    {
    }

  }
}
