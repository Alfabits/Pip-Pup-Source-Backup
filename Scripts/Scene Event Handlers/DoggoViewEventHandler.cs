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
    [SerializeField]
    private LocalStatsKeeper StatsKeeper;
    [SerializeField]
    private FloatingTextGenerator TextGenerator;
    private ChooseRandomLoveEvent LoveEventChooser;
    [SerializeField]
    private GameObject FoodObject;
    private FeedAnimation CreatedFeederScript;
    private bool wasEating;
    private StatsChangeAnimation StatsAnimator;


    // Use this for initialization
    void Start()
    {
        GM = GameManager.Instance;
        LM = LoadingManager.Instance;
        StartingGameEvent = new IntroEvent();
        ActiveScriptContent = new List<string>();
        LoveEventChooser = new ChooseRandomLoveEvent();

        UI_Functions.PrepareUI();
        UI_Functions.PrepareDefaultGameView();

        //Check in with the loading manager
        LM.CheckIn(gameObject, LoadingManager.KeysForScriptsToBeLoaded.DoggoViewEventHandler, true);

#if UNITY_EDITOR
        //Report the time when the event manager finished
        //Debug.Log(this.GetType().ToString() + " has finished loading at: <" + Time.unscaledTime + ">.");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if(TextRevealerScript.CurrentTextStatus == TextRevealLetterByLetterInGame.TextStatus.TextAllRevealed
            && ActiveGameEvent != null)
        {
            EndTextBoxEvent();
        }

        UpdateFeeding();
    }

    //A gateway for performing certain actions when certain events are triggered.
    public override void OnSceneEvent(SceneManager.SceneEventType a_Event)
    {
        if (a_Event == SceneManager.SceneEventType.SceneHidden)
        {
            SceneIsCurrentlyActive = false;
            TailWaggerScript.StopWaggingTail();
            
        }
        if (a_Event == SceneManager.SceneEventType.SceneRevealed || a_Event == SceneManager.SceneEventType.SceneStarted)
        {
            TailWaggerScript.StartWaggingTail();
            EndTextBoxEvent();
            SceneIsCurrentlyActive = true;

            //if (!Returned)
            //    RequestEventStart(StartingGameEvent);
        }
    }

    /// <summary>
    /// Starts a text box event, given a valid GameEvent.
    /// </summary>
    /// <param name="a_Event"></param>
    public void BeginTextBoxEvent(GameEvent a_Event)
    {
        //Hide the game UI, and make it so doggo cannot be interacted with
        UI_Functions.HideGameUI();
        DoggoTouchDragScript.SetDraggable(false);
        ActiveGameEvent = a_Event;

        if (a_Event.CheckIfEventUsesDelay())
        {
            //Start the text event after a short delay
            GM.SetEventIsPlaying(true);
            StartCoroutine(StartDelayedTextEvent(a_Event.GetDelayAmount()));
        }
        else
        {
            //Just start the event, no delay
            GM.SetEventIsPlaying(true);
            StartInstantTextEvent();
        }
    }

    void EndTextBoxEvent()
    {
        //Reset the text index and active script container
        TextIndex = 0;

        //Revert to regular game UI
        UI_Functions.RevealGameUI();
        UI_Functions.HideTextBox();
        TextRevealerScript.CleanUpTextEvent();
        DoggoTouchDragScript.SetDraggable(true);

        //Let the game manager know the event is over
        GM.SetEventIsPlaying(false);

        //Complete the event, then save the game
        if(ActiveGameEvent != null)
        {
            StatsKeeper.UpdateStats(ActiveGameEvent);
            ActiveGameEvent.CompleteEvent();
            GM.SaveEvent(ActiveGameEvent);
            ActiveGameEvent = null;
        }
    }

    public void ChooseAndPlayRandomLoveEvent()
    {
        if(StatsKeeper.Energy >= 10)
        {
            LoveEventChooser.ChooseRandomEvent();
            RequestEventStart(LoveEventChooser.GetChosenGameEvent());
        }
        else
        {
            TextGenerator.GenerateFloatingText("too tired", FloatingTextGenerator.FloatingTextUse.BoopDoggo, DoggoTouchDragScript.gameObject.transform);
        }
    }

    public void FeedDoggo()
    {
        //TODO: check if the dog has been fed in the past few hours
        if(StatsKeeper.Hunger <= 50)
        {
            UI_Functions.HideGameUI();
            CreatedFeederScript = Instantiate(FoodObject, Vector3.zero, Quaternion.identity).GetComponent<FeedAnimation>();
            CreatedFeederScript.BeginAnimation();
        }
        else if(StatsKeeper.Hunger > 50)
        {
            TextGenerator.GenerateFloatingText("I'm still full", FloatingTextGenerator.FloatingTextUse.BoopDoggo, DoggoTouchDragScript.gameObject.transform);
        }
    }

    void UpdateFeeding()
    {
        if (CreatedFeederScript != null)
        {
            if (wasEating && CreatedFeederScript.isEating == false)
            {
                Destroy(CreatedFeederScript.gameObject);
                CreatedFeederScript = null;

                //Update the local stats keeper
                StatsKeeper.DoggoHasBeenFed();

                //Create text for pip-pup to say
                TextGenerator.GenerateFloatingText("burp", FloatingTextGenerator.FloatingTextUse.BoopDoggo, DoggoTouchDragScript.gameObject.transform);

                //Start the stats update animation

                //Reveal the UI
                UI_Functions.RevealGameUI();
            }

            //We check if CreatedFeederScript is null here, since the above code block can destroy it
            if (CreatedFeederScript != null)
            {
                wasEating = CreatedFeederScript.isEating;
            }
            else
                wasEating = false;
        }

    }

    void SwitchSpecificUIButtons(string[] a_ButtonsToReveal, string[] a_ButtonsToHide)
    {
        UI_Functions.RevealSpecificGameUI(a_ButtonsToReveal);
        UI_Functions.HideSpecificGameUI(a_ButtonsToHide);
    }

    //TODO: Allocate the logic on whether or not to perform an initial reveal event or a returning event to a different script
    void PerformRevealEvent(GameEvent a_Event)
    {
        //Check to see if we already have a save file
        bool firstTime = GM.SaveAndLoader.IsFirstTimePlaying();

        //TODO: can uncomment when there are returning events to choose from (polish)
        //if (!firstTime)
        //{
        //    //Perform a return event
        //    ReturningIntroEvent();
        //}
        //else if (firstTime)
        //{
        //    //Perform the first-time event
        //    BeginTextBoxEvent(a_Event);
        //}

        BeginTextBoxEvent(a_Event);
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
    private GameEvent ActiveGameEvent;
    private List<string> ActiveScriptContent;
    private int TextIndex = 0;
    private bool Returned = false;
    #endregion
}
