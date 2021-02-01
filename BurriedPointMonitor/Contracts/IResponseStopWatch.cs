using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SumTotal.Framework.Core.Contracts.Logging
{
  /// <summary>
  /// Contract for the ResponseTracker.
  /// </summary>
  public interface IResponseStopWatch
  {
    void Log();

    void Stop(Object response);

    public void SetRequestDetail(string key, object val);
  }
}
