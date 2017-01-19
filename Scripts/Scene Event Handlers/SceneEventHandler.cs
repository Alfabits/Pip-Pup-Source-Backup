using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneEventHandler : MonoBehaviour {

    protected GameManager GM;
    public bool IsActive
    {
        get { return isActive; }
        protected set { isActive = value; }
    }

    private bool isActive;

	// Use this for initialization
	void Start () {
        GM = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void OnSceneEvent(GameManager.SceneEventType a_Event);
}
