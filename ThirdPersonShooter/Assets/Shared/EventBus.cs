using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public class EventListener
    {
        public delegate void Callback();
        public bool IsSingleShot;

        public Callback Method;
        public EventListener()
        {
            IsSingleShot = true;
        }
    }

    Dictionary<string, IList<EventListener>> eventTable;
    Dictionary<string, IList<EventListener>> EventTable
    {
        get
        {
            if (eventTable == null)
                eventTable = new Dictionary<string, IList<EventListener>>();
            return eventTable;
        }
    }
    public void AddListener(string name, EventListener.Callback cb)
    {
        AddListener(name, new EventListener
        {
            Method = cb
        });
    }
    public void AddListener(string name, EventListener listener)
    {
        if (!EventTable.ContainsKey(name))
            EventTable.Add(name, new List<EventListener>());
        if (EventTable[name].Contains(listener))
            return;
        EventTable[name].Add(listener);
    }

    public void RaiseEvent(string name)
    {
        if (!EventTable.ContainsKey(name))
            return;
        for (int i = 0; i < EventTable[name].Count; i++)
        {
            EventListener listener = EventTable[name][i];
            listener.Method();
            if (listener.IsSingleShot)
                EventTable[name].Remove(listener);
        }
    }
}
