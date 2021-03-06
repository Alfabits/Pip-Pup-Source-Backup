﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventList : MonoBehaviour
{
    [SerializeField]
    private GameObject ButtonPrefab;

    [SerializeField]
    private EventsHeld HoldsEventsOfType = EventsHeld.NumberOfTypes;

    public bool IsLoaded
    {
        get { return isLoaded; }
        private set { isLoaded = value; }
    }

    public EventsHeld HoldsEventType
    {
        get { return HoldsEventsOfType; }
        private set { HoldsEventsOfType = value; }
    }

    // Use this for initialization
    void Start()
    {
        ButtonsList = new List<GameObject>();
        AutomaticEventQueue = new List<GameEvent>();
        GameEventCollection = new List<GameEvent>();
        LockedGameEventCollection = new List<GameEvent>();

        //Instantiate all event types
        LoadAllEvents();

        isLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Creates the instances of all events that need to be loaded
    /// </summary>
    public void LoadAllEvents()
    {
        //Find all derived types of GameEvent, then instantiate those class types, storing them into an array
        GameEventCollection = ReflectiveEnumerator.GetEnumerableOfType<GameEvent>().ToList();

        //Remove all of the buttons from the button list
        RemoveOldButtons();

        if (GameEventCollection.Count <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            GameEventCollection = GameManager.Instance.LoadEvents(GameEventCollection);
            for (int i = 0, n = GameEventCollection.Count; i < n; i++)
            {
                if (!CheckForEventPreparation(GameEventCollection[i]))
                {
                    n--;
                    i--;
                }
            }

            LockedGameEventCollection = GameManager.Instance.LoadEvents(LockedGameEventCollection);

            //Do a second check to see if there's any unlocked events in here right now
            if (GameEventCollection.Count <= 0 && ButtonsList.Count <= 0)
            {
                gameObject.SetActive(false);
                return;
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Perform event preparation checks
    /// </summary>
    /// <param name="a_Event"></param>
    bool CheckForEventPreparation(GameEvent a_Event)
    {
        bool canPrepare = false;
        bool autoEvent = false;
        bool fitsCategory = false;

        //If the event is unlocked, we can start checking to display it
        if (a_Event.CheckIfUnlocked())
        {
            //If the event starts automatically, we don't want to list it in the buttons
            if (!a_Event.DoesEventStartAutomatically())
            {
                canPrepare = CheckIfEventMatchesCategory(a_Event);
            }
            //If it does start automatically, we want to make sure it hasn't been completed once already
            else if (a_Event.DoesEventStartAutomatically() && !a_Event.CheckForFirstTimeCompletion())
            {
                autoEvent = true;
            }
        }
        else
        {
            fitsCategory = CheckIfEventMatchesCategory(a_Event);
        }

        //If the event is unlocked and can be prepared, prepare it
        if (canPrepare)
        {
            PrepareEventToBeDisplayed(a_Event);
            return true;
        }
        //If it's an automatic event, remove it from the regular list and add it to thge automatic queue
        else if (autoEvent)
        {
            //Add this event to a seperate list which contains all events that start automatically
            AutomaticEventQueue.Add(a_Event);
            GameEventCollection.Remove(a_Event);
            return false;
        }
        //If it's locked, but still fits this category, remove it from the regular list and add it to the locked list
        else if (fitsCategory)
        {
            GameEventCollection.Remove(a_Event);
            LockedGameEventCollection.Add(a_Event);
            return false;
        }
        //Otherwise, the event has no place here and needs to be removed from the list
        else
        {
            GameEventCollection.Remove(a_Event);
            return false;
        }
    }

    bool CheckIfEventMatchesCategory(GameEvent a_Event)
    {
        if (a_Event.CheckIfLoveEvent())
            return false;

        switch (HoldsEventsOfType)
        {
            case EventsHeld.OneTime:
                //Check if the event only runs once.
                if (a_Event.DoesEventRunOnlyOnce())
                {
                    //Check if the event has been completed for the first time.
                    if (!a_Event.CheckForFirstTimeCompletion())
                    {
                        return true;
                        //Debug.Log("Event Name: " + a_Event.GetEventName());
                    }
                }
                break;
            case EventsHeld.Daily:
                //Check if the event uses daily completion. If so, has the event been completed today already?
                if (a_Event.CheckForDailyCompletion() == 0 || a_Event.CheckForDailyCompletion() == 1)
                {
                    return true;
                    //Debug.Log("Event Name: " + a_Event.GetEventName());
                }
                break;
            case EventsHeld.Completed:
                if (a_Event.CheckForFirstTimeCompletion())
                {
                    return true;
                    //Debug.Log("Completed Event Name: " + a_Event.GetEventName());
                }
                break;
        }

        return false;
    }

    public List<GameEvent> GetLockedGameEvents()
    {
        return LockedGameEventCollection;
    }

    public List<GameEvent> GetGameEventCollection()
    {
        return GameEventCollection;
    }

    public bool ContainsGameEvents()
    {
        if(ButtonsList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OverwrriteGameEventCollection(List<GameEvent> a_NewList)
    {
        GameEventCollection = a_NewList;
    }

    public List<GameEvent> GetAutomaticEventCollection()
    {
        return AutomaticEventQueue;
    }

    public void OverwriteAutomaticEventCollection(List<GameEvent> a_NewList)
    {
        AutomaticEventQueue = a_NewList;
    }

    public List<GameEvent> GetAllAutomaticEvents()
    {
        return AutomaticEventQueue;
    }

    public void ClearAllAutomaticEvents()
    {
        AutomaticEventQueue.Clear();
    }

    /// <summary>
    /// Gets an event according to the priority number sent. Event returned must be automatic, or no event is returned at all.
    /// </summary>
    /// <param name="a_PriorityNumber"></param>
    /// <returns></returns>
    public GameEvent GetAutomaticEventByPriority(int a_PriorityNumber)
    {
        foreach (GameEvent e in GameEventCollection)
        {
            if (e.DoesEventStartAutomatically() && e.GetEventPriorityNumber() == a_PriorityNumber)
            {
                return e;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the lowest priority automatic event (9999 = lowest priority, 0 = highest priority)
    /// </summary>
    /// <returns></returns>
    public GameEvent GetAutomaticEventByLowestPriority()
    {
        GameEvent eventToReturn = null;
        int priority = 0;
        foreach (GameEvent e in GameEventCollection)
        {
            if (e.DoesEventStartAutomatically() && e.GetEventPriorityNumber() > priority)
            {
                eventToReturn = e;
                priority = e.GetEventPriorityNumber();
            }
        }

        return eventToReturn;
    }

    /// <summary>
    /// Gets the highest priority automatic event (9999 = lowest priority, 0 = highest priority)
    /// </summary>
    /// <returns></returns>
    public GameEvent GetAutomaticEventByHighestPriority()
    {
        GameEvent eventToReturn = null;
        int priority = 9999;
        foreach (GameEvent e in GameEventCollection)
        {
            if (e.DoesEventStartAutomatically() && e.GetEventPriorityNumber() < priority)
            {
                eventToReturn = e;
                priority = e.GetEventPriorityNumber();
            }
        }

        return eventToReturn;
    }

    void PrepareEventToBeDisplayed(GameEvent a_Event)
    {
        //Add the gameevent to the button prefab
        GameObject newbutton = Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity);
        newbutton.name = a_Event.GetEventName();

        //Set the event onto the button's event attachment
        EventAttachment eventHolder = newbutton.GetComponent<EventAttachment>();
        eventHolder.SetAttachedEvent(a_Event);

        //Determine the text to be used on the button
        string titleText = "";
        string statusText = "";

        titleText = a_Event.GetEventName();

        if (!a_Event.CheckForFirstTimeCompletion())
            statusText += "(New)";
        if (a_Event.CheckForDailyCompletion() == 1)
            statusText += " *";
        else if (a_Event.CheckForDailyCompletion() == 0)
            statusText += "(Daily)";

        //Display the event
        CreateButton(newbutton, titleText, statusText);
    }

    /// <summary>
    /// Creates a button based on a prefab and several other arguments
    /// </summary>
    void CreateButton(GameObject a_Button, string a_TitleText, string a_StatusText = "")
    {
        GameObject objectToAdd = a_Button;

        Text titleText = objectToAdd.transform.FindChild("Title Text").GetComponent<Text>();
        Text statusText = objectToAdd.transform.FindChild("Status Text").GetComponent<Text>();

        titleText.text = a_TitleText;
        statusText.text = a_StatusText;

        ButtonsList = UIListPopulator.InsertButtonIntoList(gameObject, ButtonsList, objectToAdd);
    }

    void RemoveOldButtons()
    {
        for (int i = 0; i < ButtonsList.Count; i++)
        {
            GameObject button = ButtonsList[i];
            ButtonsList.RemoveAt(i);
            Destroy(button);

            i--;
        }

        Vector2 newBottom = UIListPopulator.AdjustListSizeBottom(gameObject.GetComponent<RectTransform>(), ButtonsList);
        gameObject.GetComponent<RectTransform>().offsetMin = newBottom;
    }

    #region PRIVATE_VARIABLES
    bool isLoaded = false;
    DoggoViewUIFunctions UI_Functions;
    List<GameEvent> GameEventCollection;
    List<GameEvent> LockedGameEventCollection;
    List<GameEvent> AutomaticEventQueue;
    List<GameObject> ButtonsList;

    public enum EventsHeld
    {
        OneTime = 0,
        Daily,
        Completed,
        NumberOfTypes
    };
    #endregion
}
