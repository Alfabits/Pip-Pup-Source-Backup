using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour {

    //A global instance of the Loading Manager. Consistent across all scenes
    public static LoadingManager Instance = null;
    public bool Accessible
    {
        get { return accessible; }
        private set { accessible = value; }
    }
    bool accessible = false;

    //Publicly accessible bool that tells other scripts if it is okay to start the game
    bool rollcall = false;
    public bool RollCallComplete
    {
        get { return rollcall; }
        private set { rollcall = value; }
    }

    //Script Enum
    public enum KeysForScriptsToBeLoaded
    {
        None = 0,
        GameManager,
        SceneManager,
        InitializationSequenceEventHandler,
        DoggoViewEventHandler,
        TextRevealLetterByLetter,
        TextRevealLettterByLetterInGame,
        DoggoTouchEventHandler,
        TouchAndDragDoggo,
        BoopDoggo,
        DoggoViewUIFunctions,
        FloatingTextGenerator,
        LoadingBar,
        BoundaryCreationAndManagement,
        EventManager,
        TouchAndScroll,
        NumberOfTypes
    };

    //A dictionary with a key for every scene. Holds whether or not the corresponding scene has finished loading
    Dictionary<KeysForScriptsToBeLoaded, bool> GlobalLoadingStatusDictionary = new Dictionary<KeysForScriptsToBeLoaded, bool>();

    private void Awake()
    {
        //Make the globally accessible instance
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        //Starting from the first scene (not zero, as that is a null value), create dictionary entries for each scene to be loaded
        for (int i = 1; i < (int)KeysForScriptsToBeLoaded.NumberOfTypes; i++)
        {
            GlobalLoadingStatusDictionary.Add((KeysForScriptsToBeLoaded)i, false);
        }

        Accessible = true;
    }

    /// <summary>
    /// Get a key value corresponding to your GameObject name. GameObject must be named the same as a key value for this to work. Returns Key.None if no key can be found.
    /// </summary>
    /// <param name="a_GameObj"></param>
    /// <returns></returns>
    public KeysForScriptsToBeLoaded GetKeyBasedOnGameObject(GameObject a_GameObj)
    {
        KeysForScriptsToBeLoaded key = KeysForScriptsToBeLoaded.None;
        string Name = a_GameObj.name;

        for (int i = 0; i < (int)KeysForScriptsToBeLoaded.NumberOfTypes; i++)
        {
            KeysForScriptsToBeLoaded KeyToBeConverted = (KeysForScriptsToBeLoaded)i;
            string ConvertedEnumName = KeyToBeConverted.ToString();
            if(Name == ConvertedEnumName)
            {
                key = KeyToBeConverted;
                break;
            }
        }

        return key;
    }

    /// <summary>
    /// Allows certain GameObjects to check in to the LoadingManager's list. All scripts must check in before the game can start.
    /// </summary>
    /// <param name="a_GameObj"></param>
    /// <param name="a_Key"></param>
    /// <param name="a_Loaded"></param>
    public void CheckIn(GameObject a_GameObj, KeysForScriptsToBeLoaded a_Key, bool a_Loaded)
    {
        //Check if the one checking in is a valid requester
        //Debug.Log(a_GameObj.name + " checking in with a status of " + a_Loaded + "!");
        GlobalLoadingStatusDictionary[a_Key] = a_Loaded;

        //Do a rollcall, to see if everyone is present;
        RollCall();
    }

    void RollCall()
    {
        bool IsEveryonePresent = true;

        for (int i = 1; i < (int)KeysForScriptsToBeLoaded.NumberOfTypes; i++)
        {
            if(GlobalLoadingStatusDictionary[(KeysForScriptsToBeLoaded)i] == false)
            {
                IsEveryonePresent = false;
            }
            if(GlobalLoadingStatusDictionary[(KeysForScriptsToBeLoaded)i] == true)
            {
                
            }
        }

        //Debug.Log("Ending with an overall status of " + IsEveryonePresent + ".");
        RollCallComplete = IsEveryonePresent;
    }
}
