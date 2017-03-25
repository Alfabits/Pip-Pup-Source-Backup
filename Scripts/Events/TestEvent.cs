using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : GameEvent {

    public TestEvent()
    {
        unlocked = false;
        autostart = false;
        onetimerun = true;
        completed = true;
        eventname = "Test Event";
        EventsToBeUnlockedAfterCompletion = new List<string>();
        TextEventScript = new List<string>();
        SetPriorityNumber();
        CreateTextEventScript();
    }

    public override void StartEvent()
    {

    }

    public override List<string> GetTextEventScript()
    {
        return TextEventScript;
    }

    protected override void SetPriorityNumber()
    {
        priorityNumber = 10000;
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("Greetings.");
    }

    protected override void UnlockNextEvents()
    {
        
    }
}
