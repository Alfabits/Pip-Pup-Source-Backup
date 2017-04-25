using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodBoyScript : GameEvent {

    public GoodBoyScript()
    {
        unlocked = true;
        autostart = false;
        usedelay = false;
        regular_events = false;
        dailycompleted = -1;
        love_event = true;
        eventname = "Good Boy";

        EventsToBeUnlockedAfterCompletion = new List<string>();

        LoveGiven = 5;
        EnergyRequired = 10;
        LevelRequired = 0;

        TextEventScript = new List<string>();
        SetPriorityNumber();
        CreateTextEventScript();
    }

    public override List<string> GetTextEventScript()
    {
        return TextEventScript;
    }

    public override void StartEvent()
    {
        
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("What is it, master?");
        TextEventScript.Add(".........");
        TextEventScript.Add("An intruiging question.");
        TextEventScript.Add("And one I cannot answer.");
        TextEventScript.Add("After all, who can be said to be truly \"good\"?");
        TextEventScript.Add("Philosophers have pondered and questioned this for ages.");
        TextEventScript.Add("I doubt we, as mere mortals, could possibly find-");
        TextEventScript.Add(".........");
        TextEventScript.Add("It is I?");
        TextEventScript.Add("I am a \"good boy\"?");
        TextEventScript.Add(".........");
        TextEventScript.Add("Well, problem solved, I suppose.");
    }

    protected override void SetPriorityNumber()
    {
        priorityNumber = 555;
    }

    protected override void UnlockNextEvents()
    {
        throw new NotImplementedException();
    }
}
