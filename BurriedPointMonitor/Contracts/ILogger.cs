using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SumTotal.Framework.Core.Contracts.Logging
{
  /// <summary>
  /// Contract for the logger.
  /// </summary>
  public interface ILogger
  {
    void LogDebug(string message);

    void LogInfo(string message);

    void LogTimeOut(string message);

    void LogTraffic(string message);

    void LogTestEvent(string message);

  }
}
