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
    GraphicRaycaster DoggoGraphicRaycaster;
    public RectTransform DoggoImage;

    public Text TempDebugText;

    int layer;
    int layermask;
    public LayerMask DoggoDragMask;

    Vector2 LastKnownMousePosition;

    // Use this for initialization
    void Start()
    {
        DoggoGraphicRaycaster = GetComponent<GraphicRaycaster>();
        layer = 3;
        layermask = 1 << layer;
        DoggoDragMask = layermask;
        LastKnownMousePosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDoggoTouch();
        TempDebugText.text = Input.mousePosition.x.ToString() + ", " + Input.mousePosition.y.ToString() + ", " + Input.mousePosition.z.ToString() + ", ";
    }

    Vector2 GetRelativeMouseMovement()
    {
        var CurrentMousePosition = Vector2.zero;



        return CurrentMousePosition;
    }

    void CheckForDoggoTouch()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            PointerEventData tempPointerEventData = new PointerEventData(null);
            tempPointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            DoggoGraphicRaycaster.Raycast(tempPointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.layer == DoggoDragMask)
                {
                    //Get the relative position of the dog to the screen width and height, to offset the canvas zero point.
                    Vector3 relativePosition = Vector3.zero;
                    float ScreenWidth = Screen.width;
                    float ScreenHeight = Screen.height;

                    //Apply the new position
                    relativePosition.x = (Input.mousePosition.x - (ScreenWidth / 2));
                    relativePosition.y = (Input.mousePosition.y - (ScreenHeight / 2));
                    result.gameObject.transform.localPosition = relativePosition;
                }
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
}
