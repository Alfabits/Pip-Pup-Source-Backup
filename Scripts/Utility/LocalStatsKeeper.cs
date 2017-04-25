using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalStatsKeeper : MonoBehaviour{

	void Start()
    {
        GM = GameManager.Instance;
        if (GM)
            SaveAndLoader = GM.SaveAndLoader;
        StatAnimator = GetComponent<StatsChangeAnimation>();
        _Time = DateTime.Now;

        LoadStats();
        Debug.Log("Level: " + Level + " | Love: " + Love + " | Hunger: " + Hunger + " | Energy: " + Energy);
        Debug.Log("Stats Loaded");

        _Time = DateTime.Now;
    }

    void Update()
    {
        UpdateHunger();
        UpdateEnergy();
    }

    void UpdateAllLocalStats()
    {
        _Time = DateTime.Now;

        UpdateHunger();
        UpdateEnergy();
    }

    public void DoggoHasBeenFed()
    {
        Hunger += 50;
        Energy += 50;

        SaveStats();
    }

    void UpdateHunger()
    {
        if(Hunger > 0)
        {
            if (_Time.Minute % 30 == 0)
            {
                if (Energy > 50)
                    Hunger -= 1;
                else
                    Hunger -= 3;
                SaveStats();
            }
        }

        if (Hunger > 100)
            Hunger = 100;
    }

    void UpdateEnergy()
    {
        //Make it so the energy resets after 24 hours
        if(_Time.Hour % 24 == 0 && _Time.Second % 60 == 0)
        {
            Energy = 100;
        }

        if (Energy > 100)
            Energy = 100;
    }

    void LoadStats()
    {
        int[] stats = SaveAndLoader.GetAllStats();

        Level = stats[0];
        Love = stats[1];
        Hunger = stats[2];
        Energy = stats[3];

        stats = null;

        UpdateAllLocalStats();
    }

    void SaveStats()
    {
        UpdateAllLocalStats();

        int[] stats = { 0, 0, 0, 0 };
        stats[0] = Level;
        stats[1] = Love;
        stats[2] = Hunger;
        stats[3] = Energy;

        Debug.Log("Level: " + Level + " | Love: " + Love + " | Hunger: " + Hunger + " | Energy: " + Energy);

        SaveAndLoader.SaveStats(stats);
        Debug.Log("Stats Saved");
    }

    void AnimateStatChange(int[] a_OldValues, int[] a_NewValues)
    {
        StatAnimator.AssignStatsToChange(a_OldValues, a_NewValues);
    }

    public void UpdateStats(GameEvent a_Event)
    {
        //Safety check
        if ((a_Event.DoesEventRunOnlyOnce() && a_Event.CheckForFirstTimeCompletion()) || a_Event.CheckForDailyCompletion() == 1)
            return;

        int[] stats = { -1, Love, Hunger, Energy };
        int[] newStats = { -1, a_Event.GetLoveGiven(), -1, a_Event.GetEnergyRequired() };
        AnimateStatChange(stats, newStats);

        Love += a_Event.GetLoveGiven();
        Energy -= a_Event.GetEnergyRequired();

        if (Love >= 100)
            LevelUp();

        SaveStats();
    }

    void LevelUp()
    {
        if(Level < 99)
            Level += 1;
        Love -= 100;
        Energy += 50;
        if (Energy > 100)
            Energy = 100;
    }

    #region PrivateVariables
    private GameManager GM;
    private SaveAndLoad SaveAndLoader;
    private StatsChangeAnimation StatAnimator;
    private DateTime _Time;
    private float TimeCounter = 0.0f;
    #endregion
    #region Stats
    int hunger = 100;
    int energy = 100;
    int love = 0;
    int level = 1;
    #endregion
    #region StatProperties
    public int Hunger
    {
        get { return hunger; }
        private set { hunger = value; }
    }
    public int Energy
    {
        get { return energy; }
        private set { energy = value; }
    }
    public int Love
    {
        get { return love; }
        private set { love = value; }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    #endregion
}
