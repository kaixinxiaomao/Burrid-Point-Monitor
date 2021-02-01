using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SumTotal.Framework.Core.Contracts.Logging;
using SumTotal.Framework.Logging;


namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// Hold a dictionary of response trackers.
  /// Instance as attribute of singleton.
  /// </summary>
  public abstract class ResponseTrackerProvider<T, T1, T2> : IResponseTrackerProvider<T, T1, T2> where T : ResponseTrackerStopWatch where T1 : ResponseTrackerConfig where T2 : ResponseTracker<T, T1>
  {

    public ResponseTrackerProvider()
    {
    }

    private IDictionary<string, T2> ResponseTrackerHash = new Dictionary<string, T2>();

    public abstract T2 CreateTracker(T1 trakerConfig);

    // retrieve a stop watch that has been started
    public T2 GetTracker(T1 trakerConfig)
    {
      var dict = ResponseTrackerHash;
      var key = trakerConfig.GetTrakerID();
      if (!dict.ContainsKey(key))
      {
        lock (dict)
        {
          if (!dict.ContainsKey(key))
          {
            dict[key] = CreateTracker(trakerConfig);
          }
        }
      }
      return dict[key];
    }

  }
}


