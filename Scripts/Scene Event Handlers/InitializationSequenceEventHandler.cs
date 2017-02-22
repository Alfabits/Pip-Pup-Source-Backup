﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationSequenceEventHandler : SceneEventHandler {

    LoadingManager LM;

    TextRevealLetterByLetter RevealScript;
    bool TextFinishedRevealing = false;

    void Start()
    {
        RevealScript = this.gameObject.GetComponent<TextRevealLetterByLetter>();
        if (GM == null)
            GM = GameManager.Instance;
        SceneIsCurrentlyActive = true;

        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.InitializationSequenceEventHandler, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneIsCurrentlyActive)
        {
            //if (RevealScript.CurrentTextStatus == TextRevealLetterByLetter.TextStatus.TextToReveal)
            //{
            //    GM.RequestSceneStart(this.gameObject);
            //}
            if (RevealScript.CurrentTextStatus == TextRevealLetterByLetter.TextStatus.TextRevealed && TextFinishedRevealing == false)
            {
                TextFinishedRevealing = true;
            }

            if (TextFinishedRevealing)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GM.RequestSceneChange(this.gameObject, SceneManager.SceneNames.Intro, SceneManager.SceneNames.GameView);
                }
            }
        }
    }

    public override void OnSceneEvent(SceneManager.SceneEventType a_Event)
    {
        if (a_Event == SceneManager.SceneEventType.SceneHidden)
        {
            SceneIsCurrentlyActive = false;
        }
        if (a_Event == SceneManager.SceneEventType.SceneStarted)
        {
            SceneIsCurrentlyActive = true;
            RevealScript.StartRevealingText();
        }
        

    }

    public override void RequestEventStart(GameEvent a_Event)
    {
        
    }
}
