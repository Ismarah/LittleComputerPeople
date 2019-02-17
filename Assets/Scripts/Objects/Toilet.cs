﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : InteractableItem
{
    private Animator anim;
    void Start()
    {
        myFloor = 0;
        Init();
        anim = GetComponent<Animator>();
    }

    public override void AgentArrivedAtMyPosition(GameObject agent)
    {
        if(nextActions[0].GetName() != "Clean toilet")
        {
            useCount++;

            if (useCount >= 3)
            {
                useCount = 0;
                WorldState.state.ChangeState(WorldState.myStates.toiletIsClean, false);
            }

            anim.SetBool(myAnimation, true);
            anim.speed = GameObject.FindGameObjectWithTag("ActionQueue").GetComponent<TimeManager>().GetGameSpeed();
        }

        base.AgentArrivedAtMyPosition(agent);
    }
}
