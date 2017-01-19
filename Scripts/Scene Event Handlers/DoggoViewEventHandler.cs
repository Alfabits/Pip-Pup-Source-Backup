using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoViewEventHandler : SceneEventHandler {

    //Public Variables
    public List<string> DoggoIntroScript;
    public TextRevealLetterByLetterInGame RevealScript;
    public TailWag TailWaggerScript;
    public GameObject SpeechBubble;
    public GameObject DoggoObject;
    public GameObject[] UI;

    public enum Scripts
    {
        None = -1,
        DoggoIntroScript
    };

    //Private Variables
    private Scripts ActiveScript;
    private int TextIndex = 0;

    // Use this for initialization
    void Start () {
        if (GM == null)
            GM = GameManager.Instance;

        SpeechBubble.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(RevealScript.CurrentTextStatus == TextRevealLetterByLetterInGame.TextStatus.TextRevealed)
            CheckIfIntroScriptProgress();
    }

    //A gateway for performing certain actions when certain events are triggered.
    public override void OnSceneEvent(GameManager.SceneEventType a_Event)
    {
        if(a_Event == GameManager.SceneEventType.SceneHidden)
        {
            IsActive = false;
        }
        if(a_Event == GameManager.SceneEventType.SceneRevealed || a_Event == GameManager.SceneEventType.SceneStarted)
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
                ActiveList = DoggoIntroScript;
                ActiveListCount = ActiveList.Count;
                break;
        }

        if(IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
            RevealScript.CurrentTextStatus == TextRevealLetterByLetterInGame.TextStatus.TextRevealed &&
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
                    RevealScript.StartRevealingText(ActiveList[TextIndex]);
                    RevealScript.CurrentTextStatus = TextRevealLetterByLetterInGame.TextStatus.TextRevealing;
                }
            }
        }
        
    }

    void BeginTextBoxEvent(Scripts a_Script)
    {
        SpeechBubble.SetActive(true);
        ActiveScript = a_Script;
    }

    void EndTextBoxEvent()
    {
        TextIndex = 0;
        ActiveScript = Scripts.None;
        SpeechBubble.SetActive(false);
        RevealGameUI();
        Debug.Log("ended");
    }

    void RevealGameUI()
    {
        foreach (GameObject element in UI)
        {
            element.SetActive(true);
        }
    }

    void HideGameUI()
    {
        foreach(GameObject element in UI)
        {
            element.SetActive(false);
        }
    }

    void PerformRevealEvent()
    {
        //Check if we already have a save file.
        if(GM.SaveFile != "")
        {
            //Perform a return event
        }
        else if(GM.SaveFile == "")
        {
            //Perform the first-time event
            StartCoroutine(DelayedIntroEvent());
        }
    }

    void RevealDoggo()
    {
        TailWaggerScript.StartWaggingTail();
        DoggoObject.SetActive(true);
    }

    IEnumerator DelayedIntroEvent()
    {
        //Reveal the dog, but hide the UI
        RevealDoggo();
        HideGameUI();

        //Wait
        yield return new WaitForSeconds(3.0f);

        //Begin the intro event
        BeginTextBoxEvent(Scripts.DoggoIntroScript);
        RevealScript.StartRevealingText(DoggoIntroScript[TextIndex]);
        yield return null;
    }
}
