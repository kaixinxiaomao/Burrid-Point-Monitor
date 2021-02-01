using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;

namespace SumTotal.Framework.Logging
{
  public abstract class ResponseTrackerStopWatch : IResponseStopWatch
  {
    public ResponseTrackerStopWatch(ILogger logger)
    {
      StartTime = DateTime.Now;
      RequestDetail = new Dictionary<string, object>();
      LoggerObj = logger;
    }

    public DateTime StartTime { get; set; }

    protected ILogger LoggerObj;

    public IDictionary<string, object> RequestDetail;
    public void SetRequestDetail(string key, object val)
    {
      RequestDetail[key] = val;
    }

    public abstract bool IsFinished();

    // will be invoked by ResponseTracker when logging
    public abstract void Log();

    // will be invoked by ResponseTracker when synchronous call finished
    public abstract void Stop(Object response);

    public abstract void Timeout();
  }
}
