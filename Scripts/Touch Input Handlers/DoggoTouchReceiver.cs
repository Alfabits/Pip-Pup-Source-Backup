using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoTouchReceiver : MonoBehaviour {

    bool IsBeingTouched = false;

	void OnTouchDown()
    {
        IsBeingTouched = true;
        Debug.Log(IsBeingTouched);
    }

    void OnTouchUp()
    {
        IsBeingTouched = false;
        Debug.Log(IsBeingTouched);
    }

    void OnTouchStay()
    {
        IsBeingTouched = true;
        Debug.Log(IsBeingTouched);
    }

    void OnTouchExit()
    {
        IsBeingTouched = false;
        Debug.Log(IsBeingTouched);
    }
}
