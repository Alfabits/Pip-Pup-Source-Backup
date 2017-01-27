using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    LoadingManager LM;

	// Use this for initialization
	void Start () {
        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.SceneManager, true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
