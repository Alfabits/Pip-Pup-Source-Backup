using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationSequenceEventHandler : SceneEventHandler {

    TextRevealLetterByLetter RevealScript;
    bool TextFinishedRevealing = false;
    

    void Start()
    {
        RevealScript = this.gameObject.GetComponent<TextRevealLetterByLetter>();
        if (GM == null)
            GM = GameManager.Instance;
        IsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
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
                    GM.RequestSceneChange(this.gameObject, GameManager.SceneNames.GameView);
                }
            }
        }
    }

    public override void OnSceneEvent(GameManager.SceneEventType a_Event)
    {
        if (a_Event == GameManager.SceneEventType.SceneHidden)
        {
            IsActive = false;
        }
        if (a_Event == GameManager.SceneEventType.SceneStarted)
        {
            IsActive = true;
            RevealScript.StartRevealingText();
        }
        
    }
}
