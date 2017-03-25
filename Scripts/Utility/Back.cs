using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour {

    GameManager GM;

	// Use this for initialization
	void Start () {
        GM = GameManager.Instance;
	}

    public void GoToPreviousScene()
    {
        GM.RequestSceneChange(gameObject, GM.GetCurrentScene(), GM.GetPreviousScene());
    }

    public void GoToHomeScene()
    {
        GM.RequestSceneChange(gameObject, GM.GetCurrentScene(), GM.GetHomeScene());
    }
}
