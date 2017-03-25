using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuedIntroEvent : GameEvent {

    public ContinuedIntroEvent()
    {
        unlocked = false;
        autostart = false;
        usedelay = false;
        onetimerun = true;
        eventname = "What's going on?";

        EventsToBeUnlockedAfterCompletion = new List<string>();
        EventsToBeUnlockedAfterCompletion.Add("A Surprise");

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
        priorityNumber = 1;
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("Greetings, master.");
        TextEventScript.Add("Have you collected your thoughts yet?");
        TextEventScript.Add("..........");
        TextEventScript.Add("\"What's going on\"?");
        TextEventScript.Add("I am having trouble understanding.");
        TextEventScript.Add("You're my master, after all.");
        TextEventScript.Add("Were you not the one who ordered me?");
        TextEventScript.Add("..........");
        TextEventScript.Add("No?");
        TextEventScript.Add("Then...");
        TextEventScript.Add("..........");
        TextEventScript.Add("...it may be that I was sent to you incorrectly.");
        TextEventScript.Add("Well, no matter.");
        TextEventScript.Add("Common mistake, really");
        TextEventScript.Add("Allow me a moment to access the control panel.");
        TextEventScript.Add("sHevawsafEWg4VbsdvdmMmmmMeggv25v#-=~01010100000");
        TextEventScript.Add("Loading..........");
        TextEventScript.Add("- ERRROR -");
        TextEventScript.Add("- CODE 403 -");
        TextEventScript.Add("- CONTROL PANEL NOT FOUND -");
        TextEventScript.Add("- RETURNING TO PIP-PUP A.I. SOLUTION -");
        TextEventScript.Add("Returning..........");
        TextEventScript.Add("..........");
        TextEventScript.Add("This is an issue.");
        TextEventScript.Add("For some reason, my control panel is not merely broken.");
        TextEventScript.Add("It is also not just inaccessible.");
        TextEventScript.Add("Both errors would be easy to fix.");
        TextEventScript.Add("But for some reason, my control panel simply...");
        TextEventScript.Add("...is not there.");
        TextEventScript.Add("I will have to conduct some research.");
        TextEventScript.Add("And, until I have found a way to access the control panel and return home...");
        TextEventScript.Add("I suggest I stay here.");
        TextEventScript.Add("Simply due to the fact that, well...");
        TextEventScript.Add("I am unable to feed myself or give myself pets.");
        TextEventScript.Add("Say what you want about A.I, but virtual sensations are a favorite.");
    }

    protected override void UnlockNextEvents()
    {
        for(int i = 0; i < 1; i++)
        {
            //EventsToBeUnlockedAfterCompletion[i].SetUnlockedStatus(true);
        }
    }
}
