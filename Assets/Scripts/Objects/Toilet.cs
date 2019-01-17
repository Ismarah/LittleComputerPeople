using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : InteractableItem
{
    Action useToilet;
    Action cleanToilet;

    void Start()
    {
        actionCount = 2;
        myFloor = 0;

        Init();
    }

    public override void UseMe()
    {
        useCount++;

        if (useCount >= 3)
        {
            useCount = 0;
            WorldState.state.ChangeState(3, false);
        }
    }
}
