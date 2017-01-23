using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoViewUIFunctions : MonoBehaviour {

    public GameObject DoggoObject;
    public GameObject SpeechBubble;
    public GameObject[] UI;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void RevealGameUI()
    {
        foreach (GameObject element in UI)
        {
            element.SetActive(true);
        }
    }

    public void HideGameUI()
    {
        foreach (GameObject element in UI)
        {
            element.SetActive(false);
        }
    }

    public void RevealTextBox()
    {
        SpeechBubble.SetActive(true);
    }

    public void HideTextBox()
    {
        SpeechBubble.SetActive(false);
    }

    public void RevealDoggo()
    {
        DoggoObject.SetActive(true);
    }

    

}
