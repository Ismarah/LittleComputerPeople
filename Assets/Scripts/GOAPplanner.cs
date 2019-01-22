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

    public void SetGoal(GameObject agent, WorldState.myStates newState, bool state)
    {
        if (!goalSet)
        {
            goalSet = true;
            currentGoal = NeedIndexToFulfillGoal(newState);
            currentAgent = agent;
            allPossibleChains = new List<ActionChain>();
            chain = new ActionChain();

            FindPossibilities(newState);

            FindActionChain(newState, state);
        }
    }

    private void FindActionChain(WorldState.myStates newState, bool state)
    {
        List<Action> possibleActions = FindActionsToFulfillCondition(currentAgent, newState, state);

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
                //chain.AddWalkTime(CalculateTimeToMove(possibleActions[i].GetObject()));
                completedChain = true;

                if (allPossibleChains.Count == possibilities)
                {
                    Debug.Log("Found all possible action chains. Count: " + allPossibleChains.Count);
                    FindBestActionChain();
                }
            }
            else //one or more conditions are not met yet
            {
                Dictionary<WorldState.myStates, bool> requiredConditions = GetRequiredConditions(possibleActions[i]);
                foreach (KeyValuePair<WorldState.myStates, bool> conditions in requiredConditions)
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

    private List<Action> FindActionsToFulfillCondition(GameObject agent, WorldState.myStates newState, bool state)
    {
        List<Action> possibleActions = new List<Action>();

        Action[] allActions = agent.GetComponent<AgentActions>().GetAllActions();

        for (int i = 0; i < allActions.Length; i++)
        {
            Dictionary<WorldState.myStates, bool> temp = allActions[i].GetEffects();

            if (temp.ContainsKey(newState))
            {
                if (temp[newState] == state)
                {
                    possibleActions.Add(allActions[i]);
                    Debug.Log("Found a possible action to change world state " + newState + " to " + state + "  : " + allActions[i].GetName());
                }
            }
        }

        return possibleActions;
    }

    private bool ConditionsMet(Action action)
    {
        Dictionary<WorldState.myStates, bool> conditions = action.GetPreconditions();
        int conditionCount = conditions.Keys.Count;
        int count = 0;

        foreach (KeyValuePair<WorldState.myStates, bool> condition in conditions)
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

    private Dictionary<WorldState.myStates, bool> GetRequiredConditions(Action action)
    {
        Dictionary<WorldState.myStates, bool> conditions = action.GetPreconditions();
        Dictionary< WorldState.myStates, bool> temp = new Dictionary<WorldState.myStates, bool>();

        foreach (KeyValuePair< WorldState.myStates, bool> condition in conditions)
        {
            if (WorldState.state.GetState(condition.Key) != condition.Value)
            {
                temp.Add(condition.Key, condition.Value);
            }
        }
        return temp;
    }

    private void FindPossibilities(WorldState.myStates newState)
    {
        switch (newState)
        {
            case WorldState.myStates.playerHasEaten:
                possibilities = 3;
                break;
            case WorldState.myStates.playerIsTired:
                possibilities = 2;
                break;
            case WorldState.myStates.playerWasOnToilet:
                possibilities = 1;
                break;
            case WorldState.myStates.playerHasNothingToDo:
                possibilities = 2;
                break;
            case WorldState.myStates.playerIsClean:
                possibilities = 2;
                break;
            default:
                break;
        }
    }

    private int NeedIndexToFulfillGoal(WorldState.myStates goal)
    {
        switch (goal)
        {
            case WorldState.myStates.playerHasEaten:
                return 0;
            case WorldState.myStates.playerIsTired:
                return 1;
            case WorldState.myStates.playerWasOnToilet:
                return 2;
            case WorldState.myStates.playerHasFun:
                return 3;
            case WorldState.myStates.playerIsClean:
                return 4;
            default:
                return -1;
        }
    }

    private float CalculateTimeToMove(GameObject target)
    {
        Debug.Log(target);
        int targetFloor = -1;
        if (target.GetComponent<InteractableItem>() != null)
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        else return 0;
        float distance = 0;
        float time = 0;
        int playersFloor = player.GetComponent<AgentMovement>().GetFloor();

        Transform firstStairsLower = player.GetComponent<AgentMovement>().GetStairs()[0];
        Transform firstStairsUpper = player.GetComponent<AgentMovement>().GetStairs()[1];
        Transform secondStairsLower = player.GetComponent<AgentMovement>().GetStairs()[2];
        Transform secondStairsUpper = player.GetComponent<AgentMovement>().GetStairs()[3];

        if (target.GetComponent<InteractableItem>() != null)
        {
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        }
        else if (target.GetComponent<AgentMovement>() != null)
        {
            targetFloor = target.GetComponent<AgentMovement>().GetFloor();
        }
        if (targetFloor == player.GetComponent<AgentMovement>().GetFloor())
        {
            distance = Vector2.Distance(player.transform.position, target.transform.position);
        }
        else
        {
            if (targetFloor > playersFloor)
            {
                if (targetFloor == playersFloor + 1)
                {
                    //target is one floor above player
                    if (playersFloor == 0)
                    {
                        distance = Vector2.Distance(player.transform.position, firstStairsLower.position);
                        distance += Vector2.Distance(firstStairsLower.position, firstStairsUpper.position);
                        distance += Vector2.Distance(firstStairsUpper.position, target.transform.position);
                    }
                    else if (playersFloor == 1)
                    {
                        distance = Vector2.Distance(player.transform.position, secondStairsLower.position);
                        distance += Vector2.Distance(secondStairsLower.position, secondStairsUpper.position);
                        distance += Vector2.Distance(secondStairsUpper.position, target.transform.position);
                    }

                }
                else if (targetFloor == playersFloor + 2)
                {
                    //target is two floors above player
                    distance = Vector2.Distance(player.transform.position, firstStairsLower.position);
                    distance += Vector2.Distance(firstStairsLower.position, firstStairsUpper.position);
                    distance += Vector2.Distance(firstStairsUpper.position, secondStairsLower.position);
                    distance += Vector2.Distance(secondStairsLower.position, secondStairsUpper.position);
                    distance += Vector2.Distance(secondStairsUpper.position, target.transform.position);
                }
            }
            else
            {
                if (targetFloor == playersFloor - 1)
                {
                    //target is one floor below player
                    if (playersFloor == 1)
                    {
                        distance = Vector2.Distance(player.transform.position, firstStairsUpper.position);
                        distance += Vector2.Distance(firstStairsUpper.position, firstStairsLower.position);
                        distance += Vector2.Distance(firstStairsLower.position, target.transform.position);
                    }
                    else if (playersFloor == 2)
                    {
                        distance = Vector2.Distance(player.transform.position, secondStairsUpper.position);
                        distance += Vector2.Distance(secondStairsUpper.position, secondStairsLower.position);
                        distance += Vector2.Distance(secondStairsLower.position, target.transform.position);
                    }

                }
                else if (targetFloor == playersFloor - 2)
                {
                    //target is two floors below player
                    distance = Vector2.Distance(player.transform.position, secondStairsUpper.position);
                    distance += Vector2.Distance(secondStairsUpper.position, secondStairsLower.position);
                    distance += Vector2.Distance(secondStairsLower.position, firstStairsUpper.position);
                    distance += Vector2.Distance(firstStairsUpper.position, firstStairsLower.position);
                    distance += Vector2.Distance(firstStairsLower.position, target.transform.position);
                }
            }
        }
        time = distance / player.GetComponent<AgentMovement>().GetMoveSpeed();
        Debug.Log("Time: " + time + " Target: " + target);
        return time;
    }
}
