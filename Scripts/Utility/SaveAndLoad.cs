using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{

    [SerializeField]
    bool UseGUITestingButtons = false;

    private static string SaveFilePath;

    string playerName = "";
    int level = 1;
    int love = 0;
    int hunger = 100;
    int energy = 100;
    int intelligence = 1;

    private void Awake()
    {
        SaveFilePath = Application.persistentDataPath + "/playerInfo.dat";
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveAllGameData()
    {
        BinaryFormatter BF = new BinaryFormatter();

        //Overwrites any existing files in the specified path. If no file exists in this path, a new file is created.
        FileStream file = File.Create(SaveFilePath);

        PlayerData data = new PlayerData();
        data.PlayerName = "Gud Boyy";
        data.Level = 2;
        data.Love = 30;
        data.Hunger = 34;
        data.Energy = 69;
        data.Intelligence = 3;

        BF.Serialize(file, data);
        file.Close();
    }

    public void LoadAllGameData()
    {
        if (File.Exists(SaveFilePath))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            PlayerData data = (PlayerData)BF.Deserialize(file);
            file.Close();

            playerName = data.PlayerName;
            level = data.Level;
            love = data.Love;
            hunger = data.Hunger;
            energy = data.Energy;
            intelligence = data.Intelligence;

            Debug.Log("Name is " + playerName + ", level is " + level + ", love is " + love + ", hunger is " + hunger + ", energy is " + energy + ", and intelligence is " + intelligence + ", bork bork!");
        }
    }

    public bool CheckForSaveFile()
    {
        return File.Exists(SaveFilePath);
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        if (UseGUITestingButtons)
        {
            if (GUI.Button(new Rect(10, 100, 100, 30), "Save"))
            {
                SaveAllGameData();
            }
            if (GUI.Button(new Rect(10, 140, 100, 30), "Load"))
            {
                LoadAllGameData();
            }
        }
#endif
    }


}

[System.Serializable]
class PlayerData
{
    /// <summary>
    /// The name of the doggo.
    /// </summary>
    public string PlayerName = "Good Boy";

    /// <summary>
    /// The doggo's level helps the story progress, and unlocks more things for the player to do.
    /// </summary>
    public int Level = 1;

    /// <summary>
    /// The amount of love the doggo feels. This essentially acts as experience points in an rpg.
    /// </summary>
    public int Love = 0;

    /// <summary>
    /// How hungry the puppy is. 100 = Full, 0 = Starving. Upon reaching less than 10 hunger, the puppy will stop talking and playing.
    /// </summary>
    public int Hunger = 100;

    /// <summary>
    /// Energy determines how many more activities the doggo can perform in one day. Energy resets every 24 hours, though some food can replenish it.
    /// </summary>
    public int Energy = 100;

    /// <summary>
    /// Intelligence refers to how much the doggo knows about the outside world. This only impacts which dialogue options the player can choose from.
    /// </summary>
    public int Intelligence = 1;
}
