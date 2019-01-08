﻿using System;
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
    private List<ActionChain> allPossibleChains;
    private int possibilities;
    private bool goalSet;
    private bool pathsFound;
    private bool completedChain;
    private ActionChain chain;
    private int currentGoal;

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
            currentGoal = NeedIndexToFulfillGoal(index);
            allPossibleChains = new List<ActionChain>();
            chain = new ActionChain();

            switch (index)
            {
                case 6:
                    possibilities = 3;
                    break;
                default:
                    break;
            }

            FindActionChain(agent, index, state);
        }
    }

    private void FindActionChain(GameObject agent, int index, bool state)
    {
        List<Action> possibleActions = FindActionsToFulfillCondition(agent, index, state);

        for (int i = 0; i < possibleActions.Count; i++)
        {

            if (completedChain)
            {
                chain = new ActionChain();
                completedChain = false;
            }

            chain.Add(possibleActions[i]);

            if (ConditionsMet(possibleActions[i])) //no further action is required to complete this action
            {
                allPossibleChains.Add(chain);
                Debug.Log("Possible chains: " + allPossibleChains.Count);
                completedChain = true;

                if (allPossibleChains.Count == possibilities)
                {
                    FindBestActionChain();
                }
            }
            else //one or more conditions are not met yet
            {
                Dictionary<int, bool> requiredConditions = GetRequiredConditions(possibleActions[i]);

                foreach (KeyValuePair<int, bool> conditions in requiredConditions)
                {
                    FindActionChain(agent, conditions.Key, conditions.Value);
                }
            }
        }

    }


    private void FindBestActionChain()
    {
        float[,] chainCost = new float[allPossibleChains.Count, 2];
        float[] allValues = new float[allPossibleChains.Count];
        for (int i = 0; i < allPossibleChains.Count; i++)
        {
            List<Action> temp = allPossibleChains[i].GetActions();
            for (int j = 0; j < temp.Count; j++)
            {
                chainCost[i, 0] += temp[j].GetCost();
                chainCost[i, 1] += temp[j].GetTime();
                allValues[i] += Mathf.Abs(temp[j].GetCost());
            }
        }

        Array.Sort(allValues);
        Array.Reverse(allValues);

        float bestValue = -1;

        for (int i = 0; i < allValues.Length; i++)
        {
            if (!CheckForProblematicNeed(allValues[i]))
            {
                bestValue = allValues[i];
                break;
            }
        }
        Debug.Log("Best option: " + bestValue);
    }

    private bool CheckForProblematicNeed(float time)
    {
        bool foundAProblem = false;
        for (int index = 0; index < 5; index++)
        {
            float stateAfterAction = player.GetComponent<PlayerState>().GetNeedState(index) + player.GetComponent<PlayerState>().GetNeedChange(index) * time * 1 / Time.deltaTime;

            if (stateAfterAction >= 0.9f)
            {
                Debug.Log("Need " + index + " would be too low after action");
                foundAProblem = true;
            }
        }
        return foundAProblem;
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
                    //Debug.Log("Found a possible action to change world state " + index + " to " + state + "  : allActions[" + i + "]");
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

    private int NeedIndexToFulfillGoal(int goal)
    {
        switch (goal)
        {
            case 6:
                return 0;
            case 10:
                return 1;
            case 14:
                return 2;
            case 15:
                return 3;
            case 16:
                return 4;
            default:
                return -1;
        }
    }
}
