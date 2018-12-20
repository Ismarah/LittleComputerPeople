﻿using System.Collections;
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

        //Possible action 1
        float[,] useToiletEffect = new float[4, 2];
        useToiletEffect[2, 0] = -0.3f;
        useToiletEffect[2, 1] = 3;
        useToilet = new Action(useToiletEffect);
        myActions[0] = useToilet;

        //Possible action 2
        float[,] cleanToiletEffect = new float[4, 2];
        cleanToiletEffect[3, 0] = 0.1f;
        cleanToiletEffect[3, 1] = 3;
        cleanToilet = new Action(cleanToiletEffect);
        myActions[1] = cleanToilet;
    }

    public override void UseMe()
    {
        useCount++;

        if (useCount >= 3)
        {
            useCount = 0;
            WorldState.state.ChangeState(1, false);
        }
    }
}
