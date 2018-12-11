using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : InteractableItem
{

    void Start()
    {
        Init();
        myFloor = 0;
    }

    public override void PlayerArrivedAtMyPosition()
    {
        player.GetComponent<PlayerNeeds>().StartEating(0.5f, 6);
    }
}
