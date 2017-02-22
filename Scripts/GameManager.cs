using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Variables
    public static GameManager Instance = null;
    LoadingManager LM = null;
    SceneManager SM = null;
    SaveAndLoad SAL = null;

    public Camera MainCamera = null;

    private bool ReadyForOutsideAccess = false;
    bool firstTime = false;

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
        while(!LM.RollCallComplete)
        {
            yield return null;
        }

#if UNITY_EDITOR
        //Report the time when the loading manager finished
        Debug.Log("Game has finished loading at: <" + Time.unscaledTime + ">.");
#endif

        //Check if this is the first time the player has started up the game
        if (SAL.CheckForSaveFile())
            firstTime = false;
        else
            firstTime = true;

        //Once the game has started, determine the initial scenes
        SM.GameStartRevealScenes(firstTime);

        yield return null;
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
    /// Sends a request to the Game Manager, asking to activate a Scene Start event.
    /// </summary>
    /// <param name="a_Requester"></param>
    /// 
    public void RequestSceneStart(GameObject a_Requester, SceneManager.SceneNames a_Scene)
    {
        SM.RequestSceneStart(a_Requester, a_Scene);
    }
}
