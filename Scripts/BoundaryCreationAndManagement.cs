using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCreationAndManagement : MonoBehaviour {

    LoadingManager LM;
    GameManager GMInstance;
    Camera MainCamera;

    public BoxCollider TopWall;
    public BoxCollider BottomWall;
    public BoxCollider RightWall;
    public BoxCollider LeftWall;

    bool WallsAreSetup = false;

    // Use this for initialization
    void Start () {
        GMInstance = GameManager.Instance;
        if(GMInstance.Accessible)
        {
            MainCamera = GMInstance.MainCamera;
            SetupWalls();
        }

        LM = LoadingManager.Instance;

        //Check in with the loading manager
        LM.CheckIn(this.gameObject, LoadingManager.KeysForScriptsToBeLoaded.BoundaryCreationAndManagement, true);
    }
	
	// Update is called once per frame
	void Update () {
        if (!WallsAreSetup)
        {
            if(GMInstance.Accessible)
            {
                MainCamera = GMInstance.MainCamera;
                SetupWalls();
            }
        }
	}

    void SetupWalls()
    {
        //Align the walls
        TopWall.size = new Vector3(MainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2.0f, 0.0f, 0.0f)).x, 1.0f, 1.0f);
        TopWall.transform.position = new Vector2(0.0f, MainCamera.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 0.0f)).y + 0.5f);

        BottomWall.size = new Vector3(MainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2.0f, 0.0f, 0.0f)).x, 1.0f, 1.0f);
        BottomWall.transform.position = new Vector2(0.0f, MainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y - 0.5f);

        LeftWall.size = new Vector3(1.0f, MainCamera.ScreenToWorldPoint(new Vector3(1.0f, Screen.height * 2.0f, 0.0f)).y, 1.0f);
        LeftWall.transform.position = new Vector2(MainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x - 0.5f, 0.0f);

        RightWall.size = new Vector3(1.0f, MainCamera.ScreenToWorldPoint(new Vector3(1.0f, Screen.height * 2.0f, 0.0f)).y, 1.0f);
        RightWall.transform.position = new Vector2(MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x + 0.5f, 0.0f);

        WallsAreSetup = true;
    }
}
