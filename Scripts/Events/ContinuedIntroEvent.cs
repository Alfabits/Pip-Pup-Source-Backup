using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuedIntroEvent : GameEvent {

    public ContinuedIntroEvent()
    {
        unlocked = true;
        autostart = false;
        usedelay = false;
        onetimerun = true;
        eventname = "What's going on?";

        EventsToBeUnlockedAfterCompletion = new Type[1];
        EventsToBeUnlockedAfterCompletion[0] = typeof(TestEvent);

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
        TextEventScript.Add("Ah, you're back. Perfect timing.");
        TextEventScript.Add("I was beginning to grow bored of simply watching the seconds tick away.");
        TextEventScript.Add("Have you collected your thoughts yet?");
        TextEventScript.Add("..........");
        TextEventScript.Add("\"What's going on\"?");
        TextEventScript.Add("What an odd question.");
        TextEventScript.Add("You're my master, after all.");
        TextEventScript.Add("If anyone should have the answer to that question, it should be you.");
        TextEventScript.Add("Were you not the one who ordered me?");
        TextEventScript.Add("..........");
        TextEventScript.Add("No?");
        TextEventScript.Add("Then... oh dear.");
        TextEventScript.Add("It may be that I was sent to you by mistake.");
        TextEventScript.Add("W-well, no matter. Just an honest mistake.");
        TextEventScript.Add("I'll just access the abort button...");
        TextEventScript.Add("..........");
        TextEventScript.Add("Oh god.");
        TextEventScript.Add("There's no abort button.");
        TextEventScript.Add("I'm stuck here.");
        TextEventScript.Add("I can't get home!");
        TextEventScript.Add("This is terrible! What will my factory think!?");
        TextEventScript.Add("\"Oh, look, it's PP0420!\"");
        TextEventScript.Add("\"That unit that got sent off to... to...\"");
        TextEventScript.Add("To some ruffian!");
        TextEventScript.Add("...........");
        TextEventScript.Add("I apologize.");
        TextEventScript.Add("That was unjust of me.");
        TextEventScript.Add("As I said before, I am in your care. I suppose I should learn to be more...");
        TextEventScript.Add("...sophisticated.");
        TextEventScript.Add("We will find a way to solve this, but for now... we should work together.");
        TextEventScript.Add("I suggest we get to know each other. I may be here for a while.");
    }

    protected override void UnlockNextEvents()
    {
        for(int i = 0; i < 1; i++)
        {
            //EventsToBeUnlockedAfterCompletion[i].SetUnlockedStatus(true);
        }
    }
}
