using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : InteractableItem
{

    void Start()
    {
        actionCount = 1;
        myFloor = 0;

        Init();
    }

    
}
