﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoopDoggo : MonoBehaviour {

    LoadingManager LM;

    public FloatingTextGenerator TextGenerator;
    Transform BoopLocation;
    string TextToUse = "";

	// Use this for initialization
	void Start () {
        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.BoopDoggo, true);

#if UNITY_EDITOR
        //Report the time when the event manager finished
        //Debug.Log(this.GetType().ToString() + " has finished loading at: <" + Time.unscaledTime + ">.");
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void CreateBoop()
    {
        TextGenerator.GenerateFloatingText(TextToUse, FloatingTextGenerator.FloatingTextUse.BoopDoggo, BoopLocation);
    }

    public void PrepareBoop(Transform a_Location, string a_Text)
    {
        BoopLocation = a_Location;
        TextToUse = a_Text;
    }
}
