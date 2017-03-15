using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAutoQueue
{
    public EventAutoQueue()
    {
        AutomaticEventQueue = new List<GameEvent>();
    }

    public int GetEventCount()
    {
        return AutomaticEventQueue.Count;
    }

    public List<GameEvent> GetEventListCopy()
    {
        return AutomaticEventQueue;
    }

    public GameEvent GetNextEvent()
    {
        return AutomaticEventQueue[0];
    }

    public void ClearNextGameEvent()
    {
        AutomaticEventQueue.RemoveAt(0);
    }

    public bool HasEventsQueued()
    {
        switch(AutomaticEventQueue.Count)
        {
            case 0:
                return false;
            default:
                return true;
        }
    }

    public void ReceiveAutomaticEvent(GameEvent a_Event)
    {
        if (AutomaticEventQueue.Count > 0)
        {
            foreach (GameEvent e in AutomaticEventQueue)
            {
                if (e.GetEventName() != a_Event.GetEventName())
                {
                    AutomaticEventQueue.Add(a_Event);
                }
            }
        }
        else
        {
            AutomaticEventQueue.Add(a_Event);
        }

        SortAutomaticEventQueue();
    }

    /// <summary>
    /// Checks if any of the events in the list are new. If so, EventList adds them to its own collection.
    /// </summary>
    /// <param name="a_List"></param>
    public void ReceiveAutomaticEvents(List<GameEvent> a_List)
    {
        bool canAdd = false;

        if (AutomaticEventQueue.Count > 0)
        {
            foreach (GameEvent e in a_List)
            {
                for (int i = 0, n = AutomaticEventQueue.Count; i < n; i++)
                {
                    if (e.GetEventName() == AutomaticEventQueue[i].GetEventName())
                    {
                        canAdd = false;
                        break;
                    }
                }
                if(canAdd)
                {
                    AutomaticEventQueue.Add(e);
                    Debug.Log(e);
                }
            }
        }
        else
        {
            foreach (GameEvent e in a_List)
            {
                AutomaticEventQueue.Add(e);
            }
        }

        canAdd = true;
        SortAutomaticEventQueue();
    }

    /// <summary>
    /// Completely overrwrites the automatic event queue with new events. 
    /// </summary>
    /// <param name="a_ReplacementList"></param>
    public void OverwrriteAutomaticEventQueue(List<GameEvent> a_ReplacementList)
    {
        AutomaticEventQueue = a_ReplacementList;
        SortAutomaticEventQueue();
    }

    void SortAutomaticEventQueue()
    {
        List<GameEvent> tempList = new List<GameEvent>(AutomaticEventQueue);
        AutomaticEventQueue.Clear();
        List<int> tempIntList = new List<int>();

        foreach (GameEvent e in tempList)
        {
            tempIntList.Add(e.GetEventPriorityNumber());
        }

        tempIntList.Sort();

        for (int i = 0; i < tempIntList.Count; i++)
        {
            foreach (GameEvent e in tempList)
            {
                if (e.GetEventPriorityNumber() == tempIntList[i])
                {
                    AutomaticEventQueue.Add(e);
                    break;
                }
            }
        }
    }

    List<GameEvent> AutomaticEventQueue;
}
