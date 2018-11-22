using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableItem
{
	private void Start ()
	{
		Init ();
		myFloor = 1;
	}

    public override void PlayerArrivedAtMyPosition()
    {
        player.GetComponent<PlayerNeeds>().StartSleep(1, 2);
    }
}
