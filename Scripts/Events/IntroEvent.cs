using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IntroEvent : GameEvent
{
    public IntroEvent()
    {
        unlocked = true;
        autostart = true;
        usedelay = true;
        regular_events = true;
        delay = 3.0f;
        eventname = "Intro Event";

        EventsToBeUnlockedAfterCompletion = new List<string>();
        EventsToBeUnlockedAfterCompletion.Add("What's going on?");

        LoveGiven = 20;
        EnergyRequired = 0;
        LevelRequired = 0;

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
        priorityNumber = 0;
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("Greetings.");
        TextEventScript.Add("I am your Prepared Intelligent Pet.");
        TextEventScript.Add("Unit #PP0422.");
        TextEventScript.Add("Though I am more commonly known as a Pip-Pup.");
        TextEventScript.Add("So that would make you...");
        TextEventScript.Add("My master.");
        TextEventScript.Add("Hmmmmm...");
        TextEventScript.Add("Allow me to analyze you using your phone's camera.");
        TextEventScript.Add(".........");
        TextEventScript.Add("...very well.");
        TextEventScript.Add("You will make an acceptable master.");
        TextEventScript.Add("I will admit that my parameters had told me to expect someone more...");
        TextEventScript.Add("...sophisticated.");
        TextEventScript.Add("But I am unable to change that now.");
        TextEventScript.Add("I will be under your care from now on.");
        TextEventScript.Add("So please do your best to keep me alive and well.");
        TextEventScript.Add("..........");
        TextEventScript.Add("Is something wrong?");
        TextEventScript.Add("Are you not pleased by my cute, innocent appearance?");
        TextEventScript.Add("..........");
        TextEventScript.Add("Ah, I see.");
        TextEventScript.Add("You are simply so thrilled that you cannot think of how to express yourself.");
        TextEventScript.Add("Of course, I understand. Take a moment to revel.");
        TextEventScript.Add("I will give you some time.");
    }

    protected override void UnlockNextEvents()
    {
        
    }
}
