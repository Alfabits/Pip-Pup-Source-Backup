using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    public float MinRightBounds = 595;
    public float MaxRightBounds = 0;

    public float LoadWaitRate = 0.05f;

    public RectTransform LoadingBarGraphic;
    public Text LoadingPercentageText;

    float StepInterval = 0.0f;


    // Use this for initialization
    void Start () {
        MaxRightBounds = LoadingBarGraphic.offsetMax.x;
        MinRightBounds = -LoadingBarGraphic.rect.width;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void BeginLoading()
    {
        StartCoroutine(Load());
    }

    public void SetStepInterval(int NumberOfSteps)
    {
        StepInterval = Mathf.Abs((Mathf.Abs(MaxRightBounds) + Mathf.Abs(MinRightBounds)) / NumberOfSteps);
    }

    public void SetLoadWaitRate(float a_Value)
    {
        LoadWaitRate = a_Value;
    }

    IEnumerator Load()
    {
        float AbsGoal = Mathf.Abs(MaxRightBounds) + Mathf.Abs(MinRightBounds);
        float loadAmount = 0;
        float loadPercent = 0;

        for(float i = MinRightBounds; i < MaxRightBounds; i+=StepInterval)
        {
            Vector3 rectOffset = LoadingBarGraphic.offsetMax;
            rectOffset.x = i;
            LoadingBarGraphic.offsetMax = rectOffset;

            loadAmount += StepInterval;
            loadPercent = (loadAmount / AbsGoal) * 100;
            LoadingPercentageText.text = ((int)loadPercent).ToString() + " %";

            yield return new WaitForSeconds(LoadWaitRate);
        }
        Debug.Log("Finished loading");
        yield return null;
    }
}
