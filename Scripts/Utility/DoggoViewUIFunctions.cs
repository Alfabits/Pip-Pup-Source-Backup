using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoViewUIFunctions : MonoBehaviour {

    LoadingManager LM;

    [SerializeField]
    private GameObject DoggoObject;
    [SerializeField]
    private GameObject SpeechBubble;
    private Dictionary<string, GameObject> UI;

    // Use this for initialization
    void Start () {

        LM = LoadingManager.Instance;
        UI = new Dictionary<string, GameObject>();
        UI.Add("Talk Button", GameObject.Find("Talk Button"));
        UI.Add("Food Button", GameObject.Find("Food Button"));
        UI.Add("Love Button", GameObject.Find("Love Button"));
        UI.Add("Play Button", GameObject.Find("Play Button"));

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.DoggoViewUIFunctions, true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

   public void RevealGameUI()
    {
        foreach (KeyValuePair<string, GameObject> element in UI)
        {
            element.Value.SetActive(true);
        }
    }

    public void HideGameUI()
    {
        foreach (KeyValuePair<string, GameObject> element in UI)
        {
            element.Value.SetActive(false);
        }
    }

    public void RevealSpecificGameUI(string[] a_UIKeys)
    {
        foreach (string key in a_UIKeys)
        {
            if(UI[key])
            {
                UI[key].SetActive(true);
            }
        }
    }

    public void HideSpecificGameUI(string[] a_UIKeys)
    {
        foreach (string key in a_UIKeys)
        {
            if (UI[key])
            {
                UI[key].SetActive(false);
            }
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

    public void PrepareDefaultGameView()
    {
        RevealDoggo();
        RevealGameUI();
        HideTextBox();
    }

    public void PrepareTextEventGameView()
    {
        RevealDoggo();
        RevealTextBox();
        HideGameUI();
    }

}
