using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;

namespace SumTotal.Framework.Logging
{
  public class ResponseMeltDownTrackerStopWatch : ResponseTrackerStopWatch
  {
    public ResponseMeltDownTrackerStopWatch(ILogger logger) : base(logger) 
    {
      Duration = -1;
    }

    public int Duration { get; set; }

    public override void Log()
    {
      LoggerObj.Log(GenerateKey() + " Stopped after " + Duration + " ms.");
    }

    public override void Timeout()
    {
      LoggerObj.LogTimeOut(GenerateKey() + " Time out!");
    }

    public override bool IsFinished()
    {
      return Duration >= 0;
    }

    public override void Stop(object response = null)
    {
      Duration = (int)(DateTime.Now - StartTime).TotalMilliseconds;
    }

    private string GenerateKey()
    {
      var key = DateTime.Now.ToString("hh:mm:ss.fff") + ":: ";
      foreach (var item in RequestDetail.Keys)
      {
        key += item + ":" + RequestDetail[item] + " | ";
      }

      return key;
    }
  }
}
