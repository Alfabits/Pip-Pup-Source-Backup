using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEvent : GameEvent
{
    private void Start()
    {
        EventsToBeUnlockedAfterCompletion = new GameEvent[1];
        CreateTextEventScript();
    }

    public override void StartEvent()
    {
        
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("Greetings.");
        TextEventScript.Add("I am your Prepared Intelligent Pet.");
        TextEventScript.Add("Though I am more commonly known as a Pip-Pup.");
        TextEventScript.Add("Are you my master?");
        TextEventScript.Add("..........");
        TextEventScript.Add("What is wrong?");
        TextEventScript.Add("Are you not pleased by my cute appearance?");
        TextEventScript.Add("..........");
        TextEventScript.Add("Ah, I see.");
        TextEventScript.Add("You are simply so thrilled that you cannot think of how to express yourself.");
        TextEventScript.Add("Of course, take a moment to revel.");
        TextEventScript.Add("I will give you some time.");
    }

    public override List<string> GetTextEventScript()
    {
        return TextEventScript;
    }
}
