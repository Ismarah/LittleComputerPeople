﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private float[,] actionStats; //index 0 = hunger, index 1 = sleep, index 3 = toilet, index 4 = fun; second float: 0 = change, 1 = time, 2 = precondition
    private float time; //how long does it take to change the need
    Dictionary<int, bool> conditions;
    Dictionary<int, bool> effects;


    public Action(float[,] _actionStats, Dictionary<int, bool> _conditions, Dictionary<int, bool> _effects, GameObject obj)
    {
        actionStats = _actionStats;
        conditions = _conditions;
        effects = _effects;

        for (int i = 0; i < 4; i++)
        {
            if(actionStats[i, 0] != 0)
            {
                time = actionStats[i, 1];
                break;
            }
        }
    }

    public float[,] GetStats()
    {
        return actionStats;
    }

    public float GetCost()
    {
        return time;
    }

    public Dictionary<int, bool> GetPreconditions()
    {
        return conditions;
    }

    public Dictionary<int, bool> GetEffects()
    {
        return effects;
    }

}
