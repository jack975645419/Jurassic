using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基本时钟
/// </summary>
public class SingleTimer
{
    public bool IsActive = false;
    public float StartTime;
    public float Interval;
    public delegate void TimerDelegate(params object[] p);
    public TimerDelegate Event;
    public bool IsLoop = false;
    //仅在IsLoop=true时有意义
    public bool DoExecuteImmediately = false;
    public object[] Params;
    public SingleTimer(float Interval, TimerDelegate Event, bool IsLoop = false, bool DoExecuteImmediately = false, params object[] Params)
    {
        this.Interval = Interval;
        this.Event = Event;
        this.IsLoop = IsLoop;
        this.DoExecuteImmediately = DoExecuteImmediately;
        this.Params = Params;
    }
    //具有消费的含义，调用后将会改变内部变量DoExecuteImmediately
    public bool IsTimeUp_Consume()
    {
        if (!IsActive) return false;
        if (IsLoop && DoExecuteImmediately)
        {
            DoExecuteImmediately = false;
            return true;
        }
        //时辰已到
        if (Time.time >= StartTime+Interval)
        {
            if(IsLoop)
            {
                StartTime = Time.time;
            }
            else
            {
                IsActive = false;
            }
            return true;
        }
        return false;
    }
}


public class TimerManager : Singleton<TimerManager> {

    private List<SingleTimer> m_Timers = new List<SingleTimer>();

    /// <summary>
    /// 新的或旧的计时器的启用
    /// </summary>
    public void StartTimer(SingleTimer timer)
    {
        timer.StartTime = Time.time;
        timer.IsActive = true;
        if(!m_Timers.Contains(timer))
        {
            m_Timers.Add(timer);
        }
    }
    public void Stop(SingleTimer timer)
    {
        timer.IsActive = false;
    }

    /// <summary>
    /// 启用一个临时的计时器，返回一个SingleTimer作为句柄
    /// </summary>
    public SingleTimer SetNewTimer(float Interval, SingleTimer.TimerDelegate Event, bool IsLoop = false, bool DoExecuteImmediately = false, params object[] Params)
    {
        SingleTimer t = new SingleTimer(Interval, Event, IsLoop, DoExecuteImmediately, Params);
        StartTimer(t);
        return t;
    }

    // Update is called once per frame
    void Update () {
        //遍历所有的计时器并尝试执行它们
		for(int k = m_Timers.Count-1; k>=0; k--)
        {
            if(m_Timers[k].IsTimeUp_Consume())
            {
                m_Timers[k].Event(m_Timers[k].Params);
            }

        }
	}
}
