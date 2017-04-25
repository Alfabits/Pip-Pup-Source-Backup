using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsChangeAnimation : MonoBehaviour {

    [SerializeField]
    GameObject m_StatBarToSpawn;
    Vector3 BAR_INIT_POSITION;
    const float BAR_OFFSET = 130.0f;
    const float BAR_FILL_EMPTY = 0.0f;
    const float BAR_FILL_FULL = 0.95f;


    // Use this for initialization
    void Start () {
        BAR_INIT_POSITION = new Vector3(0.0f, 560.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AssignStatsToChange(int[] a_OriginalStats, int[] a_NewStats)
    {
        GameObject[] statBarArray = new GameObject[3];
        for(int i = 0; i < 3; i++)
        {
            if(a_OriginalStats[i] > -1)
            {
                //Create and store the bar
                statBarArray[i] = Instantiate(m_StatBarToSpawn, Vector3.zero, Quaternion.identity, transform.FindChild("Doggo Menu Canvas"));
                statBarArray[i].transform.localPosition = BAR_INIT_POSITION;
                Text statText = statBarArray[i].transform.GetChild(1).GetComponent<Text>();

                //Sort the position of each bar
                Vector3 pos = statBarArray[i].transform.position;
                pos.y -= BAR_OFFSET * (i - 1);
                statBarArray[i].transform.position = pos;

                //Assign the correct label to each bar
                switch (i)
                {
                    case 1:
                        statText.text = "LOV";
                        break;
                    case 2:
                        statText.text = "HUN";
                        break;
                    case 3:
                        statText.text = "ENE";
                        break;
                }

                //Assign the correct fill percentage
                GameObject fill = statBarArray[i].transform.FindChild("Stat Bar").FindChild("Stat Bar Fill").gameObject;
                if (fill)
                {
                    //Normalize to the fill range
                    int normalizedCurrentValue = Normalize(a_OriginalStats[i], 0, 100, (int)BAR_FILL_EMPTY, (int)BAR_FILL_FULL);
                    Vector3 scale = fill.transform.localScale;
                    scale.x = normalizedCurrentValue;
                }
                else
                {
                    Debug.LogError("Fill not found, unable to proceed with animation");
                    break;
                }
            }
        }
    }

    //TODO: move this to a custom math library
    /// <summary>
    /// Takes a value and its current range, and scales it down to the desired range.
    /// </summary>
    /// <param name="a_Value"></param>
    /// <param name="a_OldMinimum"></param>
    /// <param name="a_OldMaximum"></param>
    /// <param name="a_NewMinimum"></param>
    /// <param name="a_NewMaximum"></param>
    /// <returns></returns>
    int Normalize(int a_Value, int a_OldMinimum, int a_OldMaximum, int a_NewMinimum, int a_NewMaximum)
    {
        int normalizedValue = 0;
        int NewRange = (a_NewMaximum - a_NewMinimum);
        int OldRange = (a_OldMaximum - a_OldMinimum);

        normalizedValue = (((a_Value - a_OldMinimum) * NewRange) / OldRange) + a_NewMinimum;

        return normalizedValue;
    }
}
