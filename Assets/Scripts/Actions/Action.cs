using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private float[,] actionStats; //index 0 = hunger, index 1 = sleep, index 3 = toilet, index 4 = fun; second float: 0 = change, 1 = time
    private float change; //which need does this satisfy
    private float time; //how long does it take to change the need

	public Action(float[,] _actionStats)
    {
        actionStats = _actionStats;
    }

    public float[,] GetStats()
    {
        return actionStats;
    }

    public float GetCost()
    {
        return time;
    }

    public float GetChange()
    {
        return change;
    }

}
