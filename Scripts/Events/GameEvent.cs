using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class GameEvent : IComparable<GameEvent>
{

    public GameEvent()
    {

    }

    //Status of the current event
    protected bool unlocked = false;
    protected bool completed = false;
    protected bool regular_events = false;
    protected bool love_event = false;
    protected bool autostart = false;
    protected bool usedelay = false;
    protected int dailycompleted = 0;
    protected int priorityNumber = -1;
    protected float delay = 3.0f;
    protected string eventname = "nameless";

    //Info for the event regarding the game's stats
    protected int LoveGiven = 0;
    protected int EnergyRequired = 0;
    protected int LevelRequired = 0;

    //Members regarding the event manager
    public List<string> EventsToBeUnlockedAfterCompletion;
    protected List<string> TextEventScript;
    protected EventManager EM;

    //The class' methods
    public abstract List<string> GetTextEventScript();
    protected abstract void CreateTextEventScript();
    protected abstract void SetPriorityNumber();
    protected abstract void UnlockNextEvents();
    public abstract void StartEvent();

    public void CompleteEvent()
    {
        completed = true;
        if (CheckForDailyCompletion() == 0)
            dailycompleted = 1;
    }

    public bool CheckIfUnlocked()
    {
        return unlocked;
    }

    public bool CheckForFirstTimeCompletion()
    {
        return completed;
    }

    public void SetIsCompleted(bool a_Arg)
    {
        completed = a_Arg;
    }

    /// <summary>
    /// Returns whether or not the event uses daily completion. If so, this will also return whether or not it has been completed in the past 24 hours. 
    /// 1 = 'Completed', 0 = 'Not completed', -1 = 'Does not use daily completion'
    /// </summary>
    /// <returns></returns>
    public int CheckForDailyCompletion()
    {
        if (!regular_events)
        {
            return dailycompleted;
        }

        return -1;
    }

    public void SetDailyComletion(int a_Arg)
    {
        dailycompleted = a_Arg;
    }

    public void SetUnlockedStatus(bool a_Arg)
    {
        unlocked = a_Arg;
    }

    public bool CheckIfLoveEvent()
    {
        return love_event;
    }

    public bool DoesEventRunOnlyOnce()
    {
        return regular_events;
    }

    public bool DoesEventStartAutomatically()
    {
        return autostart;
    }

    public bool CheckIfEventUsesDelay()
    {
        return usedelay;
    }

    public int GetEventPriorityNumber()
    {
        return priorityNumber;
    }

    public float GetDelayAmount()
    {
        return delay;
    }

    public string GetEventName()
    {
        return eventname;
    }

    public int GetLoveGiven()
    {
        return LoveGiven;
    }

    public int GetEnergyRequired()
    {
        return EnergyRequired;
    }

    public int GetLevelRequired()
    {
        return LevelRequired;
    }

    /// <summary>
    /// Takes two objects of any type and determines whether or not they are of the same type or are inherited from the same type.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int CompareType(object x, object y)
    {
        //Grabs the two types of each object
        var event1 = x.GetType();
        var event2 = y.GetType();

        //Checks if these types inherit from the same class
        if (event1.IsAssignableFrom(event2) || event2.IsAssignableFrom(event1))
        {
            return 1;
        }
        //Checks if one of these types inherits from the same class, but the other doesn't (shouldn't happen)
        else if ((event1.IsAssignableFrom(event2) && !event2.IsAssignableFrom(event1)) ||
            (!event1.IsAssignableFrom(event2) && event2.IsAssignableFrom(event1)))
        {
            return 0;
        }

        //Otherwise, both cases failed and the objects aren't related
        return -1;
    }

    public int CompareTo(GameEvent other)
    {
        return CompareType(this, other);
    }

}