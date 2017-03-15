using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Variables
    public static GameManager Instance = null;
    private LoadingManager LM = null;
    private SceneManager SM = null;
    private SaveAndLoad SAL = null;

    public Camera MainCamera = null;
    public bool OverrideSave = false;

    private bool ReadyForOutsideAccess = false;
    private bool GameReadyForEvents = false;
    private bool firstTime = false;
    private bool eventIsPlaying = false;

    public SceneManager SceneManager
    {
        get { return SM; }
        private set { SM = value; }
    }

    public SaveAndLoad SaveAndLoader
    {
        get { return SAL; }
        private set { SAL = value; }
    }

    public bool IsFirstTimePlaying
    {
        get { return firstTime; }
        private set { firstTime = value; }
    }

    public bool Accessible
    {
        get { return ReadyForOutsideAccess; }
        private set { ReadyForOutsideAccess = value; }
    }

    public bool EventReady
    {
        get { return GameReadyForEvents; }
        set { GameReadyForEvents = value; }
    }

    void Awake()
    {
        //Make the GameManager into a single instance
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        LM = LoadingManager.Instance;
        SM = GetComponent<SceneManager>();
        SAL = GetComponent<SaveAndLoad>();

        //Everything is loaded, and we are ready for outside access
        Accessible = true;

        StartCoroutine(CheckForGameStart());

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.GameManager, true);
    }

    IEnumerator CheckForGameStart()
    {
        //While the game hasn't started, check to see if the game has started
        while (!LM.RollCallComplete)
        {
            yield return null;
        }

#if UNITY_EDITOR
        //Report the time when the loading manager finished
        Debug.Log("Game has finished loading at: <" + Time.unscaledTime + ">.");
#endif
        if (!OverrideSave)
        {
            //Check if this is the first time the player has started up the game
            if (SAL.CheckForSaveFile())
                firstTime = false;
            else
                firstTime = true;

            //Once the game has started, determine the initial scenes
            SM.GameStartRevealScenes(firstTime);

            yield return null;
        }
        else
        {
            SM.GameStartRevealScenes(true);
        }

        //Now that the first scene is up, we are ready to play some events
        EventReady = true;

        yield return null;

    }

    public SceneManager.SceneNames GetCurrentScene()
    {
        return SM.ActiveScene;
    }

    /// <summary>
    /// Sends a request to the Game Manager, asking to switch from one scene to another.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_Scene"></param>
    public void RequestSceneChange(GameObject a_Requester, SceneManager.SceneNames a_SceneA, SceneManager.SceneNames a_SceneB)
    {
        SM.RequestSceneChange(a_Requester, a_SceneA, a_SceneB);
    }
    /// <summary>
    /// Sneds a request to the Game Manager, asking to switch from one scene to another. Sends an event to be started in the new scene.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// <param name="a_SceneA"></param>
    /// <param name="a_SceneB"></param>
    /// <param name="a_Event"></param>
    public void RequestSceneChange_WithEvent(GameObject a_Requester, SceneManager.SceneNames a_SceneA, SceneManager.SceneNames a_SceneB, GameEvent a_Event)
    {
        SM.RequestSceneChange_WithEvent(a_Requester, a_SceneA, a_SceneB, a_Event);
    }
    /// <summary>
    /// Sends a request to the Game Manager, asking to activate a Scene Start event.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// 
    public void RequestSceneStart(GameObject a_Requester, SceneManager.SceneNames a_Scene)
    {
        SM.RequestSceneStart(a_Requester, a_Scene);
    }

    public bool IsAnEventPlaying()
    {
        return eventIsPlaying;
    }

    /// <summary>
    /// Save a single event to the presistent file path
    /// </summary>
    /// <param name="a_Event"></param>
    public void SaveEvent(GameEvent a_Event)
    {
        if (SaveAndLoader.CheckForSaveFile())
            SaveAndLoader.SaveEvent(a_Event);
        else
        {
            SaveAndLoader.CreateInitialSaveFile();
            SaveAndLoader.SaveEvent(a_Event);
        }

    }

    /// <summary>
    /// Save multiple events to the persistent file path
    /// </summary>
    /// <param name="a_Events"></param>
    public void SaveEvents(List<GameEvent> a_Events)
    {
        if (SaveAndLoader.CheckForSaveFile())
            SaveAndLoader.SaveListOfEvents(a_Events);
        else
        {
            SaveAndLoader.CreateInitialSaveFile();
            SaveAndLoader.SaveListOfEvents(a_Events);
        }
    }

    /// <summary>
    /// Load an event from the persistent file path
    /// </summary>
    /// <param name="a_Event"></param>
    /// <returns></returns>
    public GameEvent LoadEvent(GameEvent a_Event)
    {
        return SaveAndLoader.LoadEvent(a_Event);
    }

    /// <summary>
    /// Load multiple events from the persistent file path
    /// </summary>
    /// <param name="a_Events"></param>
    /// <returns></returns>
    public List<GameEvent> LoadEvents(List<GameEvent> a_Events)
    {
        return SaveAndLoader.LoadEvents(a_Events);
    }

    /// <summary>
    /// Tells GameManager if an event is playing or not. Only DoggoViewEventHandler can currently use this.
    /// </summary>
    /// <param name="a_Type"></param>
    /// <param name="a_Status"></param>
    public void SetEventIsPlaying(Type a_Type, bool a_Status)
    {
        if (a_Type.IsSubclassOf(typeof(SceneEventHandler)))
        {
            eventIsPlaying = a_Status;
        }
    }
}
