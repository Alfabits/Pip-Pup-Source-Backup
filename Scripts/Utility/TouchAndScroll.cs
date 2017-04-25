using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class TouchAndScroll : MonoBehaviour
{
    GameManager GM;
    LoadingManager LM;

    public List<ScrollableObject> AllScrollableObjects;
    public Camera MainCamera;

    List<ScrollableObject> ActiveScrollableObjects;

    bool Touching = false;
    bool CouldTouch = false;
    bool CanTouch = false;
    float ContainerHeight;
    Vector3 TouchedPosition;
    Vector3 CurrentTouchPosition;

    RaycastHit hit;
    Canvas ThisCanvas;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Startup());
    }

    IEnumerator Startup()
    {
        ActiveScrollableObjects = new List<ScrollableObject>();
        AssignActiveScrollableObjects();

        CurrentTouchPosition = Vector3.zero;
        TouchedPosition = CurrentTouchPosition;
        ThisCanvas = GetComponent<Canvas>();

        InitHighestAndLowestObjects();

        while (GM == null)
        {
            GM = GameManager.Instance;
            yield return null;
        }
        while (LM == null)
        {
            LM = LoadingManager.Instance;
            yield return null;
        }

        //Check in with the loading manager
        LM.CheckIn(gameObject, LoadingManager.KeysForScriptsToBeLoaded.TouchAndScroll, true);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (GM != null)
        {
            //NOTE: This never happens after CanTouch is true, since this object deactivates before the bool can trigger
            if (GM.IsAnEventPlaying() && CanTouch == true)
            {
                CanTouch = false;
            }
            else if(!GM.IsAnEventPlaying() && CanTouch == false)
            {
                CanTouch = true;
            }

            //Check if the player is touching
            UpdateIfTouching();

            //If the player is touching, update the position of the scrollableobjects
            if (Touching && CanTouch)
                UpdateScrollablePositions();

            UpdateOutOfBounds();
        }
    }

    public void ResetActiveLists()
    {
        AssignActiveScrollableObjects();
        InitHighestAndLowestObjects();
    }

    void AssignActiveScrollableObjects()
    {
        ActiveScrollableObjects.Clear();

        for (int i = 0, n = AllScrollableObjects.Count; i < n; i++)
        {
            if (AllScrollableObjects[i].gameObject.activeInHierarchy)
                ActiveScrollableObjects.Add(AllScrollableObjects[i]);
        }
    }

    void InitHighestAndLowestObjects()
    {
        if (ActiveScrollableObjects.Count <= 0)
            return;

        ScrollableObject highestObj = ActiveScrollableObjects[0];
        ScrollableObject lowestObj = ActiveScrollableObjects[0];
        for (int i = 0, n = ActiveScrollableObjects.Count; i < n; i++)
        {
            if (ActiveScrollableObjects[i].IsAccessible)
            {
                if (ActiveScrollableObjects[i].GetAnchoredPosition().y > highestObj.GetAnchoredPosition().y)
                {
                    highestObj = ActiveScrollableObjects[i];
                }
                else if (ActiveScrollableObjects[i].GetAnchoredPosition().y < lowestObj.GetAnchoredPosition().y)
                {
                    lowestObj = ActiveScrollableObjects[i];
                }
            }
        }

        if (highestObj == lowestObj)
        {
            highestObj.IsHighestObject = false;
            lowestObj.IsLowestObject = false;
            highestObj.IsOnlyObject = true;
        }
        else
        {
            highestObj.IsHighestObject = true;
            lowestObj.IsLowestObject = true;
        }
    }

    void UpdateIfTouching()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && !Touching)
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && hit.collider && hit.transform.tag == "Scroll List")
            {
                Touching = true;
                UpdateCurrentTouchPosition();
                ContainerHeight = hit.collider.GetComponent<RectTransform>().rect.height;
            }
            else
            {
                Touching = false;
                ContainerHeight = 0.0f;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Touching = false;
            ContainerHeight = 0.0f;
        }
#elif UNITY_ANDROID
        //if(Input.touchCount >= 1)
        //{
        //    Ray ray = MainCamera.ScreenPointToRay(Input.touches[0].position);

        //    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && hit.collider && hit.transform.tag == "Scroll List")
        //    {
        //        Touching = true;
        //    }
        //}
        //else
        //{
        //    Touching = false;
        //}
#endif
    }

    void UpdateScrollablePositions()
    {
        //Set TouchedPosition to the CurrentTouchPosition
        TouchedPosition = CurrentTouchPosition;

        //Determine the next CurrentTouchPosition based on platform
        UpdateCurrentTouchPosition();

        Vector3 NewPosition = Vector3.zero;
        Vector3 StartPosition = Vector3.zero;
        Vector3 TouchDifference = CurrentTouchPosition - TouchedPosition;
        bool canMove = true;

        //Check whether any of the objects are unable to move
        for (int i = 0, n = ActiveScrollableObjects.Count; i < n; i++)
        {
            if (ActiveScrollableObjects[i] != null && ActiveScrollableObjects[i].ActiveInScene)
            {
                //Calculate the NewPosition while we're here
                StartPosition = ActiveScrollableObjects[i].GetAnchoredPosition();
                NewPosition = ActiveScrollableObjects[i].GetAnchoredPosition();
                NewPosition = StartPosition + TouchDifference;

                //Check if the highest object is going to be out of bounds, then the lowest object
                if (ActiveScrollableObjects[i].IsHighestObject)
                {
                    float objBoundsCheckUpper = (ActiveScrollableObjects[i].GetTop() + new Vector2(TouchDifference.x, TouchDifference.y)).y;
                    if (objBoundsCheckUpper < -5.0f)
                    {
                        canMove = false;
                        break;
                    }
                }
                else if (ActiveScrollableObjects[i].IsLowestObject)
                {
                    float objBoundsCheckLower = (ActiveScrollableObjects[i].GetBottom() + new Vector2(TouchDifference.x, TouchDifference.y)).y;
                    if (objBoundsCheckLower > -ContainerHeight)
                    {
                        canMove = false;
                        break;
                    }
                }
                else if (ActiveScrollableObjects[i].IsOnlyObject)
                {
                    float objBoundsCheckUpper = (ActiveScrollableObjects[i].GetTop() + new Vector2(TouchDifference.x, TouchDifference.y)).y;
                    float objBoundsCheckLower = (ActiveScrollableObjects[i].GetBottom() + new Vector2(TouchDifference.x, TouchDifference.y)).y;
                    if (objBoundsCheckUpper < -5.0f)
                    {
                        canMove = false;
                        break;
                    }
                    if (objBoundsCheckLower > -ContainerHeight)
                    {
                        canMove = false;
                        break;
                    }
                }
            }
        }

        //Now that we've confirmed that every object can be moved, let's move them
        if (canMove)
        {
            for (int i = 0, n = AllScrollableObjects.Count; i < n; i++)
            {
                if (AllScrollableObjects[i] != null && AllScrollableObjects[i].ActiveInScene)
                {
                    //Calculate the NewPosition while we're here
                    StartPosition = AllScrollableObjects[i].GetAnchoredPosition();
                    NewPosition = AllScrollableObjects[i].GetAnchoredPosition();
                    NewPosition = StartPosition + (CurrentTouchPosition - TouchedPosition);

                    if (AllScrollableObjects[i] != null && AllScrollableObjects[i].ActiveInScene)
                    {
                        AllScrollableObjects[i].ChangeRectPosition(NewPosition);
                    }
                }
            }
        }
    }

    void UpdateOutOfBounds()
    {
        for (int i = 0, n = ActiveScrollableObjects.Count; i < n; i++)
        {
            if(ActiveScrollableObjects[i].GetTop().y > 0.0f)
            {
                ActiveScrollableObjects[i].ChangeRectPosition(new Vector2(ActiveScrollableObjects[i].GetAnchoredPosition().x, 0.0f - ActiveScrollableObjects[i].GetAnchoredPosition().y));
            }
            else if(ActiveScrollableObjects[i].GetBottom().y < -ContainerHeight)
            {
                ActiveScrollableObjects[i].ChangeRectPosition(new Vector2(ActiveScrollableObjects[i].GetAnchoredPosition().x, -ContainerHeight + ActiveScrollableObjects[i].GetAnchoredPosition().y));
            }
        }
    }

    void UpdateCurrentTouchPosition()
    {
        Vector2 pos;

#if UNITY_EDITOR
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ThisCanvas.transform as RectTransform, Input.mousePosition, ThisCanvas.worldCamera, out pos);
        CurrentTouchPosition = new Vector3(pos.x, pos.y, 0.0f);
#elif UNITY_ANDROID
        CurrentTouchPosition = MainCamera.ScreenToWorldPoint(Input.touches[0].position);
#endif
    }
}
