using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : InteractableItem {

    void Start()
    {
        Init();
        myFloor = 1;
    }

    public override void PlayerArrivedAtMyPosition()
    {
        //player.GetComponent<PlayerNeeds>().StartHavingFun(0.8f, 8);
    }
}
