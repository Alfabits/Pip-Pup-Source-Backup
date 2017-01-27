using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailWag : MonoBehaviour {

    public GameObject TailWag1;
    public GameObject TailWag2;
    bool wagStarted = false;
    bool wagging = false;
    public bool WaggingTail
    {
        get { return wagging; }
        private set { wagging = value; }
    }

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void StartWaggingTail()
    {
        if (wagStarted == false)
        {
            wagStarted = true;
            ResumeWaggingTail();
            StartCoroutine(WagTail());
        }
    }

    public void StopWaggingTail()
    {
        if(wagStarted == true)
        {
            wagStarted = false;
            PauseWaggingTail();
        }
    }

    void PauseWaggingTail()
    {
        if(WaggingTail == true)
        {
            WaggingTail = false;
        }
    }

    void ResumeWaggingTail()
    {
        if(WaggingTail == false)
        {
            WaggingTail = true;
        }
    }

    IEnumerator WagTail()
    {
        int wagIndex = 0;
        while(WaggingTail)
        {
            switch(wagIndex)
            {
                case 0:
                    TailWag1.SetActive(false);
                    TailWag2.SetActive(true);
                    wagIndex = 1;
                    break;

                case 1:
                    TailWag2.SetActive(false);
                    TailWag1.SetActive(true);
                    wagIndex = 0;
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
}
