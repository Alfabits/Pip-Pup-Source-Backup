using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptProgressHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
