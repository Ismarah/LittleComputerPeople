using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : InteractableItem
{
    void Start()
    {
        myFloor = 0;
        Init();
    }

    public override void PlayerArrivedAtMyPosition()
    {
        base.PlayerArrivedAtMyPosition();
        useCount++;

        if (useCount >= 3)
        {
            useCount = 0;
            WorldState.state.ChangeState(WorldState.myStates.toiletIsClean, false);
        }
    }
}
