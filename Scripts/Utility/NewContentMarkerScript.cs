using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewContentMarkerScript : MonoBehaviour {

    [SerializeField]
    GameObject ExclamationMarkPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnOnExclamationMark()
    {
        ExclamationMarkPrefab.SetActive(true);
    }

    public void TurnOffExclamationMark()
    {
        ExclamationMarkPrefab.SetActive(false);
    }
}
