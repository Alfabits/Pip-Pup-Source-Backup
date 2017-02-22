using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    List<GameObject> ButtonsList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Creates a button based on a prefab and several other arguments
    /// </summary>
    void CreateButton(GameObject a_Button, string a_TitleText, string a_StatusText = "")
    {
        GameObject objectToAdd = a_Button;

        Text titleText = objectToAdd.transform.FindChild("Title Text").GetComponent<Text>();
        Text statusText = objectToAdd.transform.FindChild("Status Text").GetComponent<Text>();

        titleText.text = a_TitleText;
        statusText.text = a_StatusText;

        ButtonsList = UIListPopulator.InsertButtonIntoList(this.gameObject, ButtonsList, objectToAdd);
    }
}
