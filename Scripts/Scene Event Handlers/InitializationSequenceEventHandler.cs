using System;
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
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GM.SetEventIsPlaying(GetType(), false);
                    GM.RequestSceneChange(this.gameObject, SceneManager.SceneNames.Intro, SceneManager.SceneNames.GameView);
                }
#endif
#if UNITY_ANDROID
                if (Input.touchCount > 0)
                {
                    GM.SetEventIsPlaying(GetType(), false);
                    GM.RequestSceneChange(this.gameObject, SceneManager.SceneNames.Intro, SceneManager.SceneNames.GameView);
                }
#endif
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
            GM.SetEventIsPlaying(GetType(), true);
            RevealScript.StartRevealingText();
        }
        

    }

    public override void RequestEventStart(GameEvent a_Event)
    {
        
    }
}
