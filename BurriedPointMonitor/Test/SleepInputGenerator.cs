using System;
using System.Collections.Generic;
using System.Text;

namespace BurriedPointMonitor.Test
{
  class SleepInputGenerator
  {
    public SleepInputGenerator(int totalMs, int minMs, int maxMs)
    {
      TotalMs = totalMs;
      MinMs = minMs;
      MaxMs = maxMs;
    }

    public int[] Generate()
    {
      var rst = new List<int>();
      var remain = TotalMs;
      var random = new System.Random();
      rst.Add(random.Next(MinMs, MaxMs)); //to void all runner's first request to be sent out at same time, SleepRunner will sleep for rst[0] ms before sending out first request.
      while (remain > 0)
      {
        var cur = random.Next(MinMs, MaxMs);
        rst.Add(cur);
        remain -= cur;
      }
      return rst.ToArray();
    }
    private int TotalMs;
    private int MinMs;
    private int MaxMs;
  }
}
