using UnityEngine;
using System.Collections;

public class ScrollMaterial : MonoBehaviour {

    public float ScrollSpeed;

    Material Target;

	// Use this for initialization
	void Start () {
        Target = this.GetComponent<Renderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 Offset = new Vector2(0.0f, 0.0f);
        Offset.x = Mathf.Repeat(Time.time * ScrollSpeed, 1.0f);
        Offset.y = Mathf.Repeat(Time.time * ScrollSpeed, 1.0f);
        Target.SetTextureOffset("_MainTex", Offset);
    }
}
