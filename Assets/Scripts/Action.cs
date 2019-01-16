using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    private float[,] actionStats; //index 0 = hunger, index 1 = sleep, index 3 = toilet, index 4 = fun; second float: 0 = change, 1 = time
    private Dictionary<int, bool> conditions;
    private Dictionary<int, bool> effects;
    private GameObject myObject;
    private string name;

    public Action(string _name, float[,] _actionStats, Dictionary<int, bool> _conditions, Dictionary<int, bool> _effects, GameObject obj)
    {
        actionStats = _actionStats;
        conditions = _conditions;
        effects = _effects;
        myObject = obj;
        name = _name;
    }

    public float[,] GetStats()
    {
        return actionStats;
    }

    public string GetName()
    {
        return name;
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

    public float GetStateChange()
    {
        float cost = 0;

        for (int i = 0; i < 5; i++)
        {
            cost += actionStats[i, 0] * actionStats[i, 1] * Time.deltaTime;
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
