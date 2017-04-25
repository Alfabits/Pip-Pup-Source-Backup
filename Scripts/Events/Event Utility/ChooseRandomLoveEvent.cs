using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChooseRandomLoveEvent {

    public List<GameEvent> PossibleLoveEvents;
    GameEvent m_Event;

	public ChooseRandomLoveEvent () {
        InitLoveEvents();
    }

    void InitLoveEvents()
    {
        PossibleLoveEvents = ReflectiveEnumerator.GetEnumerableOfType<GameEvent>().ToList();
        PossibleLoveEvents = GameManager.Instance.LoadEvents(PossibleLoveEvents);
        for (int i = 0, n = PossibleLoveEvents.Count; i < n; i++)
        {
            if(!PossibleLoveEvents[i].CheckIfLoveEvent())
            {
                PossibleLoveEvents.RemoveAt(i);
                i--;
                n--;
            }
        }
    }

    public void ChooseRandomEvent()
    {
        int rand = Random.Range(0, PossibleLoveEvents.Count - 1);
        m_Event = PossibleLoveEvents[rand];
    }

    public GameEvent GetChosenGameEvent()
    {
        return m_Event;
    }
}
