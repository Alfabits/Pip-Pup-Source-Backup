using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FloatingText : MonoBehaviour {

    public Text TextComponent;
    string TextToDisplay = "";
    bool Floating = false;
    float CurrentLifetime = 0.0f;

    public float MaximumLifeTime = 100.0f;
    public float timeInterval = 0.2f;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        CheckFloatText();
	}

    public void StartFloatingText()
    {
        TextComponent.text = TextToDisplay;
        Floating = true;
    }

    public void SetTextToDisplay(string a_Text)
    {
        TextToDisplay = a_Text;
    }

    void CheckFloatText()
    {
        if(Floating)
        {
            if (CurrentLifetime < MaximumLifeTime)
            {
                CurrentLifetime += timeInterval;
            }
            else if (CurrentLifetime >= MaximumLifeTime)
            {
                Floating = false;
            }
        }
        else
        {
            //Destroy this game object
            Destroy(this.gameObject);
        }
    }

    IEnumerator FloatText()
    {
        while(Floating)
        {
            
        }

        
        yield return null;
    }
}
