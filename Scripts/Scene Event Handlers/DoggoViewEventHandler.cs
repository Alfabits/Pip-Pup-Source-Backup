using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoViewEventHandler : SceneEventHandler
{
    //Public Variables
    public TextRevealLetterByLetterInGame TextRevealerScript;
    public DoggoViewUIFunctions UI_Functions;
    public TailWag TailWaggerScript;

    public enum Scripts
    {
        None = -1,
        DoggoIntroScript
    };

    //Private Variables
    private LoadingManager LM;
    private Dictionary<string, GameEvent> EventList;
    private List<string> ActiveScriptContent;
    private Scripts ActiveScript;
    private GameEvent ActiveEvent;
    private int TextIndex = 0;

    // Use this for initialization
    void Start()
    {
        if (GM == null)
            GM = GameManager.Instance;
        LM = LoadingManager.Instance;

        PrepareDefaultGameView();

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.DoggoViewEventHandler, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (TextRevealerScript.CurrentTextStatus == TextRevealLetterByLetterInGame.TextStatus.TextRevealed)
            CheckIfIntroScriptProgress();
    }

    //A gateway for performing certain actions when certain events are triggered.
    public override void OnSceneEvent(SceneManager.SceneEventType a_Event)
    {
        if (a_Event == SceneManager.SceneEventType.SceneHidden)
        {
            IsActive = false;
        }
        if (a_Event == SceneManager.SceneEventType.SceneRevealed || a_Event == SceneManager.SceneEventType.SceneStarted)
        {
            IsActive = true;
            PerformRevealEvent();
        }
    }

    void CheckIfIntroScriptProgress()
    {
        //Check the script that should be used
        List<string> ActiveList = new List<string>();
        int ActiveListCount = 0;

        switch (ActiveScript)
        {
            case Scripts.DoggoIntroScript:
                ActiveList = ActiveScriptContent;
                ActiveListCount = ActiveList.Count;
                break;
        }

        if (IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
            TextRevealerScript.CurrentTextStatus == TextRevealLetterByLetterInGame.TextStatus.TextRevealed &&
            TextIndex >= 0 &&
            TextIndex < ActiveListCount)
            {
                TextIndex += 1;

                if (TextIndex > ActiveListCount - 1)
                {
                    Debug.Log("ending");
                    EndTextBoxEvent();
                }
                else
                {
                    TextRevealerScript.StartRevealingText(ActiveList[TextIndex]);
                    TextRevealerScript.CurrentTextStatus = TextRevealLetterByLetterInGame.TextStatus.TextRevealing;
                }
            }
        }

    }

    void BeginTextBoxEvent(Scripts a_Script)
    {
        UI_Functions.RevealTextBox();
        UI_Functions.HideGameUI();
        ActiveScript = a_Script;
    }

    void EndTextBoxEvent()
    {
        //Reset the text index and active script container
        TextIndex = 0;
        ActiveScript = Scripts.None;

        //Change to the regular game UI
        UI_Functions.HideTextBox();
        UI_Functions.RevealGameUI();

        //Save the game
        GM.SaveAndLoader.SaveAllGameData();
    }

    void PerformRevealEvent()
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
            StartCoroutine(DelayedIntroEvent());
        }
    }

    void ReturningIntroEvent()
    {
        TailWaggerScript.StartWaggingTail();
        UI_Functions.RevealDoggo();
        UI_Functions.RevealGameUI();
    }

    IEnumerator DelayedIntroEvent()
    {
        //Reveal the dog, but hide the UI
        UI_Functions.RevealDoggo();
        UI_Functions.HideGameUI();

        //Wait
        yield return new WaitForSeconds(3.0f);

        //Begin the intro event
        BeginTextBoxEvent(Scripts.DoggoIntroScript);
        TextRevealerScript.StartRevealingText(ActiveScriptContent[TextIndex]);
        yield return null;
    }

    void PrepareDefaultGameView()
    {
        EventList = new Dictionary<string, GameEvent>();
        EventList.Add("Intro", new IntroEvent());

        UI_Functions.HideTextBox();
    }

    public override void RequestEventStart(GameEvent a_Event)
    {
        ActiveScriptContent = a_Event.GetTextEventScript();
    }
}
