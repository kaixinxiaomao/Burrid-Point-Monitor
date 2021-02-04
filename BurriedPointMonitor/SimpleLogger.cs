using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;



namespace SumTotal.Framework.Logging
{
  public enum LogLevels
  { 
    TimeOut = 0,// output timed out traffic
    Log = 1,  // output captured traffic
    TestEvent = 2, // output test events such as thread creation and conclusion
    Traffic = 3, // output when sending/receiving every traffic
    Info = 4    // output internal events from ResponseTracker
  }
  /// <summary>
  /// </summary>
  public class SimpleLogger : ILogger
  {
    public SimpleLogger()
    {
    }

    public int Count { get; set; }
    public int TimeOutCount { get; set; }

    public LogLevels LogLevel { get; set; }
    public void Log(string message)
    {
      if (LogLevel < LogLevels.Log)
      {
        return;
      }
      Console.WriteLine(message);
      Count += 1;
    }

    public void LogTestEvent(string message)
    {
      if (LogLevel < LogLevels.TestEvent)
      {
        return;
      }
      Console.WriteLine(message);
    }

    public void LogTestEventInline(string message)
    {
      if (LogLevel < LogLevels.TestEvent)
      {
        return;
      }
      Console.Write(message);
    }

    public void LogTraffic(string message)
    {
      if (LogLevel < LogLevels.Traffic)
      {
        return;
      }
      Console.WriteLine(message);
    }

    public void LogInfo(string message)
    {
      if (LogLevel < LogLevels.Info)
      {
        return;
      }
      Console.WriteLine(message);
    }


    public void LogTimeOut(string message)
    {
      if (LogLevel < LogLevels.TimeOut)
      {
        return;
      }
      TimeOutCount += 1;
      Console.WriteLine(message);
    }

  }
}
