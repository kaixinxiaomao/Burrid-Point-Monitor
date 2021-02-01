using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;


namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public abstract class ResponseTrackerConfig : IResponseTrackerConfig
  {
    public string CallerKey { get; set; }
    public float QPS { get; set; }
    public int BucketSize { get; set; }

    public int TimeOutInSecond { get; set; }
    public ILogger Logger { get; set; }

    public ResponseTrackerConfig(string key, float qps, int bucketSize, int timeOutInSecond, ILogger logger)
    {
      CallerKey = key;
      QPS = qps;
      BucketSize = bucketSize;
      TimeOutInSecond = timeOutInSecond;
      Logger = logger;
    }

    public string GetTrakerID()
    {
      return CallerKey;
    }
  }
}
