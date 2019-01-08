using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

    private float[,] actionStats; //index 0 = hunger, index 1 = sleep, index 3 = toilet, index 4 = fun; second float: 0 = change, 1 = time
    private Dictionary<int, bool> conditions;
    private Dictionary<int, bool> effects;
    private GameObject myObject;


    public Action(float[,] _actionStats, Dictionary<int, bool> _conditions, Dictionary<int, bool> _effects, GameObject obj)
    {
        actionStats = _actionStats;
        conditions = _conditions;
        effects = _effects;
        myObject = obj;
    }

    public float[,] GetStats()
    {
        return actionStats;
    }

    public float GetTime(int i)
    {
        return actionStats[i, 1];
    }

    public float GetTime()
    {
        float time = 0;

        for (int i = 0; i < 5; i++)
        {
            time += actionStats[i, 1];
        }

        return time;
    }

    public float GetCost()
    {
        float cost = 0;

        for (int i = 0; i < 5; i++)
        {
            cost += actionStats[i, 0] * actionStats[i, 1] * 60;
        }

        return cost;
    }

    public Dictionary<int, bool> GetPreconditions()
    {
        return conditions;
    }

    public Dictionary<int, bool> GetEffects()
    {
        return effects;
    }

    public GameObject GetObject()
    {
        return myObject;
    }

}
