using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SumTotal.Framework.Core.Contracts.Logging;

namespace SumTotal.Framework.Logging
{
  /// <summary>
  /// </summary>
  public abstract class ResponseTracker<T, T1> : IResponseTracker<T, T1> where T : ResponseTrackerStopWatch where T1 : ResponseTrackerConfig
  {
    protected static ILogger Logger;
    public ResponseTracker(T1 config)
    {
      Configue = config;
      Logger = config.Logger;
    }

    // leaking bucket implementation
    public virtual T Start()
    {
      T stopWatch = null;
      IList<T> toBeLogged = null;
      lock (this)
      {
        while (LeakyBucket.First != null && LeakyBucket.First.Value.StartTime < BottomOfBucket.Value.StartTime && (int)(DateTime.Now - LeakyBucket.First.Value.StartTime).TotalSeconds > Configue.TimeOutInSecond)
        {
          LeakyBucket.First.Value.Timeout();
          OnTimeOut();
          LeakyBucketHash.Remove(LeakyBucket.First.Value.StartTime);
          LeakyBucket.RemoveFirst();
          LastLogTime = DateTime.Now;
        }

        // before tracking current request, process existing requests already in bucket no more than QPS allowed
        int leakingSize = 0;
        if (BottomOfBucket != null)
        {
          //Logger.LogInfo("bottom at " + BottomOfBucket.Value.RequestDetail["index"]);
          leakingSize = GetLeakingSince(LastLogTime);
        }
        for (var i = 0; i < leakingSize; i++) // leaking limited number of items out of bucket according to QPS 
        {
          if (BottomOfBucket == null) // bucket is empty;
          {
            break;
          }
          if (BottomOfBucket.Value.IsFinished()) // if it's already stopped/received when being leaked
          {
            var prev = BottomOfBucket;
            BottomOfBucket = BottomOfBucket.Next; // leak

            //directly remove and log
            LeakyBucket.Remove(prev);
            Logger.LogInfo(">> Logging the already finishsed when leaking, " + "sent at " + prev.Value.StartTime.Ticks);
            if (toBeLogged == null)
            {
              toBeLogged = new List<T>();
            }
            toBeLogged.Add(prev.Value);
            LastLogTime = DateTime.Now;
          }
          else
          {
            BottomOfBucket = BottomOfBucket.Next; // leak only
          }
          nItemsInBucket -= 1;
        }

        // if bucket is full, ignore current request to realize down sampling
        if (nItemsInBucket >= Configue.BucketSize)
        {
          Logger.LogInfo("Ignored, too frequent, nItemsInBucket as " + nItemsInBucket);
        }
        else
        {
          // enqueue and track this request
          stopWatch = CreateStopWatch(Configue);
          var node = new LinkedListNode<T>(stopWatch);

          LeakyBucket.AddLast(node);
          LeakyBucketHash[stopWatch.StartTime] = node;  // so to locate the stopWatch with O(1) when its response received and finish method invoked.
          nItemsInBucket += 1;
          Logger.LogInfo("Enqueued" + stopWatch.StartTime.Ticks + ", nItemsInBucket become " + nItemsInBucket);
          if (BottomOfBucket == null)
          {
            BottomOfBucket = node;
          }
        }
      }

      // move watcher's Log logic out of locked critical section in case any lengthy operation defined
      if (toBeLogged != null)
      {
        foreach (var watcher in toBeLogged)
        {
          watcher.Log();
        }
      }


      return stopWatch;

    }

    // conclude the stop watch upon receiving a response 
    public virtual void Finish(T stopWatch)
    {
      if (stopWatch == null)  // in case current reqeuest has been ignored when this.Start
      {
        return;
      }
      var bLogged = false;  // so to move out stopWatch execution out of locked critical section
      LinkedListNode<T> node = null;
      lock (this)
      {
        if (LeakyBucketHash.ContainsKey(stopWatch.StartTime)) // current request has been sampled
        {
          node = LeakyBucketHash[stopWatch.StartTime];
          LeakyBucketHash.Remove(stopWatch.StartTime);
          if ((int)(DateTime.Now - node.Value.StartTime).TotalSeconds > Configue.TimeOutInSecond)
          {
            OnTimeOut();
          }
          else
          {
            OnFinish();
          }

          if (node.Value.StartTime < BottomOfBucket.Value.StartTime)  // in case current request already been leaked
          {
            bLogged = true;
            LeakyBucket.Remove(node);
            LastLogTime = DateTime.Now;
          }

        }
      }
      if (node == null)
      {
        return;
      }

      node.Value.Stop(null);    // stop the located stopWatch
      
      if (bLogged)
      {
        Logger.LogInfo(">>> Log upon receiving leaked response, sent at " + node.Value.StartTime.Ticks);
        node.Value.Log();
      }

    }

    public abstract T CreateStopWatch(T1 config);

    protected virtual void OnTimeOut()
    { }

    protected virtual void OnFinish()
    { }


    private int GetLeakingSince(DateTime end)
    {
      return (int)((DateTime.Now - end).TotalSeconds * Configue.LogsPerSecond);
    }


    private int nItemsInBucket = 0;

    private LinkedList<T> LeakyBucket = new LinkedList<T>();

    // a startTime to stopWatch hash who has been sampled but yet been finished/received
    private IDictionary<DateTime, LinkedListNode<T>> LeakyBucketHash = new Dictionary<DateTime, LinkedListNode<T>>();
    private LinkedListNode<T> BottomOfBucket;
    private DateTime LastLogTime = DateTime.Now; // = new DateTime(2021, 1, 1, 16, 45, 0); // this is to make sure the very first request can be leaked/captured 
    public T1 Configue { get; set; }

  }
}
