using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchAndDragDoggo : MonoBehaviour
{
    LoadingManager LM;

    public Camera MainCamera;
    public FloatingTextGenerator TextGenerator;

    Ray ray;
    RaycastHit hit;
    GameObject hitObject;

    bool DoggoWantsToBeDragged = false;
    bool DoggoCanBeDragged = false;
    float HitLength;

    LayerMask DoggoDragMask;

    // Use this for initialization
    void Start()
    {
        DoggoDragMask = 1 << 8;

        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.TouchAndDragDoggo, true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDoggoTouch();
    }

    void CheckForDoggoTouch()
    {

#if UNITY_EDITOR
        if (DoggoWantsToBeDragged)
        {
            if (Input.GetMouseButton(0))
            {
                //Update the raycast position
                ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                //Check if the text generator can spawn a new piece of text
                if (TextGenerator.TimerComplete)
                {
                    TextGenerator.GenerateFloatingText("", FloatingTextGenerator.FloatingTextUse.TouchAndDragDoggo, hitObject.transform);
                    TextGenerator.ResetTimer();
                }

                //Calculate the new position/velocity of the floating doggo
                Vector3 velocity = (ray.GetPoint(HitLength) - hitObject.transform.position) * 4.0f;
                if (velocity.magnitude > 10.0f)
                {
                    velocity *= 10.0f / velocity.magnitude;
                }
                hitObject.GetComponent<Rigidbody>().velocity = velocity;
            }
        }
        else
        {
            ResetDraggingParameters();
        }
#endif
    }

    void ResetDraggingParameters()
    {
        hitObject = null;
        TextGenerator.ResetTimer();
    }

    public void StartLiftingDoggo(RaycastHit a_Hit, bool a_UseTimer)
    {
        hitObject = a_Hit.collider.gameObject;
        HitLength = a_Hit.distance;

        if (a_UseTimer)
        {
            TextGenerator.UseTimer();
        }

        DoggoWantsToBeDragged = true;
    }

    public void StopLiftingDoggo()
    {
        hitObject = null;
        DoggoWantsToBeDragged = false;
    }

    public LayerMask GetDoggoDragMask()
    {
        return DoggoDragMask;
    }

    public void SetDraggable(bool a_Value)
    {
        DoggoCanBeDragged = a_Value;
    }

    public bool GetDraggable()
    {
        return DoggoCanBeDragged;
    }

    //Vector3 CurrentTouchPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
    //Vector3 NewHOPosition = hitObject.transform.position;
    //NewHOPosition = HOStartPosition + (CurrentTouchPosition - TouchedPosition);
    //hitObject.transform.position = new Vector3(NewHOPosition.x, NewHOPosition.y, HOStartPosition.z);
}
