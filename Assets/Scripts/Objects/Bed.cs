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
    }
}
