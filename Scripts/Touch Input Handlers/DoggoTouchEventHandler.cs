using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (TouchAndDragDoggo))]
public class DoggoTouchEventHandler : MonoBehaviour {

    LoadingManager LM;

    Camera MainCamera;
    TouchAndDragDoggo TouchScript;
    BoopDoggo BoopScript;
    DoggoViewUIFunctions UI_Functions;

    bool Counting = false;
    bool LiftingDoggo = false;
    float CurrentCountedTime = MinimumCountedTime;
    const float MaximumCountedTime = 8.0f;
    const float MinimumCountedTime = 0.0f;
    const float TimeCountInterval = 0.2f;

    RaycastHit hit;


    // Use this for initialization
    void Start () {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        TouchScript = this.gameObject.GetComponent<TouchAndDragDoggo>();
        BoopScript = this.gameObject.GetComponent<BoopDoggo>();
        UI_Functions = GameObject.Find("Doggo Menu").GetComponent<DoggoViewUIFunctions>();

        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.DoggoTouchEventHandler, true);
    }
	
	// Update is called once per frame
	void Update () {

        //Base check to see if the doggo is being touched
        CheckForTouchUpdates();
	}

    void CheckForTouchUpdates()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0) && !Counting)
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, TouchScript.GetDoggoDragMask()) && hit.rigidbody)
            {
                //Start counting
                Counting = true;
            }
                
        }
        if(Input.GetMouseButton(0) && Counting)
        {
            //Check the current counted time to the target time.
            if(CurrentCountedTime < MaximumCountedTime)
            {
                //Otherwise, increase the time by a fixed amount.
                IncrementCounter();
            }
            else if(CurrentCountedTime >= MaximumCountedTime)
            {
                //If we've reached the target time, stop counting.
                //Then send a message to TouchAndDragDoggo that the player wants to drag the dog around
                StartLiftingDoggo();
                UI_Functions.HideGameUI();
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(LiftingDoggo)
            {
                //Stop the doggo from being lifted
                StopLiftingDoggo();
                UI_Functions.RevealGameUI();

                //Reset the counter
                ResetCounter();
            }
            if(!LiftingDoggo)
            {
                //Boop the doggo, since we didn't finish the countdown.
                BoopTheDoggo();

                //Reset the counter
                ResetCounter();
            }
        }
#endif
    }

    void StartLiftingDoggo()
    {
        //Send a message to the touch script, telling it to start lifting the doggo
        TouchScript.StartLiftingDoggo(hit, true);

        //Stop counting and start lifting
        Counting = false;
        LiftingDoggo = true;
    }

    void StopLiftingDoggo()
    {
        //Send a message to the touch script, saying that the doggo is no longer being lifted
        TouchScript.StopLiftingDoggo();

        //Set the boolean to false
        LiftingDoggo = false;
    }

    void BoopTheDoggo()
    {
        //Prepare the boop
        BoopScript.PrepareBoop(hit.transform, "");

        //Execute the boop
        BoopScript.CreateBoop();
    }

    void IncrementCounter()
    {
        CurrentCountedTime += TimeCountInterval;
    }

    void DecrementTimer()
    {
        CurrentCountedTime -= TimeCountInterval;
    }

    void FillTimer()
    {
        CurrentCountedTime = MaximumCountedTime;
    }

    void ResetCounter()
    {
        CurrentCountedTime = 0.0f;
    }
}
