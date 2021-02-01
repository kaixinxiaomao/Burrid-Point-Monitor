using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SumTotal.Framework.Core.Contracts.Logging
{
  /// <summary>
  /// Contract for the ResponseTracker.
  /// </summary>
  public interface IResponseTrackerProvider<T, T1, T2> where T : IResponseStopWatch where T1 : IResponseTrackerConfig where T2 : IResponseTracker<T, T1>
  {
    T2 GetTracker(T1 config);
  }

}
