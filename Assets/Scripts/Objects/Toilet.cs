using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : InteractableItem
{

	void Start ()
	{
		Init ();
		myFloor = 0;
	}

    public override void PlayerArrivedAtMyPosition()
    {
        player.GetComponent<PlayerNeeds>().StartUsingToilet(1, 12);
    }
}
