using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneEventHandler : MonoBehaviour {

    protected GameManager GM;
    public bool SceneIsCurrentlyActive
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

    /// <summary>
    /// A function that is called by the SceneManager whenever an scene's status is changed. Every scene event handler must have this.
    /// </summary>
    /// <param name="a_Event"></param>
    public abstract void OnSceneEvent(SceneManager.SceneEventType a_Event);

    /// <summary>
    /// A function that allows a scene to start a GameEvent based on the event passed.
    /// </summary>
    /// <param name="a_Event"></param>
    public abstract void RequestEventStart(GameEvent a_Event);
}
