using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenTapToProgress : MonoBehaviour {

    GameManager GM;

	// Use this for initialization
	void Start () {
        GM = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
                GM.RequestSceneChange(this.gameObject, SceneManager.SceneNames.TitleScreen);
        }

        if(Input.GetMouseButtonDown(0))
        {
            GM.RequestSceneChange(this.gameObject, SceneManager.SceneNames.TitleScreen);
        }
    }
}
