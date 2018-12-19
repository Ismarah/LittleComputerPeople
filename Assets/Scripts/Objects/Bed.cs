using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableItem
{
    Action sleep;
    Action doze;

    private void Start()
    {
        actionCount = 2;
        myFloor = 2;

        Init();

        //Possible action 1
        float[,] sleepEffect = new float[4, 2];
        sleepEffect[1, 0] = -1f;
        sleepEffect[1, 1] = 10;
        sleep = new Action(sleepEffect);
        myActions[0] = sleep;

        //Possible action 2
        float[,] dozeEffect = new float[4, 2];
        dozeEffect[1, 0] = -0.3f;
        dozeEffect[1, 1] = 2;
        doze = new Action(dozeEffect);
        myActions[1] = doze;
    }
}
