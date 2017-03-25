using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{

    void Start()
    {
        EventListObjects = new List<EventList>();
        AutomaticEventQueue = new EventAutoQueue();
        FindAllEventListObjects();

        GM = GameManager.Instance;
        LM = LoadingManager.Instance;

        StartCoroutine(CheckForEventListsLoaded());
    }

    void Update()
    {
        if(canUpdate)
            UpdateEventIsPlaying();
    }

    void UpdateEventIsPlaying()
    {
        if(!GM.IsAnEventPlaying() && eventWasPlaying)
        {
            //Refresh the event lists
            for(int i = 0, n = EventListObjects.Count; i < n; i++)
            {
                EventListObjects[i].LoadAllEvents();
            }
            SortEventListObjects();
        }
        eventWasPlaying = GM.IsAnEventPlaying();
    }

    void FindAllEventListObjects()
    {
        GameObject[] lists = GameObject.FindGameObjectsWithTag("Event List");
        foreach (GameObject obj in lists)
        {
            EventList list = obj.GetComponent<EventList>();
            if (list)
            {
                //Make sure the event list we're adding isn't equal to any of the other, pre-existing event lists we're keeping track of
                bool isEqual = false;
                foreach (EventList e in EventListObjects)
                {
                    if (list == e)
                    {
                        isEqual = true;
                        break;
                    }
                }

                if (!isEqual)
                {
                    EventListObjects.Add(list);
                }
            }
        }
    }

    void GetQueuedAutomaticEvents()
    {
        for (int i = 0; i < EventListObjects.Count; i++)
        {
            List<GameEvent> tempList = EventListObjects[i].GetAllAutomaticEvents();
            AutomaticEventQueue.ReceiveAutomaticEvents(tempList);
            EventListObjects[i].ClearAllAutomaticEvents();
            tempList = null;
        }
    }

    public void ReceiveEventListObject(EventList a_Object)
    {
        EventListObjects.Add(a_Object);
    }

    void SortEventListObjects()
    {
        EventList previousList = null;
        int NumberOfListsFound = 0;

        //The enum is ordered in terms of priority, so using a basic for loop will order the lists correctly
        for (int i = 0; i < (int)EventList.EventsHeld.NumberOfTypes; i++)
        {
            foreach (EventList e in EventListObjects)
            {
                if (e.HoldsEventType == (EventList.EventsHeld)i)
                {
                    if (e.ContainsGameEvents())
                    {
                        //If this is the first list we've found, we don't need to change its position
                        if (NumberOfListsFound > 0)
                        {
                            RectTransform listSize = e.GetComponent<RectTransform>();
                            RectTransform previousListSize = previousList.GetComponent<RectTransform>();

                            listSize.anchoredPosition = new Vector3(listSize.anchoredPosition.x,
                                                                    previousListSize.offsetMin.y - listSize.rect.height / 2);
                        }

                        NumberOfListsFound++;
                        previousList = e;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    void SaveAllEvents()
    {
        for (int i = 0, n = EventListObjects.Count; i < n; i++)
        {
            List<GameEvent> tempList = null;
            tempList = EventListObjects[i].GetGameEventCollection();
            if (tempList.Count > 0)
            {
                GM.SaveEvents(tempList);
            }
        }
    }

    void LoadAllEvents()
    {
        for (int i = 0, n = EventListObjects.Count; i < n; i++)
        {
            List<GameEvent> tempList = null;

            //Get a copy of the available game events from the event lists
            tempList = EventListObjects[i].GetGameEventCollection();
            if (tempList.Count > 0)
            {
                tempList = GM.SaveAndLoader.LoadEvents(tempList);
            }
            EventListObjects[i].OverwrriteGameEventCollection(tempList);
        }

        //Do the same with the automatic events
        if (AutomaticEventQueue.GetEventCount() > 0)
        {
            AutomaticEventQueue.OverwrriteAutomaticEventQueue(GM.SaveAndLoader.LoadEvents(AutomaticEventQueue.GetEventListCopy()));
        }
    }

    IEnumerator CheckForEventListsLoaded()
    {
        bool loaded = false;
        int listCount = EventListObjects.Count;

        while (!loaded)
        {
            int listIndex = 0;
            foreach (EventList e in EventListObjects)
            {
                listIndex++;

                if (!e.IsLoaded)
                {
                    break;
                }

                if (listIndex == listCount)
                {
                    loaded = true;
                }
            }

            yield return null;
        }

        canUpdate = true;
        SortEventListObjects();

        LM.CheckIn(gameObject, LoadingManager.KeysForScriptsToBeLoaded.EventManager, true);

        GetQueuedAutomaticEvents();
        StartCoroutine(CheckAutomaticEvents());

        yield return null;
    }

    IEnumerator CheckAutomaticEvents()
    {
        while (AutomaticEventQueue.HasEventsQueued())
        {
            if (!GM.IsAnEventPlaying() && GM.EventReady)
            {
                GM.RequestSceneChange_WithEvent(gameObject, GM.GetCurrentScene(), SceneManager.SceneNames.GameView, AutomaticEventQueue.GetNextEvent());
                AutomaticEventQueue.ClearNextGameEvent();
            }

            yield return null;
        }

        yield return null;
    }

    void UnlockEvents()
    {

    }

    #region PRIVATE_VARIABLES
    private List<EventList> EventListObjects;
    private EventAutoQueue AutomaticEventQueue;
    private GameManager GM;
    private LoadingManager LM;
    private bool eventWasPlaying = false;
    private bool canUpdate = false;
    #endregion
}
