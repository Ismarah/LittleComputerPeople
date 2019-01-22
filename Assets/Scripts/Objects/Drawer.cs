using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : InteractableItem
{
	void Start ()
	{
		Init ();
		myFloor = 2;
	}

    public override void PlayerArrivedAtMyPosition()
    {
        base.PlayerArrivedAtMyPosition();

        Invoke("ChangeClothes", 0.5f);
    }

    private void ChangeClothes()
    {
        player.GetComponent<PlayerVisuals>().ChangeClothes();
    }

}
