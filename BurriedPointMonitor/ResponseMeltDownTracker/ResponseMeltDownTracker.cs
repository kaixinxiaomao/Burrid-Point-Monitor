using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public class ResponseMeltDownTracker<T, T1> : ResponseTracker<T, T1> where T : ResponseMeltDownTrackerStopWatch where T1 : ResponseMeltDownTrackerConfig
  {    

    public ResponseMeltDownTracker(T1 config) : base(config)
    {
      MeltDownUntil = DateTime.Now;
      RecentTimeOutCount = Configue.TimeOutCountThreshold / 2; //Initialized to halfway to avoid crossing threshold right after boot up
    }

    public override T CreateStopWatch(T1 config)
    {
      return (T) new ResponseMeltDownTrackerStopWatch(config.Logger);
    }

    public bool IsMeltedDown()
    {
      if (DateTime.Now < MeltDownUntil)
      {
        //Logger.LogTestEventInline(".");
        return true;
      }
      else 
      {
        return false;
      }
    }

    //When RecentTimeOutCount crossed above TimeOutCountThreshold, start the meltdown and double MeltDownStepInSecond
    //there will not be any stopwatch timeout when meltdown ongoing
    //but there can be series of TimeOut happening together when remove first in loop at  ResponseTracker::Start, in this circumstance later/bigger timeout will override early/smaller timeout 
    protected override void OnTimeOut()
    {
      RecentTimeOutCount += 1;
      if (RecentTimeOutCount > Configue.TimeOutCountThreshold)  // when RecentTimeOutCount reached to TimeOutCountThreshold, meltdown for MeltDownStepInSecond seconds, and double next meltdown length
      {
        RecentTimeOutCount = Configue.TimeOutCountThreshold / 2;  // recount timeout/response from halfway
        MeltDownUntil = DateTime.Now.AddSeconds(MeltDownStepInSecond);
        Logger.LogTestEvent(">>>>>--------:(--------> Triggered Melt-down for " + MeltDownStepInSecond + " seconds.<------------");
        MeltDownStepInSecond = Math.Min(MeltDownStepInSecond * 2, Configue.MaxiMeltdownInSecond);
      }
    }

    //When RecentTimeOutCount crossed below 0, Shrink MeltDownStepInSecond by half
    //there will not be any stopwatch finishing when meltdown ongoing
    protected override void OnFinish()
    {
      if (MeltDownStepInSecond <= Configue.DefaultMeltDownStepInSecond)
      {
        return; // no previous meltdown going on
      }
      RecentTimeOutCount -= 1;
      if (RecentTimeOutCount < 0) //when RecentTimeOutCount reach to 0
      {
        MeltDownStepInSecond = Math.Max(MeltDownStepInSecond / 2, Configue.DefaultMeltDownStepInSecond);
        RecentTimeOutCount = Configue.TimeOutCountThreshold / 2; // recount timeout/response from halfway
        Logger.LogTestEvent("Next MeltDown Step decreased to " + MeltDownStepInSecond + "secondes. ----:)--->");
      }
    }

    private int RecentTimeOutCount; // increase when timeout, decrease when not time-out;
    private DateTime MeltDownUntil;
    private int MeltDownStepInSecond = 1;
  }
}
