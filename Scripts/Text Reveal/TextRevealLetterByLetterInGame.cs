using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRevealLetterByLetterInGame : MonoBehaviour
{

    LoadingManager LM;

    public string TextToLoad;
    public Text TextToAssignTo;

    public float TextRevealRate = 0.05f;

    public enum TextStatus
    {
        TextToReveal = 0,
        TextRevealing,
        TextRevealed,
        TextAllRevealed
    };

    public TextStatus CurrentTextStatus;
    private List<string> ActiveScriptContent;
    private bool Blinking = false;
    private bool SkipText = false;
    private bool CanSkip = false;
    private int TextIndex = 0;
    private string StringToUse;

    // Use this for initialization
    void Start()
    {

        CheckTextAssignment();
        CurrentTextStatus = TextStatus.TextToReveal;

        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.TextRevealLettterByLetterInGame, true);

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTextStatus == TextStatus.TextRevealed && Blinking == false)
        {
            //Do the blinky thing
            StartCoroutine(BlinkLetter());
            Blinking = true;
        }

        if (CurrentTextStatus == TextStatus.TextRevealed)
        {
            CheckForProgressToNextScriptContent();
        }

        //Check if we can skip yet. We make it so the player can't just hold down the button after pressing 
        //space to progress to the next piece of content, as that feels sludgy and inprecise.
        if (CanSkip)
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
            CurrentTextStatus == TextStatus.TextRevealing &&
            SkipText == false)
            {
                //set skip text to true
                SkipText = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space) &&
            CurrentTextStatus == TextStatus.TextRevealing &&
            SkipText == true)
            {
                //set skip text to false
                SkipText = false;
            }
            else if (CurrentTextStatus == TextStatus.TextRevealed)
            {
                SkipText = false;
                CanSkip = false;
            }
        }
        else
        {
            if (CurrentTextStatus == TextStatus.TextRevealing &&
            Input.GetKeyUp(KeyCode.Space))
            {
                CanSkip = true;
            }
        }

    }

    void CheckForProgressToNextScriptContent()
    {
        //Check the script that should be used
        List<string> ActiveList = new List<string>();
        int ActiveListCount = 0;

        ActiveList = ActiveScriptContent;
        ActiveListCount = ActiveList.Count;

        if (Input.GetKeyDown(KeyCode.Space) &&
        CurrentTextStatus == TextStatus.TextRevealed &&
        TextIndex >= 0 &&
        TextIndex < ActiveListCount)
        {
            TextIndex += 1;

            if (TextIndex > ActiveListCount - 1)
            {
                CurrentTextStatus = TextStatus.TextAllRevealed;
            }
            else
            {
                RevealNextLine(ActiveList[TextIndex]);
                CurrentTextStatus = TextStatus.TextRevealing;
            }
        }
    }

    public void StartRevealingText(List<string> a_ScriptContent)
    {
        if (CurrentTextStatus != TextStatus.TextRevealing)
        {
            ActiveScriptContent = a_ScriptContent;
            RevealNextLine(a_ScriptContent[0]);
        }
    }

    public void CleanUpTextEvent()
    {
        if (CurrentTextStatus == TextStatus.TextAllRevealed)
        {
            ActiveScriptContent = null;
            TextIndex = 0;
            Blinking = false;
        }
    }

    void RevealNextLine(string a_UseThis)
    {
        if (CurrentTextStatus != TextStatus.TextRevealing)
        {
            CurrentTextStatus = TextStatus.TextRevealing;
            StartCoroutine(AssignText(a_UseThis));
        }
    }

    void CheckTextAssignment()
    {
        //Check to make sure the text boxes to assign to are not null
        if (TextToAssignTo == null)
        {
            TextToAssignTo = this.GetComponent<Text>();
        }
    }

    void CheckActive()
    {
        if (!TextToAssignTo.IsActive())
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

        StringToUse = a_StringToUse;

        for (int i = 0; i < StringToUse.Length; i++)
        {
            if (i < StringToUse.Length - 1)
            {
                RandomLetter = GetRandomLetter();
                BuildString += StringToUse[i];
                TextToAssignTo.text = BuildString + RandomLetter;
            }
            else if (i >= StringToUse.Length - 1)
            {
                BuildString += StringToUse[i];
                TextToAssignTo.text = BuildString;
            }

            if (!SkipText)
                yield return new WaitForSeconds(TextRevealRate);
            else
                yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        CurrentTextStatus = TextStatus.TextRevealed;
        yield return null;
    }

    IEnumerator AssignTextList(List<string> a_StringsToUse)
    {

        //Start the text assignment
        for (int j = 0; j < a_StringsToUse.Count; j++)
        {
            //Get the initial random letter, and init the BuildString varaible
            char RandomLetter;
            string BuildString = "";

            RandomLetter = GetRandomLetter();
            TextToAssignTo.text += RandomLetter;

            StringToUse = a_StringsToUse[j];

            for (int i = 0; i < StringToUse.Length; i++)
            {
                if (i < StringToUse.Length - 1)
                {
                    RandomLetter = GetRandomLetter();
                    BuildString += StringToUse[i];
                    TextToAssignTo.text = BuildString + RandomLetter;
                }
                else if (i >= StringToUse.Length - 1)
                {
                    BuildString += StringToUse[i];
                    TextToAssignTo.text = BuildString;
                }


                yield return new WaitForSeconds(TextRevealRate);
            }

            yield return new WaitForSeconds(1.0f);
            CurrentTextStatus = TextStatus.TextRevealed;
        }

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
