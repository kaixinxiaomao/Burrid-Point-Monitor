using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SumTotal.Framework.Core.Contracts.Logging
{
  /// <summary>
  /// Contract for the ResponseTracker.
  /// </summary>
  public interface IResponseTracker<T, T1> where T: IResponseStopWatch where T1: IResponseTrackerConfig
  {
    T Start();

    void Finish(T stopWatch);
  }
}
