using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPplanner : MonoBehaviour
{
    private GameObject player;
    private GameObject fridge;
    private GameObject toilet;
    private GameObject bed;
    private GameObject computer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fridge = GameObject.FindGameObjectWithTag("Fridge");
        toilet = GameObject.FindGameObjectWithTag("Toilet");
        bed = GameObject.FindGameObjectWithTag("Bed");
        computer = GameObject.FindGameObjectWithTag("Computer");
    }

    public void HaveFun()
    {
        if (!GetComponent<ActionQueue>().IsEnqueued("Computer"))
        {
            float[] costs = computer.GetComponent<InteractableItem>().GetActionCosts();
            int lowestCostIndex = 0;
            float lowestCost = float.MaxValue;
            for (int i = 0; i < costs.Length; i++)
            {
                if (costs[i] != 0 && costs[i] < lowestCost)
                {
                    lowestCostIndex = i;
                    lowestCost = costs[i];
                }
            }

            GetComponent<ActionQueue>().AddToQueue(computer, lowestCostIndex);
        }
    }

    public void EatSomething()
    {
        if (!GetComponent<ActionQueue>().IsEnqueued("Fridge"))
        {
            if (WorldState.state.GetState(0))
            {
                //food is available in the fridge
                float[] costs = fridge.GetComponent<InteractableItem>().GetActionCosts();
                int lowestCostIndex = 0;
                float lowestCost = float.MaxValue;
                for (int i = 0; i < costs.Length; i++)
                {
                    if (costs[i] != 0 && costs[i] < lowestCost)
                    {
                        lowestCostIndex = i;
                        lowestCost = costs[i];
                    }
                }

                GetComponent<ActionQueue>().AddToQueue(fridge, lowestCostIndex);
            }
        }
    }

    public void UseToilet()
    {
        if (!GetComponent<ActionQueue>().IsEnqueued("Toilet"))
        {
            if (WorldState.state.GetState(1))
            {
                //the toilet is clean
                GetComponent<ActionQueue>().AddToQueue(toilet, 0);
            }
            else
            {
                //the toilet has to be cleaned before using it again
                GetComponent<ActionQueue>().AddToQueue(toilet, 1);
            }
        }
    }

    public void GoToBed()
    {
        if (!GetComponent<ActionQueue>().IsEnqueued("Bed"))
        {
            float[] costs = bed.GetComponent<InteractableItem>().GetActionCosts();
            int lowestCostIndex = 0;
            float lowestCost = float.MaxValue;
            for (int i = 0; i < costs.Length; i++)
            {
                if (costs[i] != 0 && costs[i] < lowestCost)
                {
                    lowestCostIndex = i;
                    lowestCost = costs[i];
                }
            }

            GetComponent<ActionQueue>().AddToQueue(bed, lowestCostIndex);
        }
    }
}
