using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Variables
    public static GameManager Instance = null;
    public Dictionary<SceneNames, GameObject> SceneObjectList;
    public Dictionary<SceneNames, SceneEventHandler> SceneHandlerList;

    private bool ReadyForOutsideAccess = false;

    private string PlayerData = "";

    public string SaveFile
    {
        get { return PlayerData; }
        private set { PlayerData = value; }
    }

    public bool Accessible
    {
        get { return ReadyForOutsideAccess; }
        private set { ReadyForOutsideAccess = value; }
    }


    //Enums
    public enum SceneNames
    {
        TitleScreen = 0,
        MainMenu,
        GameView,
        Intro
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

    void Awake()
    {
        //Make the GameManager into a singleton
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
        SceneObjectList = new Dictionary<SceneNames, GameObject>();
        SceneHandlerList = new Dictionary<SceneNames, SceneEventHandler>();
        GatherPossibleScenes();

        //Everything is loaded, and we are ready for outside access
        Accessible = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GatherPossibleScenes()
    {
        GameObject[] TempSceneArray = GameObject.FindGameObjectsWithTag("Scene");
        foreach(GameObject scene in TempSceneArray)
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
                }
            }
            else
            {
                Debug.LogError("Error: found object tagged as a 'Scene' missing either a CanvasGroup or a SceneEventHandler!");
            }
        }
    }

    public void RequestSceneChange(GameObject a_Requester, SceneNames a_Scene)
    {
        bool ValidRequest = ValidateRequester(a_Requester, RequestType.SceneChange);
        bool RequestComplete = false;
        if(ValidRequest)
        {
            RequestComplete = ProcessRequest(a_Requester, a_Scene, RequestType.SceneChange);
        }
        else
        {
            Debug.LogError("Error: Invalid request. Please try again with different parameters.");
        }
    }

    public void RequestSceneStart(GameObject a_Requester)
    {
        bool ValidRequest = ValidateRequester(a_Requester, RequestType.SceneStart);
        bool RequestComplete = false;
        if(ValidRequest)
        {
            RequestComplete = ProcessRequest(a_Requester, SceneNames.Intro, RequestType.SceneStart);
        }
        else
        {
            Debug.LogError("Error: Invalid request. Please try again with different parameters.");
        }
    }

    /// <summary>
    /// Validates the requester of a GameManager function based on the request type.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_Value"></param>
    /// <returns></returns>
    bool ValidateRequester(GameObject a_Requester, RequestType a_Value)
    {
        if(Accessible)
        {
            switch (a_Value)
            {
                case RequestType.SceneChange:
                    if (a_Requester.tag == "Scene")
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
    /// Processes a request for a GameManager function. All calls to non-processing functions inside GameManager should be done through here.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_Scene"></param>
    /// <param name="a_Request"></param>
    /// <returns></returns>
    bool ProcessRequest(GameObject a_Requester, SceneNames a_Scene, RequestType a_Request)
    {
        //If we're switching scenes
        if(a_Request == RequestType.SceneChange)
        {
            if (a_Requester.name == "Title")
            {
                return SwitchScene(SceneNames.TitleScreen, a_Scene);
            }
            else if (a_Requester.name == "Intro")
            {
                return SwitchScene(SceneNames.Intro, a_Scene);
            }
        }
        //If we're starting a scene
        else if(a_Request == RequestType.SceneStart)
        {
            if (a_Requester.name == "Intro")
            {
                return StartScene(SceneHandlerList[SceneNames.Intro]);
            }
        }

        Debug.LogError("Error: Unable to fulfill request.");
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
