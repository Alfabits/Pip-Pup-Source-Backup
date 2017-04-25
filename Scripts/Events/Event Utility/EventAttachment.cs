using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAttachment : MonoBehaviour {

    #region Public
    public GameEvent AttachedEvent
    {
        get { return objectEvent; }
        private set { objectEvent = value; }
    }

    EventStartupStatsChecker StatsChecker;

    #endregion

    // Use this for initialization
    void Awake () {
        GM = GameManager.Instance;
        StatsChecker = new EventStartupStatsChecker();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAttachedEvent(GameEvent a_Event)
    {
        objectEvent = a_Event;
    }

    void SetObjectEvent(GameEvent a_Event)
    {
        objectEvent = a_Event;
    }

    public void StartObjectEvent_ViaSceneChange()
    {
        bool canStart = StatsChecker.StatsAreOkay(GM.SaveAndLoader.GetEnergy(), AttachedEvent.GetEnergyRequired(), GM.SaveAndLoader.GetHunger());

        if(canStart)
            GM.RequestSceneChange_WithEvent(gameObject, GM.GetCurrentScene(), SceneManager.SceneNames.GameView, objectEvent);
    }

    private GameManager GM;
    private GameEvent objectEvent;
}
