using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private float[,] actionStats; //index 0 = hunger, index 1 = sleep, index 3 = toilet, index 4 = fun; second float: 0 = change, 1 = time
    private float time; //how long does it take to change the need

	public Action(float[,] _actionStats)
    {
        actionStats = _actionStats;
        for (int i = 0; i < 4; i++)
        {
            if(actionStats[i, 0] != 0)
            {
                time = actionStats[i, 1];
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

}
