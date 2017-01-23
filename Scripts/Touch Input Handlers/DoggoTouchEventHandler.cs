using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (TouchAndDragDoggo))]
public class DoggoTouchEventHandler : MonoBehaviour {

    Camera MainCamera;
    GameObject HitObject;
    TouchAndDragDoggo TouchScript;

    bool Counting = false;
    bool LiftingDoggo = false;
    float CurrentCountedTime = MinimumCountedTime;
    const float MaximumCountedTime = 5.0f;
    const float MinimumCountedTime = 0.0f;
    const float TimeCountInterval = 0.2f;

    RaycastHit hit;


    // Use this for initialization
    void Start () {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        TouchScript = this.gameObject.GetComponent<TouchAndDragDoggo>();
        HitObject = null;
	}
	
	// Update is called once per frame
	void Update () {
        CheckForTouchUpdates();
	}

    void CheckForTouchUpdates()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0) && !Counting)
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, TouchScript.GetDoggoDragMask()))
            {
                //Create a copy of the object we hit
                HitObject = hit.collider.gameObject;

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
                StartLiftingDoggo(HitObject);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(LiftingDoggo)
            {
                //Stop the doggo from being lifted
                StopLiftingDoggo();

                //Reset the counter
                ResetCounter();
            }
            if(!LiftingDoggo)
            {
                //Boop the doggo, since we didn't finish the countdown.
            }
        }
#endif
    }

    void StartLiftingDoggo(GameObject a_HitObject)
    {
        //Send a message to the touch script, telling it to start lifting the doggo
        TouchScript.StartLiftingDoggo(a_HitObject);

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
        HitObject = null;
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
