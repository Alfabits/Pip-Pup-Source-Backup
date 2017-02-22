using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoggoViewEventHandler : SceneEventHandler
{
    //Public Variables
    [SerializeField]
    private TextRevealLetterByLetterInGame TextRevealerScript;
    [SerializeField]
    private DoggoViewUIFunctions UI_Functions;
    [SerializeField]
    private TouchAndDragDoggo DoggoTouchDragScript;
    [SerializeField]
    private TailWag TailWaggerScript;

    // Use this for initialization
    void Start()
    {
        if (GM == null)
            GM = GameManager.Instance;
        LM = LoadingManager.Instance;

        StartingGameEvent = new IntroEvent();
        ActiveScriptContent = new List<string>();

        UI_Functions.PrepareDefaultGameView();

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.DoggoViewEventHandler, true);

#if UNITY_EDITOR
        //Report the time when the event manager finished
        Debug.Log(this.GetType().ToString() + " has finished loading at: <" + Time.unscaledTime + ">.");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if(TextRevealerScript.CurrentTextStatus == TextRevealLetterByLetterInGame.TextStatus.TextAllRevealed)
        {
            EndTextBoxEvent();
        }
    }

    //A gateway for performing certain actions when certain events are triggered.
    public override void OnSceneEvent(SceneManager.SceneEventType a_Event)
    {
        if (a_Event == SceneManager.SceneEventType.SceneHidden)
        {
            SceneIsCurrentlyActive = false;
        }
        if (a_Event == SceneManager.SceneEventType.SceneRevealed || a_Event == SceneManager.SceneEventType.SceneStarted)
        {
            SceneIsCurrentlyActive = true;

            if (!Returned)
                RequestEventStart(StartingGameEvent);
        }
    }

    /// <summary>
    /// Starts a text box event, assuming a valid script has been loaded. Delay defaults to 0, and will only be used if the bool is true.
    /// </summary>
    /// <param name="a_UseDelay"></param>
    /// <param name="a_Delay"></param>
    void BeginTextBoxEvent(GameEvent a_Event)
    {
        //Hide the game UI, and make it so doggo cannot be interacted with
        UI_Functions.HideGameUI();
        DoggoTouchDragScript.SetDraggable(false);

        if (a_Event.CheckIfEventUsesDelay())
        {
            //Start the text event after a short delay
            StartCoroutine(StartDelayedTextEvent(a_Event.GetDelayAmount()));
        }
        else
        {
            //Just start the event, no delay
            StartInstantTextEvent();
        }
    }

    void EndTextBoxEvent()
    {
        //Reset the text index and active script container
        TextIndex = 0;

        //Change to the regular game UI and make it so doggo can be interacted with
        string[] buttonsToReveal = new string[1];
        string[] buttonsToHide = new string[3];
        buttonsToReveal[0] = "Talk Button";
        buttonsToHide[0] = "Love Button";
        buttonsToHide[1] = "Food Button";
        buttonsToHide[2] = "Play Button";
        SwitchSpecificUIButtons(buttonsToReveal, buttonsToHide);

        UI_Functions.HideTextBox();
        TextRevealerScript.CleanUpTextEvent();
        DoggoTouchDragScript.SetDraggable(true);

        //Save the game
        GM.SaveAndLoader.SaveAllGameData();
    }

    void SwitchSpecificUIButtons(string[] a_ButtonsToReveal, string[] a_ButtonsToHide)
    {
        UI_Functions.RevealSpecificGameUI(a_ButtonsToReveal);
        UI_Functions.HideSpecificGameUI(a_ButtonsToHide);
    }

    void PerformRevealEvent(GameEvent a_Event)
    {
        //Check to see if we already have a save file
        bool SaveFileExists = GM.SaveAndLoader.CheckForSaveFile();

        if (SaveFileExists)
        {
            //Perform a return event
            ReturningIntroEvent();
        }
        else if (!SaveFileExists)
        {
            //Perform the first-time event
            BeginTextBoxEvent(a_Event);
        }

        Returned = true;
    }

    void ReturningIntroEvent()
    {
        TailWaggerScript.StartWaggingTail();
        UI_Functions.RevealDoggo();
        UI_Functions.RevealGameUI();
    }

    IEnumerator StartDelayedTextEvent(float a_Delay)
    {
        //Wait
        yield return new WaitForSeconds(a_Delay);

        //Show the game ui
        UI_Functions.PrepareTextEventGameView();

        //Begin showing the script
        TextRevealerScript.StartRevealingText(ActiveScriptContent);

        yield return null;
    }

    void StartInstantTextEvent()
    {
        //Show the game UI
        UI_Functions.PrepareTextEventGameView();

        //Begin showing the script
        TextRevealerScript.StartRevealingText(ActiveScriptContent);
    }

    public override void RequestEventStart(GameEvent a_Event)
    {
        if (a_Event.CheckIfUnlocked())
        {
            ActiveScriptContent = a_Event.GetTextEventScript();

            if (a_Event.GetEventName() == "Intro Event")
            {
                PerformRevealEvent(a_Event);
            }
            else
            {
                BeginTextBoxEvent(a_Event);
            }
        }
    }

    public void RequestSceneChange(Button a_Button)
    {
        switch(a_Button.name)
        {
            case "Talk Button":
                GM.RequestSceneChange(this.gameObject, SceneManager.SceneNames.GameView, SceneManager.SceneNames.TextMenu);
                break;
        }
    }

    #region PRIVATE VARIABLES
    private LoadingManager LM;
    private GameEvent StartingGameEvent;
    private List<string> ActiveScriptContent;
    private int TextIndex = 0;
    private bool Returned = false;
    #endregion
}
