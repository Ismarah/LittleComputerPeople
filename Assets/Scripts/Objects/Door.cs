using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableItem
{
    void Start()
    {
        myFloor = 0;
        Init();
    }
}
