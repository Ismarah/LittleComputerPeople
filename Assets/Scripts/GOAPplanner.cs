using System;
using System.Collections.Generic;
using UnityEngine;

public class GOAPplanner : MonoBehaviour
{
    private GameObject player;
    private GameObject petFood;
    private List<ActionChain> allPossibleChains;
    private int possibilities;
    private bool goalSet;
    private bool completedChain;
    private ActionChain chain;
    private int currentGoal;
    private GameObject currentAgent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        petFood = GameObject.FindGameObjectWithTag("PetFood");
    }

    public void SetGoal(GameObject agent, int index, bool state)
    {
        if (!goalSet)
        {
            goalSet = true;
            currentGoal = NeedIndexToFulfillGoal(index);
            currentAgent = agent;
            allPossibleChains = new List<ActionChain>();
            chain = new ActionChain();

            switch (index)
            {
                case 6:
                    possibilities = 3;
                    break;
                case 17:
                    possibilities = 2;
                    break;
                case 14:
                    possibilities = 1;
                    break;
                case 19:
                    possibilities = 2;
                    break;
                case 16:
                    possibilities = 2;
                    break;
                default:
                    break;
            }

            FindActionChain(index, state);
        }
    }

    private void FindActionChain(int index, bool state)
    {
        List<Action> possibleActions = FindActionsToFulfillCondition(currentAgent, index, state);

        for (int i = 0; i < possibleActions.Count; i++)
        {
            if (completedChain)
            {
                chain = new ActionChain();
                completedChain = false;
            }
            if (possibleActions[i] == player.GetComponent<PlayerActions>().GetAction(13))
            {
                chain.Add(player.GetComponent<PlayerActions>().GetAction(16));
            }
            chain.Add(possibleActions[i]);

            if (ConditionsMet(possibleActions[i])) //no further action is required to complete this action
            {
                allPossibleChains.Add(chain);
                completedChain = true;

                if (allPossibleChains.Count == possibilities)
                {
                    Debug.Log("Found all possible action chains. Count: " + allPossibleChains.Count);
                    FindBestActionChain();
                }
            }
            else //one or more conditions are not met yet
            {
                Dictionary<int, bool> requiredConditions = GetRequiredConditions(possibleActions[i]);

                foreach (KeyValuePair<int, bool> conditions in requiredConditions)
                {
                    FindActionChain(conditions.Key, conditions.Value);
                }
            }
        }
    }

    private void FindBestActionChain()
    {
        float[] allValues = new float[allPossibleChains.Count];
        for (int i = 0; i < allPossibleChains.Count; i++)
        {
            List<Action> temp = allPossibleChains[i].GetActions();
            for (int j = 0; j < temp.Count; j++)
            {
                allValues[i] += Mathf.Abs(temp[j].GetStateChange());
            }
        }

        Array.Sort(allValues);
        Array.Reverse(allValues);

        float bestValue = -1;
        int bestIndex = -1;

        for (int i = 0; i < allValues.Length; i++)
        {
            if (!CheckForProblematicNeed(allValues[i]))
            {
                bestValue = allValues[i];
                for (int j = 0; j < allPossibleChains.Count; j++)
                {
                    if (Mathf.Abs(allPossibleChains[j].GetChainStateChange()) == bestValue) bestIndex = j;
                }
                break;
            }

        }
        if (bestValue != -1 && bestIndex != -1)
        {
            Debug.Log("Decided that action at index " + bestIndex + " is the best option. Value change: " + bestValue + "  Time needed: " + allPossibleChains[bestIndex].GetChainDuration());
            AddChosenChainToQueue(bestIndex);
        }
        else
        {
            //no action can be done because other needs are more important
        }
    }

    private void AddChosenChainToQueue(int index)
    {
        List<Action> newQueue = allPossibleChains[index].GetActions();
        newQueue.Reverse();
        for (int i = 0; i < newQueue.Count; i++)
        {
            if (!GetComponent<ActionQueue>().IsEnqueued(newQueue[i]))
            {
                GetComponent<ActionQueue>().AddToQueue(newQueue[i], currentAgent);
            }
        }
        goalSet = false;
    }

    private bool CheckForProblematicNeed(float time)
    {
        bool foundAProblem = false;
        for (int index = 0; index < 5; index++)
        {
            if (index != currentGoal)
            {
                float stateAfterAction = player.GetComponent<PlayerState>().GetNeedState(index) + player.GetComponent<PlayerState>().GetNeedChange(index) * time * Time.deltaTime;

                if (stateAfterAction >= 0.85f)
                {
                    Debug.Log("Need " + index + " would be too low after action " + stateAfterAction);
                    foundAProblem = true;
                }
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
                    Debug.Log("Found a possible action to change world state " + index + " to " + state + "  : allActions[" + i + "]");
                }
            }
        }

        return possibleActions;
    }

    private bool ConditionsMet(Action action)
    {
        Dictionary<int, bool> conditions = action.GetPreconditions();
        int conditionCount = conditions.Keys.Count;
        int count = 0;

        foreach (KeyValuePair<int, bool> condition in conditions)
        {
            //Debug.Log("Worldstate " + condition.Key + " has to be " + condition.Value + ". It is currently " + WorldState.state.GetState(condition.Key));
            if (WorldState.state.GetState(condition.Key) == condition.Value)
            {
                count++;
            }
        }
        if (count == conditionCount)
        {
            //Debug.Log(count + " == " + conditionCount + "  all conditions seem to be met.");
            return true;
        }
        else return false;
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
            case 17:
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
