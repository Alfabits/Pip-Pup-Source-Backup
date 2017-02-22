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

        LM.CheckIn(gameObject, LoadingManager.KeysForScriptsToBeLoaded.EventManager, true);

        yield return null;
    }

    #region PRIVATE_VARIABLES
    private List<EventList> EventListObjects;
    private LoadingManager LM;
    #endregion
}
