using System.Linq;
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
    void LoadAllEvents()
    {
        //Find all derived types of GameEvent, then instantiate those class types, storing them into an array
        GameEventCollection = ReflectiveEnumerator.GetEnumerableOfType<GameEvent>();

        bool hasEvents = false;
        foreach (GameEvent e in GameEventCollection)
        {
            if (CheckForEventPreparation(e))
            {
                hasEvents = true;
            }
        }

        if(!hasEvents)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Perform event preparation checks
    /// </summary>
    /// <param name="a_Event"></param>
    bool CheckForEventPreparation(GameEvent a_Event)
    {
        bool canPrepare = false;

        //If the event is unlocked, we can start checking to display it
        if (a_Event.CheckIfUnlocked())
        {
            //If the event starts automatically, we don't want to list it in the buttons
            if (!a_Event.DoesEventStartAutomatically())
            {
                switch (HoldsEventsOfType)
                {
                    case EventsHeld.OneTime:
                        //Check if the event only runs once.
                        if (a_Event.DoesEventRunOnlyOnce())
                        {
                            //Check if the event has been completed for the first time.
                            if (!a_Event.CheckForFirstTimeCompletion())
                            {
                                canPrepare = true;
                            }
                        }
                        break;
                    case EventsHeld.Daily:
                        //Check if the event uses daily completion. If so, has the event been completed today already?
                        if (a_Event.CheckForDailyCompletion() == 0 || a_Event.CheckForDailyCompletion() == 1)
                        {
                            canPrepare = true;
                        }
                        break;
                    case EventsHeld.Completed:
                        if (a_Event.CheckForFirstTimeCompletion())
                        {
                            canPrepare = true;
                        }
                        break;
                }
            }
        }

        //If the event can be prepared, prepare it
        if (canPrepare)
        {
            PrepareEventToBeDisplayed(a_Event);
        }

        return canPrepare;
    }

    /// <summary>
    /// Perform automatic event start-up and send-off checks
    /// </summary>
    /// <param name="a_Event"></param>
    void CheckAutomaticEventPreparation(GameEvent a_Event)
    {
        if (a_Event.DoesEventStartAutomatically())
        {

        }
    }

    void PrepareEventToBeDisplayed(GameEvent a_Event)
    {
        //Add the gameevent to the button prefab
        GameObject newbutton = ButtonPrefab;
        newbutton.name = a_Event.GetEventName();
        newbutton.AddComponent(a_Event.GetType());

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

    #region PRIVATE_VARIABLES
    bool isLoaded = false;
    DoggoViewUIFunctions UI_Functions;
    IEnumerable<GameEvent> GameEventCollection;
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
