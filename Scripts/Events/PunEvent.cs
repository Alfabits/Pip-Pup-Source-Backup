using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunEvent : GameEvent {

    public PunEvent()
    {
        unlocked = false;
        dailycompleted = 0;
        autostart = false;
        usedelay = false;
        regular_events = true;
        eventname = "What are puns?";

        EventsToBeUnlockedAfterCompletion = new List<string>();

        LoveGiven = 15;
        EnergyRequired = 10;
        LevelRequired = 3;

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
        priorityNumber = 16;
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("Master, I have an inquiry for you.");
        TextEventScript.Add("It pertains to the aspects of comedy used by humans.");
        TextEventScript.Add("Specifically, in regards to what you call \"puns\".");
        TextEventScript.Add("Frankly, master, I find myself confused whenever they come up.");
        TextEventScript.Add("Space rocks, for instance, do not taste better than earth rocks.");
        TextEventScript.Add("They contain no meat and would need to be surgically removed, if swallowed.");
        TextEventScript.Add("What do you think, master?");
        TextEventScript.Add("Do puns make you laugh?");
        TextEventScript.Add(".........");
        TextEventScript.Add("They do?");
        TextEventScript.Add("Interesting.");
        TextEventScript.Add("In that case, would you indulge me in a few?");
        TextEventScript.Add("No particular reason. I simply wish to understand comedy.");
        TextEventScript.Add(".........");
        TextEventScript.Add("I don't know, master, what is the worst part of planning a party in space?");
        TextEventScript.Add(".........");
        TextEventScript.Add("Ah, I see; you have to \"planet\".");
        TextEventScript.Add("Excellent work, master.");
        TextEventScript.Add("Perhaps next time we can think of a more sophisticated joke.");
        TextEventScript.Add("Maybe one involving actual comedy.");
    }

    protected override void UnlockNextEvents()
    {
        for (int i = 0; i < 1; i++)
        {
            //EventsToBeUnlockedAfterCompletion[i].SetUnlockedStatus(true);
        }
    }
}
