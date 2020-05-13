using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTimer : MonoBehaviour
{
    class TimeEvent
    {
        public float TimeToExecute;
        public Callback Method;
    }
    List<TimeEvent> events;
    // 委托类型
    public delegate void Callback();

    private void Awake()
    {
        events = new List<TimeEvent>();

    }

    public void Add(Callback cb, float inSeconds)
    {
        events.Add(new TimeEvent
        {
            Method = cb,
            TimeToExecute = Time.time + inSeconds
        });

    }

    private void Update()
    {
        if (events.Count == 0)
            return;
        for (int i = 0; i < events.Count; i++)
        {
            var timeEvent = events[i];
            if (timeEvent.TimeToExecute <= Time.time)
            {
                timeEvent.Method();
                events.Remove(timeEvent);
            }
        }
    }
}
