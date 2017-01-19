using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkOnAndOff : MonoBehaviour {

    bool On = false;
    bool Off = false;
    bool Waiting = false;

    public float BlinkRate = 1.0f;

    public GameObject BlinkTarget;

	// Use this for initialization
	void Start () {
        if (this.gameObject.activeInHierarchy == true)
            On = true;
        else
            Off = true;

        Waiting = true;
        StartCoroutine(Blink());
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Waiting = false;
        }

        if(On)
        {
            BlinkTarget.SetActive(true);
        }
        else if(Off)
        {
            BlinkTarget.SetActive(false);
        }
	}

    IEnumerator Blink()
    {
        while(Waiting)
        {
            yield return new WaitForSeconds(BlinkRate);
            if (On && !Off)
            {
                On = false;
                Off = true;
            }
            else if (Off && !On)
            {
                Off = false;
                On = true;
            }
        }
    }
}
