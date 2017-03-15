using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    #region Private Variables
    LoadingManager LM;
    private SceneNames CurrentActiveScene = SceneNames.None;
    private bool ReadyForOutsideAccess = false;
    #endregion

    #region Public Variables
    public Dictionary<SceneNames, GameObject> SceneObjectList;
    public Dictionary<SceneNames, SceneEventHandler> SceneHandlerList;
    public SceneNames SceneToUseOnStartup = SceneNames.None;

    public SceneNames ActiveScene
    {
        get { return CurrentActiveScene; }
        private set { CurrentActiveScene = value; }
    }

    public bool Accessible
    {
        get { return ReadyForOutsideAccess; }
        private set { ReadyForOutsideAccess = value; }
    }
    #endregion

    #region Enums
    public enum SceneNames
    {
        None = 0,
        TitleScreen,
        GameView,
        Intro,
        TextMenu,
        Return
    };

    public enum RequestType
    {
        SceneChange = 0,
        SceneStart
    };

    public enum SceneEventType
    {
        SceneHidden = 0,
        SceneRevealed,
        SceneEnded,
        SceneStarted
    };
    #endregion

    // Use this for initialization
    void Start()
    {

        LM = LoadingManager.Instance;
        SceneObjectList = new Dictionary<SceneNames, GameObject>();
        SceneHandlerList = new Dictionary<SceneNames, SceneEventHandler>();
        GatherPossibleScenes();

        Accessible = true;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.SceneManager, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStartRevealScenes(bool a_FirstTime)
    {
        DetermineStartingScene(a_FirstTime);
        DeactivateNonActiveScenes();
    }

    void GatherPossibleScenes()
    {
        //Create a null scene for when the game starts
        CreateNewScene("Null Scene", SceneNames.None);

        //Begin locating and gathering all of the scenes in the game
        GameObject[] TempSceneArray = GameObject.FindGameObjectsWithTag("Scene");
        foreach (GameObject scene in TempSceneArray)
        {
            SceneEventHandler IsHandled = scene.GetComponent<SceneEventHandler>();
            if (scene != null && IsHandled)
            {
                switch (scene.name)
                {
                    case "Title":
                        SceneObjectList.Add(SceneNames.TitleScreen, scene);
                        SceneHandlerList.Add(SceneNames.TitleScreen, IsHandled);
                        break;
                    case "Intro":
                        SceneObjectList.Add(SceneNames.Intro, scene);
                        SceneHandlerList.Add(SceneNames.Intro, IsHandled);
                        break;
                    case "Doggo Menu":
                        SceneObjectList.Add(SceneNames.GameView, scene);
                        SceneHandlerList.Add(SceneNames.GameView, IsHandled);
                        break;
                    case "Text Menu":
                        SceneObjectList.Add(SceneNames.TextMenu, scene);
                        SceneHandlerList.Add(SceneNames.TextMenu, IsHandled);
                        break;
                }
            }
            else
            {
                Debug.LogError("Error: found object tagged as a 'Scene' missing either a CanvasGroup or a SceneEventHandler!");
            }
        }
    }

    void CreateNewScene(string a_SceneName, SceneNames a_SceneClass)
    {
        GameObject TempScene = new GameObject(a_SceneName);
        SceneEventHandler TempSceneScript = null;

        switch (a_SceneClass)
        {
            case SceneNames.None:
                TempSceneScript = TempScene.AddComponent<NullSceneEventHandler>();
                break;
            case SceneNames.Intro:
                TempSceneScript = TempScene.AddComponent<InitializationSequenceEventHandler>();
                break;
            case SceneNames.GameView:
                TempSceneScript = TempScene.AddComponent<DoggoViewEventHandler>();
                break;
            case SceneNames.TextMenu:
                TempSceneScript = TempScene.AddComponent<TextMenuEventHandler>();
                break;
        }

        SceneObjectList.Add(SceneNames.None, TempScene);
        SceneHandlerList.Add(SceneNames.None, TempSceneScript);

        DeactivateNonActiveScenes();
    }

    public void DetermineStartingScene(bool a_FirstTime)
    {
        bool StartingSceneFound = false;
        bool StartingSceneInitialized = false;

        //Check if the player is starting up the game for the first time.
        if (a_FirstTime)
        {
            SceneToUseOnStartup = SceneNames.Intro;
        }
        else
        {
            SceneToUseOnStartup = SceneNames.GameView;
        }

        //Check if there is no scene to use on startup
        if (SceneToUseOnStartup == SceneNames.None)
        {
            Debug.LogError("Error: no startup scene selected");
            return;
        }
        else
        {
            //Turn on the starting scene
            StartingSceneFound = SwitchScene(SceneNames.None, SceneToUseOnStartup);
            StartingSceneInitialized = StartScene(SceneHandlerList[SceneToUseOnStartup]);
        }
    }

    /// <summary>
    /// Sends a request to the Scene Manager, asking to switch from one scene to another.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_Scene"></param>
    public void RequestSceneChange(GameObject a_Requester, SceneNames a_SceneA, SceneNames a_SceneB)
    {
        bool ValidRequest = ValidateRequester(a_Requester, RequestType.SceneChange);
        bool RequestComplete = false;
        if (ValidRequest)
        {
            RequestComplete = ProcessRequest(RequestType.SceneChange, a_SceneA, a_SceneB);
        }
        else
        {
            Debug.LogError("Error: Invalid request. Please try again with different parameters.");
        }
    }
    /// <summary>
    /// Sends a request to the Scene Manager, asking to switch from one scene to another. Send an event to the new scene to automatically play that event
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_SceneA"></param>
    /// <param name="a_SceneB"></param>
    /// <param name="a_Event"></param>
    public void RequestSceneChange_WithEvent(GameObject a_Requester, SceneNames a_SceneA, SceneNames a_SceneB, GameEvent a_Event)
    {
        bool ValidRequest = ValidateRequester(a_Requester, RequestType.SceneChange);
        bool RequestComplete = false;
        if (ValidRequest)
        {
            RequestComplete = ProcessRequest_WithEvent(RequestType.SceneChange, a_SceneA, a_SceneB, a_Event);
        }
        else
        {
            Debug.LogError("Error: Invalid request. Please try again with different parameters.");
        }
    }
    /// <summary>
    /// Sends a request to the Scene Manager, asking to activate a Scene Start event.
    /// </summary>
    /// <param name="a_Requester"></param>
    public void RequestSceneStart(GameObject a_Requester, SceneNames a_Scene)
    {
        bool ValidRequest = ValidateRequester(a_Requester, RequestType.SceneStart);
        bool RequestComplete = false;
        if (ValidRequest)
        {
            RequestComplete = ProcessRequest(RequestType.SceneStart, a_Scene);
        }
        else
        {
            Debug.LogError("Error: Invalid request. Please try again with different parameters.");
        }
    }
    /// <summary>
    /// Validates the requester of a Scene Manager function based on the request type.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_Value"></param>
    /// <returns></returns>
    bool ValidateRequester(GameObject a_Requester, RequestType a_Value)
    {
        if (Accessible)
        {
            switch (a_Value)
            {
                case RequestType.SceneChange:
                    if (a_Requester.tag == "Scene" || a_Requester.tag == "Event Button"
                        || a_Requester.tag == "Manager")
                    {
                        return true;
                    }
                    else
                        return false;
                case RequestType.SceneStart:
                    if (a_Requester.tag == "Scene")
                    {
                        return true;
                    }
                    else
                        return false;
            }

            Debug.LogError("Error 0: Invalid RequestType submitted. Either submit a valid RequestType, or create permissions for the RequestType submitted.");
            return false;
        }

        Debug.LogError("Error: a script tried accessing the GameManager before it was finished loading. Please wait until the GameManager is done loading before attempting to access it.");
        return false;
    }
    /// <summary>
    /// Processes a request for a Scene Manager function. All calls to non-processing functions inside GameManager should be done through here.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_Scene"></param>
    /// <param name="a_Request"></param>
    /// <returns></returns>
    bool ProcessRequest(RequestType a_Request, SceneNames a_SceneA, SceneNames a_SceneB = SceneNames.None)
    {
        bool RequestProcessed = false;

        //If we're switching scenes
        if (a_Request == RequestType.SceneChange)
        {
            RequestProcessed = SwitchScene(a_SceneA, a_SceneB);
        }
        //If we're starting a scene
        else if (a_Request == RequestType.SceneStart)
        {
            RequestProcessed = StartScene(SceneHandlerList[a_SceneA]);
        }
        else
        {
            Debug.LogError("Error: Unable to fulfill request due to invalid request type.");
        }

        return RequestProcessed;
    }
    /// <summary>
    /// Processes a request for a Scene Manager function. All calls to non-processing functions inside GameManager should be done through here. Overwritten to include event startup support.
    /// </summary>
    /// <param name="a_Request"></param>
    /// <param name="a_SceneA"></param>
    /// <param name="a_SceneB"></param>
    /// <param name="a_Event"></param>
    /// <returns></returns>
    bool ProcessRequest_WithEvent(RequestType a_Request, SceneNames a_SceneA, SceneNames a_SceneB = SceneNames.None, GameEvent a_Event = null)
    {
        bool RequestProcessed = false;

        //If we're switching scenes
        if (a_Request == RequestType.SceneChange)
        {
            RequestProcessed = SwitchSceneWithEvent(a_SceneA, a_SceneB, a_Event);
        }
        //If we're starting a scene
        else if (a_Request == RequestType.SceneStart)
        {
            RequestProcessed = StartScene(SceneHandlerList[a_SceneA]);
        }
        else
        {
            Debug.LogError("Error: Unable to fulfill request due to invalid request type.");
        }

        return RequestProcessed;
    }

    bool DeactivateNonActiveScenes()
    {
        foreach (KeyValuePair<SceneNames, GameObject> scene in SceneObjectList)
        {
            if (scene.Key != CurrentActiveScene)
            {
                scene.Value.SetActive(false);
            }
        }
        return false;
    }

    bool StartScene(SceneEventHandler a_Handler)
    {
        a_Handler.OnSceneEvent(SceneEventType.SceneStarted);
        return true;
    }

    bool SwitchScene(SceneNames a_SceneToSwitchFrom, SceneNames a_SceneToSwitchTo)
    {
        //Turn off the previous scene
        TurnOffScene(SceneObjectList[a_SceneToSwitchFrom], SceneHandlerList[a_SceneToSwitchFrom]);

        //Turn on the new scene
        TurnOnScene(SceneObjectList[a_SceneToSwitchTo], SceneHandlerList[a_SceneToSwitchTo]);

        //Set the new, active scene
        CurrentActiveScene = a_SceneToSwitchTo;
        DeactivateNonActiveScenes();

        return true;
    }

    bool SwitchSceneWithEvent(SceneNames a_SceneToSwitchFrom, SceneNames a_SceneToSwitchTo, GameEvent a_Event)
    {
        //Turn off the previous scene
        TurnOffScene(SceneObjectList[a_SceneToSwitchFrom], SceneHandlerList[a_SceneToSwitchFrom]);

        //Turn on the new scene
        TurnOnScene(SceneObjectList[a_SceneToSwitchTo], SceneHandlerList[a_SceneToSwitchTo]);

        //Set the new, active scene
        CurrentActiveScene = a_SceneToSwitchTo;
        DeactivateNonActiveScenes();

        //Send an event to the new scene
        SceneHandlerList[a_SceneToSwitchTo].RequestEventStart(a_Event);

        return true;
    }

    void TurnOnScene(GameObject a_Scene, SceneEventHandler a_Handler)
    {
        a_Scene.SetActive(true);
        a_Handler.OnSceneEvent(SceneEventType.SceneRevealed);
    }

    void TurnOffScene(GameObject a_Scene, SceneEventHandler a_Handler)
    {
        a_Scene.SetActive(false);
        a_Handler.OnSceneEvent(SceneEventType.SceneHidden);
    }
}
