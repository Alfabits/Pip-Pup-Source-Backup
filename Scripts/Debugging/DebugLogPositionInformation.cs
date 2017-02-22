using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogPositionInformation : MonoBehaviour {

    [SerializeField]
    private string ObjectName = "";
    [SerializeField]
    private Vector3 RectTransformPosition = Vector3.zero;
    [SerializeField]
    private float TimeSinceStart = 0.0f;
    [SerializeField, Range(0, 1000000)]
    private float ComparisonThreshold = 500.0f;

    Vector3 PreviousRectTransformPosition = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ObjectName = gameObject.name;
        PreviousRectTransformPosition = RectTransformPosition;
        RectTransformPosition = GetComponent<RectTransform>().anchoredPosition;
        TimeSinceStart = Time.unscaledTime;

        CompareCurrentAndPreviousRectTransformPositions();
	}

    void CompareCurrentAndPreviousRectTransformPositions()
    {
        if(Mathf.Abs(RectTransformPosition.x - PreviousRectTransformPosition.x) > ComparisonThreshold ||
           Mathf.Abs(RectTransformPosition.y - PreviousRectTransformPosition.y) > ComparisonThreshold ||
           Mathf.Abs(RectTransformPosition.z - PreviousRectTransformPosition.z) > ComparisonThreshold)
        {
            Debug.Log("Position drastically changed from <" + PreviousRectTransformPosition + "> to <" + RectTransformPosition + "> at: <" + Time.unscaledTime + ">.");
        }
    }
}
