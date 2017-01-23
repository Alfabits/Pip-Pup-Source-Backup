using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchAndDragDoggo : MonoBehaviour
{

    public Camera MainCamera;
    List<GameObject> touchList = new List<GameObject>();
    GameObject[] touchesOld;

    RaycastHit hit;
    GameObject hitObject;
    Vector3 HOStartPosition;
    Vector3 TouchedPosition;

    bool StartTouchAndDrag = false;
    bool DoggoWantsToBeDragged = false;

    LayerMask DoggoDragMask;

    Vector2 LastKnownMousePosition;

    // Use this for initialization
    void Start()
    {
        DoggoDragMask = 1 << 8;
        LastKnownMousePosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDoggoTouch();
    }

    void CheckForDoggoTouch()
    {

#if UNITY_EDITOR
        if(DoggoWantsToBeDragged)
        {
            if (Input.GetMouseButton(0))
            {
                if (!StartTouchAndDrag)
                {
                    //Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                    //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                    //if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, DoggoDragMask))
                    //{
                        //hitObject = hit.collider.gameObject;
                        HOStartPosition = hitObject.transform.position;
                        TouchedPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                        StartTouchAndDrag = true;
                    //}
                }
                else if (StartTouchAndDrag)
                {
                    Vector3 CurrentTouchPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 NewHOPosition = hitObject.transform.position;
                    NewHOPosition = HOStartPosition + (CurrentTouchPosition - TouchedPosition);
                    hitObject.transform.position = new Vector3(NewHOPosition.x, NewHOPosition.y, HOStartPosition.z);
                }

            }
            else
            {
                StartTouchAndDrag = false;
                hitObject = null;
            }
        }
#endif

        if (Input.touchCount > 0)
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            foreach (Touch touch in Input.touches)
            {
                Ray ray = MainCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit, DoggoDragMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    touchList.Add(recipient);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }

                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    public void StartLiftingDoggo(GameObject a_HitObject)
    {
        hitObject = a_HitObject;
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
}
