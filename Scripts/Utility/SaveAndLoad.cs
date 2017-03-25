using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{
    [SerializeField]
    bool UseGUITestingButtons = false;

    private bool FirstTimePlaying = false;
    private static string SaveFilePath;

    private void Awake()
    {
        SaveFilePath = Application.persistentDataPath + "/playerInfo.sav";
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

    public bool IsFirstTimePlaying()
    {
        return FirstTimePlaying;
    }

    public void CreateInitialSaveFile()
    {
        BinaryFormatter BF = new BinaryFormatter();

        //Overwrites any existing files in the specified path. If no file exists in this path, a new file is created.
        FileStream file = File.Create(SaveFilePath);

        SaveData data = new SaveData();
        data.playerData = new PlayerData();
        data.eventDataList = new EventDataList();

        data.playerData.PlayerName = "Gud Boyy";
        data.playerData.Level = 2;
        data.playerData.Love = 30;
        data.playerData.Hunger = 34;
        data.playerData.Energy = 69;
        data.playerData.Intelligence = 3;

        data.eventDataList.EventList = new List<EventData>();
        List<GameEvent> GameEventCollection = ReflectiveEnumerator.GetEnumerableOfType<GameEvent>().ToList();
        data.eventDataList = AssignEventListToList(GameEventCollection, data.eventDataList);

        try
        {
            BF.Serialize(file, data);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            file.Close();
        }

        FirstTimePlaying = true;
    }

    public GameEvent LoadEvent(GameEvent a_Event)
    {
        if (File.Exists(SaveFilePath))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            SaveData data = (SaveData)BF.Deserialize(file);
            file.Close();

            //Search for a game event in the list that matches the passed event's name
            for (int i = 0, n = data.eventDataList.EventList.Count; i < n; i++)
            {
                if(data.eventDataList.EventList[i].eventname == a_Event.GetEventName())
                {
                    a_Event.SetIsCompleted(data.eventDataList.EventList[i].completed);
                    a_Event.SetDailyComletion(data.eventDataList.EventList[i].dailycompleted);
                    a_Event.SetUnlockedStatus(data.eventDataList.EventList[i].unlocked);
                }
            }

            //Return the modified game event
            return a_Event;
        }

        return a_Event;
    }

    public List<GameEvent> LoadEvents(List<GameEvent> a_Events)
    {
        if (File.Exists(SaveFilePath))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            SaveData data = (SaveData)BF.Deserialize(file);
            file.Close();

            for(int j = 0, k = a_Events.Count; j < k; j++)
            {
                //Search for a game event in the list that matches the passed event's name
                for (int i = 0, n = data.eventDataList.EventList.Count; i < n; i++)
                {
                    if (data.eventDataList.EventList[i].eventname == a_Events[j].GetEventName())
                    {
                        a_Events[j].SetIsCompleted(data.eventDataList.EventList[i].completed);
                        a_Events[j].SetDailyComletion(data.eventDataList.EventList[i].dailycompleted);
                        a_Events[j].SetUnlockedStatus(data.eventDataList.EventList[i].unlocked);
                    }
                }
            }
            

            //Return the modified game events
            return a_Events;
        }

        return a_Events;
    }

    public void SaveEvent(GameEvent a_Event)
    {
        //If no file exists at the specified path, we cannot save anything
        if (File.Exists(SaveFilePath))
        {
            //Create a new binary formatter, open a read-only file, and copy the SaveData from the save file
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            SaveData data = (SaveData)BF.Deserialize(file);
            EventDataList dataList = data.eventDataList;

            //Close the file, then re-open it to be written to
            file.Close();
            file = File.Create(SaveFilePath);

            //If a data list does not exist, create one and assign this event to it.
            if (dataList == null)
            {
                dataList = AssignEventToList(a_Event, dataList);
            }
            else
            {
                //If a list already exists, check to see if this event already exists inside of it.
                bool found = false;
                for (int i = 0, n = dataList.EventList.Count; i < n; i++)
                {
                    if (dataList.EventList[i].eventname == a_Event.GetEventName())
                    {
                        //If so, update the pre-existing event data
                        Debug.Log("updating event: " + dataList.EventList[i].eventname);
                        dataList.EventList[i].completed = a_Event.CheckForFirstTimeCompletion();
                        dataList.EventList[i].unlocked = a_Event.CheckIfUnlocked();
                        dataList.EventList[i].dailycompleted = a_Event.CheckForDailyCompletion();

                        found = true;
                    }

                    //If this event's type matches any of the events to be unlocked, then unlock this event
                    if (a_Event.CheckForFirstTimeCompletion())
                    {
                        for (int j = 0, k = a_Event.EventsToBeUnlockedAfterCompletion.Count; j < k; j++)
                        {
                            if (dataList.EventList[i].eventname == a_Event.EventsToBeUnlockedAfterCompletion[j]
                                && dataList.EventList[i].unlocked == false)
                            {
                                dataList.EventList[i].unlocked = true;
                            }
                        }
                    }
                }

                //If not, add the event data to the list
                if (!found)
                {
                    Debug.Log("new event added: ");
                    dataList = AssignEventToList(a_Event, dataList);
                }
            }

            //Reassign dataList to the SaveData
            data.eventDataList = dataList;

            //Attempt to serialize the data
            try
            {
                BF.Serialize(file, data);
            }
            catch (SerializationException e)
            {
                Debug.LogError("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                file.Close();
            }
        }
        else
        {
            Debug.LogError("Error: no file to save to. Please call \"CreateInitialSaveFile()\" first.");
        }
    }

    public void SaveListOfEvents(List<GameEvent> a_Events)
    {
        //If no file exists at the specified path, we cannot save anything
        if (File.Exists(SaveFilePath))
        {
            //Create a new binary formatter, open a read-only file, and copy the SaveData from the save file
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            SaveData data = (SaveData)BF.Deserialize(file);
            EventDataList dataList = data.eventDataList;

            //Close the file, then re-open it to be written to
            file.Close();
            file = File.Create(SaveFilePath);

            //If a data list does not exist, create one and assign this event to it.
            if (dataList == null)
            {
                dataList = AssignEventListToList(a_Events, dataList);
            }
            else
            {
                for (int j = 0, k = a_Events.Count; j < k; j++)
                {
                    //If a list already exists, check to see if this event already exists inside of it.
                    bool found = false;
                    for (int i = 0, n = dataList.EventList.Count; i < n; i++)
                    {
                        if (dataList.EventList[i].eventname == a_Events[j].GetEventName())
                        {
                            //If so, update the pre-existing event data
                            Debug.Log("updating event: " + dataList.EventList[i].eventname);
                            dataList.EventList[i].completed = a_Events[j].CheckForFirstTimeCompletion();
                            dataList.EventList[i].unlocked = a_Events[j].CheckIfUnlocked();
                            dataList.EventList[i].dailycompleted = a_Events[j].CheckForDailyCompletion();
                            found = true;

                            break;
                        }

                        //If this event's type matches any of the events to be unlocked, then unlock this event
                        for (int l = 0, m = a_Events[j].EventsToBeUnlockedAfterCompletion.Count; l < m; l++)
                        {
                            if (dataList.EventList[i].eventname == a_Events[j].EventsToBeUnlockedAfterCompletion[l]
                                && dataList.EventList[i].unlocked == false)
                            {
                                dataList.EventList[i].unlocked = true;
                            }
                        }
                    }

                    //If not, add the event list to the list
                    if (!found)
                    {
                        Debug.Log("new event added: ");
                        dataList = AssignEventToList(a_Events[j], dataList);
                    }
                }
            }

            //Reassign dataList to the SaveData
            data.eventDataList = dataList;

            //Attempt to serialize the data
            try
            {
                BF.Serialize(file, data);
            }
            catch (SerializationException e)
            {
                Debug.LogError("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                file.Close();
                Debug.Log("File saved successfully");
            }
        }
        else
        {
            Debug.LogError("Error: no file to save to. Please call \"CreateInitialSaveFile()\" first.");
        }
    }

    public bool CheckForSaveFile()
    {
        return File.Exists(SaveFilePath);
    }

    private EventDataList AssignEventToList(GameEvent a_Event, EventDataList a_DataList)
    {
        if (a_DataList == null)
            a_DataList = new EventDataList();
        EventData data = new EventData();

        data.eventname = a_Event.GetEventName();
        Debug.Log(data.eventname);
        data.unlocked = a_Event.CheckIfUnlocked();
        data.completed = a_Event.CheckForFirstTimeCompletion();
        data.dailycompleted = a_Event.CheckForDailyCompletion();

        a_DataList.EventList.Add(data);
        return a_DataList;
    }

    private EventDataList AssignEventListToList(List<GameEvent> a_Events, EventDataList a_DataList)
    {
        if (a_DataList == null)
            a_DataList = new EventDataList();

        for (int i = 0, n = a_Events.Count; i < n; i++)
        {
            EventData data = new EventData();

            data.eventname = a_Events[i].GetEventName();
            Debug.Log(data.eventname);
            data.unlocked = a_Events[i].CheckIfUnlocked();
            data.completed = a_Events[i].CheckForFirstTimeCompletion();
            data.dailycompleted = a_Events[i].CheckForDailyCompletion();

            a_DataList.EventList.Add(data);
        }
        return a_DataList;
    }
}

[System.Serializable]
class SaveData
{
    public PlayerData playerData;
    public EventDataList eventDataList;
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

[System.Serializable]
class EventData
{
    public bool unlocked = false;
    public bool completed = false;
    public int dailycompleted = 0;
    public string eventname = "nameless";
}

[System.Serializable]
class EventDataList
{
    public List<EventData> EventList;
}
