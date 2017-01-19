using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRevealLetterByLetter : MonoBehaviour {

    public Text TextToLoad;
    public Text TextToAssignTo;

    public float TextRevealRate = 0.05f;
    private bool Blinking = false;

    public LoadingBar LoadBarScript;

    public enum TextStatus
    {
        TextToReveal = 0,
        TextRevealing,
        TextRevealed
    };
    public TextStatus CurrentTextStatus;

    string StringToUse;

	// Use this for initialization
	void Start () {

        CheckTextAssignment();
        CurrentTextStatus = TextStatus.TextToReveal;

    }
	
	// Update is called once per frame
	void Update () {

        if (CurrentTextStatus == TextStatus.TextRevealed && Blinking == false)
        {
            //Do the blinky thing
            StartCoroutine(BlinkLetter());
            Blinking = true;
        }

    }

    void ResetTextAssignment()
    {

    }

    public void StartRevealingText()
    {
        StringToUse = TextToLoad.text;
        CurrentTextStatus = TextStatus.TextRevealing;
        StartCoroutine(AssignText(StringToUse));
    }

    void CheckTextAssignment()
    {
        //Check to make sure the text boxes to load from and assign to are not null
        if(TextToLoad == null)
        {
            TextToLoad = this.GetComponent<Text>();
        }

        if(TextToAssignTo == null)
        {
            TextToAssignTo = this.GetComponent<Text>();
        }
    }

    void CheckActive()
    {
        if(!TextToAssignTo.IsActive())
        {
            TextToAssignTo.gameObject.SetActive(true);
        }
    }

    IEnumerator AssignText(string a_StringToUse)
    {
        //Get the initial random letter, and init the BuildString varaible
        char RandomLetter;
        string BuildString = "";

        RandomLetter = GetRandomLetter();
        TextToAssignTo.text += RandomLetter;

        //Start the loading bar
        LoadBarScript.SetStepInterval(a_StringToUse.Length);
        LoadBarScript.SetLoadWaitRate(TextRevealRate);
        LoadBarScript.BeginLoading();

        //Start the text assignment
        for(int i = 0; i < a_StringToUse.Length; i++)
        {
            if (i < a_StringToUse.Length - 1)
            {
                RandomLetter = GetRandomLetter();
                BuildString += a_StringToUse[i];
                TextToAssignTo.text = BuildString + RandomLetter;
            }
            else if(i >= a_StringToUse.Length - 1)
            {
                BuildString += a_StringToUse[i];
                TextToAssignTo.text = BuildString;
            }
            

            yield return new WaitForSeconds(TextRevealRate);
        }

        yield return new WaitForSeconds(3.0f);
        CurrentTextStatus = TextStatus.TextRevealed;
        yield return null;
    }

    IEnumerator BlinkLetter()
    {
        char blinkChar = '_';
        string OriginalBuildString = "";
        bool on = false;

        OriginalBuildString = TextToAssignTo.text;

        while (CurrentTextStatus == TextStatus.TextRevealed)
        {
            switch (on)
            {
                case true:
                    TextToAssignTo.text = OriginalBuildString + " " + blinkChar;
                    on = false;
                    break;

                case false:
                    TextToAssignTo.text = OriginalBuildString;
                    on = true;
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }

        Blinking = false;
        yield return null;
    }

    char GetRandomLetter()
    {
        int RandomLetterChooser = Random.Range(0, 12);
        switch (RandomLetterChooser)
        {
            case 0:
                return 'B';

            case 1:
                return 'Z';

            case 2:
                return 'H';
            
            case 3:
                return 'K';

            case 4:
                return 'R';

            case 5:
                return 'D';

            case 6:
                return 'i';

            case 7:
                return 'j';

            case 8:
                return 'g';

            case 9:
                return 'v';

            case 10:
                return '-';

            case 11:
                return 'f';

            case 12:
                return 'C';
                
            case 13:
                return 'W';
                
        }

        return '_';
    }
}
