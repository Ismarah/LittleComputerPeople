using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : InteractableItem
{
    void Start()
    {
        myFloor = 0;
        Init();
    }

    public override void AgentArrivedAtMyPosition(GameObject agent)
    {
        base.AgentArrivedAtMyPosition(agent);
        useCount++;

        if (useCount >= 1)
        {
            useCount = 0;
            WorldState.state.ChangeState(0, false);
        }
    }
}
