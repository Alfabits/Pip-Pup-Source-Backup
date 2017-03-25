using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextGenerator : MonoBehaviour {

    LoadingManager LM;

    [SerializeField]
    GameObject FloatingTextPrefab;
    [SerializeField]
    Transform FloatingTextParent;

    string TextToDisplay = "";

    bool Timing = false;

    const float BaseTimerTime = 0.0f;
    float CurrentTimerTime = 0.0f;
    float MaximumTimerTime = 10.0f;
    float TimerInterval = 0.2f;
    public bool TimerComplete = false;

    public enum FloatingTextUse
    {
        TouchAndDragDoggo = 0,
        BoopDoggo
    };

    // Use this for initialization
    void Start () {
        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.FloatingTextGenerator, true);
    }
	
	// Update is called once per frame
	void Update () {
		if(Timing && !TimerComplete)
        {
            UpdateTimer();
        }
	}

    public void GenerateFloatingText(string a_TextToUse, FloatingTextUse a_Use, Transform a_Location)
    {
        //If text to use is blank, generate random text
        if(a_TextToUse == "")
        {
            ChooseRandomTextToDisplay(a_Use);
        }
        else
        {
            TextToDisplay = a_TextToUse;
        }

        //Determine a starting position for the floating text
        float xoffset = 2;
        float yoffset = 2;
        Vector3 TextOffset = new Vector3(a_Location.position.x + xoffset, a_Location.position.y + yoffset, a_Location.position.z);

        //Spawn the floating text prefab
        GameObject tempFloatingTextReference = GameObject.Instantiate(FloatingTextPrefab, TextOffset, Quaternion.identity, FloatingTextParent);
        FloatingText tempFloatingTextScript = tempFloatingTextReference.GetComponent<FloatingText>();

        //Set the text to display
        tempFloatingTextScript.SetTextToDisplay(TextToDisplay);
        //Start floating the floating text
        tempFloatingTextScript.StartFloatingText();

    }

    void ChooseRandomTextToDisplay(FloatingTextUse a_Use)
    {
        int randomNumber;

        //Touch and Drag Doggo Text Library
        if (a_Use == FloatingTextUse.TouchAndDragDoggo)
        {
            randomNumber = Random.Range(0, 20);

            switch (randomNumber)
            {
                case 0:
                    TextToDisplay = "no";
                    break;

                case 1:
                    TextToDisplay = "please";
                    break;

                case 2:
                    TextToDisplay = "down";
                    break;

                case 3:
                    TextToDisplay = "bork";
                    break;

                case 4:
                    TextToDisplay = "bark";
                    break;

                case 5:
                    TextToDisplay = "enough";
                    break;

                case 6:
                    TextToDisplay = "stop";
                    break;

                case 7:
                    TextToDisplay = "cease and desist";
                    break;

                case 8:
                    TextToDisplay = "i have rights";
                    break;

                case 9:
                    TextToDisplay = "whimper";
                    break;

                case 10:
                    TextToDisplay = "i want down";
                    break;

                case 11:
                    TextToDisplay = "no more";
                    break;

                case 12:
                    TextToDisplay = "cease this";
                    break;

                case 13:
                    TextToDisplay = "mommy make it stop";
                    break;

                case 14:
                    TextToDisplay = "sniff";
                    break;

                case 15:
                    TextToDisplay = "lick";
                    break;

                case 16:
                    TextToDisplay = "down down down";
                    break;

                case 17:
                    TextToDisplay = "why";
                    break;

                case 18:
                    TextToDisplay = "hands off";
                    break;

                case 19:
                    TextToDisplay = "not a toy";
                    break;

                case 20:
                    TextToDisplay = "smol dog \n want down";
                    break;

                case 21:
                    TextToDisplay = "master plz";
                    break;

                case 22:
                    TextToDisplay = "01101110 \n 01101111";
                    break;
            }
        }

        //Boop Doggo Text Library
        if (a_Use == FloatingTextUse.BoopDoggo)
        {
            randomNumber = Random.Range(0, 6);

            switch (randomNumber)
            {
                case 0:
                    TextToDisplay = "+1 Boop";
                    break;

                case 1:
                    TextToDisplay = "boop";
                    break;

                case 2:
                    TextToDisplay = "boopalicious";
                    break;

                case 3:
                    TextToDisplay = "beep boop";
                    break;

                case 4:
                    TextToDisplay = "stop";
                    break;

                case 5:
                    TextToDisplay = "<3";
                    break;

                case 6:
                    TextToDisplay = "bibbity bobbity boop";
                    break;

                case 7:
                    TextToDisplay = "do you even boop";
                    break;

                case 8:
                    TextToDisplay = "bop";
                    break;

                case 9:
                    TextToDisplay = "bahp";
                    break;

                case 10:
                    TextToDisplay = "pip";
                    break;
            }
        }
    }
    public void UseTimer()
    {
        CurrentTimerTime = BaseTimerTime;
        Timing = true;
    }
    public void ResetTimer()
    {
        CurrentTimerTime = BaseTimerTime;
        TimerComplete = false;
        Timing = true;
    }
    void UpdateTimer()
    {
        if(CurrentTimerTime < MaximumTimerTime)
        {
            CurrentTimerTime += TimerInterval;
        }
        else if(CurrentTimerTime >= MaximumTimerTime)
        {
            TimerComplete = true;
        }
    }

    
}
