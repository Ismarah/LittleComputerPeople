using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : InteractableItem
{
    void Start()
    {
        Init();
        myFloor = 2;
    }

    public override void AgentArrivedAtMyPosition(GameObject agent)
    {
        if (agent == player)
        {
            if (nextPlayerActions[0].GetName() == "Put on street clothes")
                Invoke("StreetClothes", 0.5f);
            else
                Invoke("ChangeClothes", 0.5f);
        }
        base.AgentArrivedAtMyPosition(agent);
    }

    private void ChangeClothes()
    {
        player.GetComponent<PlayerVisuals>().ChangeClothes();
    }

    private void StreetClothes()
    {
        player.GetComponent<PlayerVisuals>().ReturnToDefaultClothes();
    }

}
