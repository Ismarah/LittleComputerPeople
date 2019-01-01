using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : InteractableItem
{
    Action meal;
    Action snack;
    Action refill;

    void Start()
    {
        actionCount = 3;
        myFloor = 0;

        Init();
    }

    public override void UseMe()
    {
        useCount++;

        if (useCount >= 1)
        {
            useCount = 0;
            WorldState.state.ChangeState(0, false);
        }
    }
}
