using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMenuEventHandler : SceneEventHandler {

    #region Private Variables
    Dictionary<string, GameEvent> GameEventDictionary;
    #endregion

    // Use this for initialization
    void Start () {
        GameEventDictionary = new Dictionary<string, GameEvent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnSceneEvent(SceneManager.SceneEventType a_Event)
    {
        if(a_Event == SceneManager.SceneEventType.SceneHidden)
        {
            SceneIsCurrentlyActive = false;
        }
        if(a_Event == SceneManager.SceneEventType.SceneRevealed)
        {
            SceneIsCurrentlyActive = true;
        }
    }

    public override void RequestEventStart(GameEvent a_Event)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gives an event to the handler for later use. Populates event list directly afterwards.
    /// </summary>
    /// <param name="a_Event"></param>
    public void ReceiveEvent(GameEvent a_Event, bool a_New, bool a_DailyComplete = false)
    {
        //Add the event to the current event dictionary
        GameEventDictionary.Add(a_Event.GetEventName(), a_Event);

        //Populate the event list in the game with the new event
        Populate();
    }

    /// <summary>
    /// Gives events to the handler for later use. Populates event list directly afterwards.
    /// </summary>
    /// <param name="a_Events"></param>
    public void ReceiveEvents(IEnumerable<GameEvent> a_Events)
    {
        //Add the events to the current event dictionary
        foreach(GameEvent e in a_Events)
        {
            GameEventDictionary.Add(e.GetEventName(), e);
        }

        //Populate the event list in the game with the new events
        Populate();
    }

    /// <summary>
    /// Gives events to the handler for later use. Populates event list directly afterwards.
    /// </summary>
    /// <param name="a_Events"></param>
    public void ReceiveEvents(List<GameEvent> a_Events)
    {
        //Add the events to the current event dictionary
        foreach (GameEvent e in a_Events)
        {
            GameEventDictionary.Add(e.GetEventName(), e);
        }

        //Populate the event list in the game with the new events
        Populate();
    }

    /// <summary>
    /// Take all of the events in the GameEventDictionary and create buttons for them in the EventButtonManager
    /// </summary>
    void Populate()
    {
        foreach(KeyValuePair<string, GameEvent> e in GameEventDictionary)
        {
            
        }
    }
}
