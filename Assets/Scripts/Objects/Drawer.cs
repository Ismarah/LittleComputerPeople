using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : InteractableItem
{
	void Start ()
	{
		Init ();
		myFloor = 1;
	}

    public override void PlayerArrivedAtMyPosition()
    {
        player.GetComponent<Player>().ChangeClothes();
    }
}
