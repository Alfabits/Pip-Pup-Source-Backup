using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent {

    protected bool unlocked = false;
    protected GameEvent[] EventsToBeUnlockedAfterCompletion;
    protected List<string> TextEventScript;

    public abstract void StartEvent();

    protected abstract void CreateTextEventScript();
    public abstract List<string> GetTextEventScript();

    public bool CheckIfUnlocked()
    {
        return unlocked;
    }

}
