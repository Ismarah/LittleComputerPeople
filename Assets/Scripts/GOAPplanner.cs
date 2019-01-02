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
    private GameObject petFood;

    private bool goalSet;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fridge = GameObject.FindGameObjectWithTag("Fridge");
        toilet = GameObject.FindGameObjectWithTag("Toilet");
        bed = GameObject.FindGameObjectWithTag("Bed");
        computer = GameObject.FindGameObjectWithTag("Computer");
        petFood = GameObject.FindGameObjectWithTag("PetFood");
    }

    public void SetGoal(GameObject agent, int index, bool state)
    {
        if (!goalSet)
        {
            goalSet = true;
            Action[] allActions = agent.GetComponent<AgentActions>().GetAllActions();

            for (int i = 0; i < allActions.Length; i++)
            {
                Dictionary<int, bool> temp = allActions[i].GetEffects();

                if (temp.ContainsKey(index))
                {
                    if (temp[index] == state)
                    {
                        Debug.Log("Found a possible action to satisfy hunger: allActions[" + i + "]");
                    }
                }
            }
        }
    }

    //public void HaveFun()
    //{
    //    if (!GetComponent<ActionQueue>().IsEnqueued("Computer"))
    //    {
    //        float[] costs = computer.GetComponent<InteractableItem>().GetActionCosts();
    //        int lowestCostIndex = 0;
    //        float lowestCost = float.MaxValue;
    //        for (int i = 0; i < costs.Length; i++)
    //        {
    //            if (costs[i] != 0 && costs[i] < lowestCost)
    //            {
    //                lowestCostIndex = i;
    //                lowestCost = costs[i];
    //            }
    //        }

    //        GetComponent<ActionQueue>().AddToQueue(computer, lowestCostIndex, -1);
    //    }
    //}

    //public void EatSomething()
    //{
    //    if (!GetComponent<ActionQueue>().IsEnqueued("Fridge"))
    //    {
    //        if (WorldState.state.GetState(0))
    //        {
    //            //food is available in the fridge
    //            float[] costs = fridge.GetComponent<InteractableItem>().GetActionCosts();
    //            int lowestCostIndex = 0;
    //            float lowestCost = float.MaxValue;
    //            for (int i = 0; i < costs.Length; i++)
    //            {
    //                if (costs[i] != 0 && costs[i] < lowestCost)
    //                {
    //                    lowestCostIndex = i;
    //                    lowestCost = costs[i];
    //                }
    //            }

    //            GetComponent<ActionQueue>().AddToQueue(fridge, lowestCostIndex, 0, 0);
    //        }
    //        else
    //        {
    //            GetComponent<ActionQueue>().AddToQueue(fridge, 2, 0, 1);


    //        }
    //    }
    //}

    //public void UseToilet()
    //{
    //    if (WorldState.state.GetState(1)) //toilet is clean
    //    {
    //        if (!GetComponent<ActionQueue>().IsEnqueued("Toilet", 0)) //Using toilet action is not yet queued
    //        {
    //            GetComponent<ActionQueue>().AddToQueue(toilet, 0);
    //        }

    //    }
    //    else //the toilet has to be cleaned before using it again
    //    {
    //        if (!GetComponent<ActionQueue>().IsEnqueued("Toilet", 1)) //Cleaning toilet action is not yet queued
    //        {
    //            GetComponent<ActionQueue>().AddToQueue(toilet, 1);
    //            GetComponent<ActionQueue>().AddToQueue(toilet, 0);
    //        }
    //    }
    //}

    //public void GoToBed()
    //{
    //    if (!GetComponent<ActionQueue>().IsEnqueued("Bed"))
    //    {
    //        float[] costs = bed.GetComponent<InteractableItem>().GetActionCosts();
    //        int lowestCostIndex = 0;
    //        float lowestCost = float.MaxValue;
    //        for (int i = 0; i < costs.Length; i++)
    //        {
    //            if (costs[i] != 0 && costs[i] < lowestCost)
    //            {
    //                lowestCostIndex = i;
    //                lowestCost = costs[i];
    //            }
    //        }

    //        GetComponent<ActionQueue>().AddToQueue(bed, lowestCostIndex);
    //    }
    //}
}
