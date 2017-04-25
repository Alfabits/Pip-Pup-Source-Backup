using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStartupStatsChecker {

    public EventStartupStatsChecker()
    {

    }

	public bool StatsAreOkay(int a_CurrentEnergy, int a_EnergyReq, int a_CurrentHunger)
    {
        if (a_CurrentEnergy - a_EnergyReq >= 0 && a_CurrentHunger > 10)
            return true;

        return false;
    }
}
