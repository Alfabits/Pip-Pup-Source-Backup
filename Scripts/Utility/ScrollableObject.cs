using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScrollableObject : MonoBehaviour
{

    RectTransform objRect;
    bool active;
    bool hiObj;
    bool lowObj;
    bool onlyObj;
    bool accessible;

    public bool ActiveInScene
    {
        get { return active; }
    }

    public bool IsHighestObject
    {
        get { return hiObj; }
        set { hiObj = value; }
    }

    public bool IsLowestObject
    {
        get { return lowObj; }
        set { lowObj = value; }
    }

    public bool IsOnlyObject
    {
        get { return onlyObj; }
        set { onlyObj = value; }
    }

    public bool IsAccessible
    {
        get { return accessible; }
    }

    // Use this for initialization
    void Start()
    {
        objRect = GetComponent<RectTransform>();
        if(objRect != null)
            accessible = true;
    }

    // Update is called once per frame
    void Update()
    {
        active = objRect ? true : false;
    }

    public void ChangeRectPosition(Vector2 a_NewPos)
    {
        objRect.anchoredPosition = new Vector3(objRect.anchoredPosition.x, a_NewPos.y);
    }

    public Vector2 GetAnchoredPosition()
    {
        if (objRect != null)
            return objRect.anchoredPosition;
        else
            return Vector2.zero;
    }

    public Vector2 GetTop()
    {
        return objRect.anchoredPosition + new Vector2(0.0f, objRect.rect.height / 2);
    }

    public Vector2 GetBottom()
    {
        return objRect.anchoredPosition - new Vector2(0.0f, objRect.rect.height / 2);
    }
}
