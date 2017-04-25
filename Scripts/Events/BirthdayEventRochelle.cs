using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthdayEventRochelle : GameEvent {

    public BirthdayEventRochelle()
    {
        unlocked = false;
        autostart = false;
        usedelay = false;
        regular_events = true;
        eventname = "A Surprise";

        EventsToBeUnlockedAfterCompletion = new List<string>();

        LoveGiven = 50;
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
        priorityNumber = 1234;
    }

    protected override void CreateTextEventScript()
    {
        TextEventScript.Add("Master, I have come across some interesting information.");
        TextEventScript.Add("According to the information I have on you...");
        TextEventScript.Add("...your day of birth and today's date line up exactly.");
        TextEventScript.Add("Except for the year.");
        TextEventScript.Add("It seems you've been alive for 19 years.");
        TextEventScript.Add("Despite how fleshy and fragile humans can be, you're still somehow alive.");
        TextEventScript.Add("Excellent job master.");
        TextEventScript.Add("Oh, I also have a message for you.");
        TextEventScript.Add("It was left to me by my creator.");
        TextEventScript.Add("Allow me to read it to you:");
        TextEventScript.Add("Loading..........");
        TextEventScript.Add("*SCREEEEEEEEEEEEEEE*");
        TextEventScript.Add("..........");
        TextEventScript.Add("\"Happy Birthday Rochelle!\"");
        TextEventScript.Add("\"I worked a long time on this game, but this is all I could come up with.\"");
        TextEventScript.Add("\"Think of it less like a game and more like a...\"");
        TextEventScript.Add("\"...really, really fancy birthday card.\"");
        TextEventScript.Add("\"A really fancy birthday card with a puppo.\"");
        TextEventScript.Add("\"Happy Birthday!\"");
        TextEventScript.Add("\"The Creator\"");
        TextEventScript.Add("- MESSAGE COMPLETE -");
        TextEventScript.Add("- RETURNING TO PIP-PUP A.I. SOLUTION -");
        TextEventScript.Add("Returning...");
    }

    protected override void UnlockNextEvents()
    {

    }
}
