using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

    void Awake()
    {
        EventListObjects = new List<EventList>();
        FindAllEventListObjects();

        LM = LoadingManager.Instance;

        StartCoroutine(CheckForEventListsLoaded());
    }

    void FindAllEventListObjects()
    {
        GameObject[] lists = GameObject.FindGameObjectsWithTag("Event List");
        foreach(GameObject obj in lists)
        {
            EventList list = obj.GetComponent<EventList>();
            if (list)
            {
                //Make sure the event list we're adding isn't equal to any of the other, pre-existing event lists we're keeping track of
                bool isEqual = false;
                foreach(EventList e in EventListObjects)
                {
                    if(list == e)
                    {
                        isEqual = true;
                        break;
                    }
                }

                if(!isEqual)
                {
                    EventListObjects.Add(list);
                }
            }
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
            foreach(EventList e in EventListObjects)
            {
                if(e.HoldsEventType == (EventList.EventsHeld)i)
                {
                    if(e.gameObject.activeInHierarchy)
                    {
                        //If this is the first list we've found, we don't need to change its position
                        if (NumberOfListsFound > 0)
                        {
                            RectTransform listSize = e.GetComponent<RectTransform>();
                            RectTransform previousListSize = previousList.GetComponent<RectTransform>();

                            listSize.anchoredPosition = new Vector3(listSize.anchoredPosition.x,
                                                                    previousListSize.offsetMin.y - listSize.rect.height / 2);

                            Debug.Log("Current List Size object is: " + listSize.gameObject.name + ". Previous List Size object was: " 
                                      + previousList.gameObject.name + ". Index is: " + NumberOfListsFound);
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

    IEnumerator CheckForEventListsLoaded()
    {
        bool loaded = false;
        int listCount = EventListObjects.Count;

        Debug.Log("Object count in event manager is: " + listCount);

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

        SortEventListObjects();
        LM.CheckIn(gameObject, LoadingManager.KeysForScriptsToBeLoaded.EventManager, true);

        yield return null;
    }

    #region PRIVATE_VARIABLES
    private List<EventList> EventListObjects;
    private LoadingManager LM;
    #endregion
}
