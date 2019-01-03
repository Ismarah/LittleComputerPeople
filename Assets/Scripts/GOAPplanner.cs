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
    List<ActionChain> allPossibleChains;

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
            allPossibleChains = new List<ActionChain>();
            ActionChain newChain = new ActionChain();

            FindActionChain(agent, index, state, newChain);
        }
    }

    private void FindActionChain(GameObject agent, int index, bool state, ActionChain chain)
    {
        List<Action> possibleActions = FindActionsToFulfillCondition(agent, index, state);

        for (int i = 0; i < possibleActions.Count; i++)
        {
            chain.Add(possibleActions[i]);
            if (ConditionsMet(possibleActions[i])) //no further action is required to complete this action
            {
                Debug.Log("Conditions are met for: allActions[" + i + "]");
                allPossibleChains.Add(chain);
            }
            else //one ore more conditions are not met yet
            {
                Dictionary<int, bool> requiredConditions = GetRequiredConditions(possibleActions[i]);

                foreach (KeyValuePair<int, bool> conditions in requiredConditions)
                {
                    FindActionChain(agent, conditions.Key, conditions.Value, chain);
                }
            }
        }
        //float[,] stats = allActions[i].GetStats();


    }

    private List<Action> FindActionsToFulfillCondition(GameObject agent, int index, bool state)
    {
        List<Action> possibleActions = new List<Action>();

        Action[] allActions = agent.GetComponent<AgentActions>().GetAllActions();

        for (int i = 0; i < allActions.Length; i++)
        {
            Dictionary<int, bool> temp = allActions[i].GetEffects();

            if (temp.ContainsKey(index))
            {
                if (temp[index] == state)
                {
                    possibleActions.Add(allActions[i]);
                    Debug.Log("Found a possible action to change world state " + index + " to " + state + "  : allActions[" + i + "]");
                }
            }
        }

        return possibleActions;
    }

    private bool ConditionsMet(Action action)
    {
        Dictionary<int, bool> conditions = action.GetPreconditions();

        foreach (KeyValuePair<int, bool> condition in conditions)
        {
            if (WorldState.state.GetState(condition.Key) == condition.Value)
            {
                return true;
            }
        }
        return false;
    }

    private Dictionary<int, bool> GetRequiredConditions(Action action)
    {
        Dictionary<int, bool> conditions = action.GetPreconditions();
        Dictionary<int, bool> temp = new Dictionary<int, bool>();

        foreach (KeyValuePair<int, bool> condition in conditions)
        {
            if (WorldState.state.GetState(condition.Key) != condition.Value)
            {
                temp.Add(condition.Key, condition.Value);
            }
        }
        return temp;
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
