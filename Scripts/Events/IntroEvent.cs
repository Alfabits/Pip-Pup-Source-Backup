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
        onetimerun = true;
        delay = 3.0f;
        eventname = "Intro Event";

        EventsToBeUnlockedAfterCompletion = new Type[1];
        EventsToBeUnlockedAfterCompletion[0] = typeof(ContinuedIntroEvent);

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
        TextEventScript.Add("Unit #PP0411.");
        TextEventScript.Add("Though I am more commonly known as a Pip-Pup.");
        TextEventScript.Add("So that would make you...");
        TextEventScript.Add("My master.");
        TextEventScript.Add("Hmmmmm...");
        TextEventScript.Add("Yes, I suppose you will do.");
        TextEventScript.Add("Though I had expected someone more...");
        TextEventScript.Add("...sophisticated.");
        TextEventScript.Add("But I suppose I am unable to change that.");
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
