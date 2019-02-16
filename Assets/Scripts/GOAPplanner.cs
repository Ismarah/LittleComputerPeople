using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GOAPplanner : MonoBehaviour
{
    private GameObject player;
    private GameObject petFood;
    private List<ActionChain> allPossibleChains;
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

    public IEnumerator SetGoal(GameObject agent, WorldState.myStates newState, bool state)
    {
        if (!goalSet)
        {
            goalSet = true;
            currentGoal = NeedIndexToFulfillGoal(newState);
            currentAgent = agent;
            allPossibleChains = new List<ActionChain>();
            chain = new ActionChain();

            List<Action> possibleActions = FindActionsToFulfillCondition(currentAgent, newState, state);
            for (int i = 0; i < possibleActions.Count; i++)
            {
                yield return StartCoroutine(FindActionChain(possibleActions[i]));
            }
            //Debug.Log(allPossibleChains.Count + " chains found");
            FindBestActionChain();
        }
    }

    private IEnumerator FindActionChain(Action _possibleAction)
    {
        Action possibleAction = _possibleAction;
        //while (!completedChain)
        //{
        if (possibleAction == player.GetComponent<PlayerActions>().GetAction("Use toilet"))
            chain.Add(player.GetComponent<PlayerActions>().GetAction("Wash hands"));
        else if (possibleAction == player.GetComponent<PlayerActions>().GetAction("Sleep"))
            chain.Add(player.GetComponent<PlayerActions>().GetAction("Put on street clothes"));

        chain.Add(possibleAction);

        if (ConditionsMet(possibleAction)) //no further action is required to complete this action
        {
            chain.AddWalkTime(CalculateTimeToMove(possibleAction.GetObject()));
            allPossibleChains.Add(chain);

            completedChain = true;
        }
        else //one or more conditions are not met yet
        {
            Dictionary<WorldState.myStates, bool> requiredConditions = GetRequiredConditions(possibleAction);
            foreach (KeyValuePair<WorldState.myStates, bool> conditions in requiredConditions)
            {
                List<Action> newActions = FindActionsToFulfillCondition(currentAgent, conditions.Key, conditions.Value);
                if (newActions.Count != 0)
                {
                    for (int i = 0; i < newActions.Count; i++)
                    {
                        possibleAction = newActions[i];
                        yield return StartCoroutine(FindActionChain(newActions[i]));
                    }
                }
                else
                {
                    Debug.Log("No action found to fullfill condition " + conditions.Key + " to " + conditions.Value);
                }
            }
            //}
        }
        chain = new ActionChain();
        completedChain = false;
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
        if (bestValue != -1 && bestIndex != -1) AddChosenChainToQueue(bestIndex);
        else
        {
            Debug.Log("No possible chain :(");
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
                if (currentAgent == player)
                    GetComponent<PlayerQueue>().AddToQueue(newQueue[i]);
                else
                    GetComponent<PetQueue>().AddToQueue(newQueue[i]);
            }
        }
        goalSet = false;
    }

    private bool CheckForProblematicNeed(float time)
    {
        bool foundAProblem = false;
        for (int index = 0; index < currentAgent.GetComponent<AgentState>().GetActionCount(); index++)
        {
            if (index != currentGoal)
            {
                float stateAfterAction = currentAgent.GetComponent<AgentState>().GetNeedState(index) + currentAgent.GetComponent<AgentState>().GetNeedChange(index) * time * Time.deltaTime;

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
        List<Action> allActions = agent.GetComponent<AgentActions>().GetAllActions();

        for (int i = 0; i < allActions.Count; i++)
        {
            Dictionary<WorldState.myStates, bool> temp = allActions[i].GetEffects();

            if (temp.ContainsKey(newState))
            {
                if (temp[newState] == state)
                {
                    possibleActions.Add(allActions[i]);
                    //Debug.Log("Found a possible action to change world state " + newState + " to " + state + "  : " + allActions[i].GetName());
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
            //Debug.Log("Action " + action.GetName() + " needs Worldstate " + condition.Key + " to be " + condition.Value + ". It is currently " + WorldState.state.GetState(condition.Key));
            if (WorldState.state.GetState(condition.Key) == condition.Value) count++;
        }
        if (count == conditionCount) return true;
        else return false;
    }

    private Dictionary<WorldState.myStates, bool> GetRequiredConditions(Action action)
    {
        Dictionary<WorldState.myStates, bool> conditions = action.GetPreconditions();
        Dictionary<WorldState.myStates, bool> temp = new Dictionary<WorldState.myStates, bool>();

        foreach (KeyValuePair<WorldState.myStates, bool> condition in conditions)
        {
            if (WorldState.state.GetState(condition.Key) != condition.Value)
            {
                temp.Add(condition.Key, condition.Value);
            }
        }
        return temp;
    }

    private int NeedIndexToFulfillGoal(WorldState.myStates goal)
    {
        if (currentAgent == player)
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
        else
        {
            switch (goal)
            {
                case WorldState.myStates.petHasEaten:
                    return 0;
                case WorldState.myStates.petIsTired:
                    return 1;
                default:
                    return -1;
            }
        }
    }

    private float CalculateTimeToMove(GameObject target)
    {
        int targetFloor = -1;
        if (target.GetComponent<InteractableItem>() != null)
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        else return 0;
        float distance = 0;
        float time = 0;
        int agentFloor = currentAgent.GetComponent<AgentMovement>().GetFloor();

        Transform firstStairsLower = currentAgent.GetComponent<AgentMovement>().GetStairs()[0];
        Transform firstStairsUpper = currentAgent.GetComponent<AgentMovement>().GetStairs()[1];
        Transform secondStairsLower = currentAgent.GetComponent<AgentMovement>().GetStairs()[2];
        Transform secondStairsUpper = currentAgent.GetComponent<AgentMovement>().GetStairs()[3];

        if (target.GetComponent<InteractableItem>() != null)
        {
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        }
        else if (target.GetComponent<AgentMovement>() != null)
        {
            targetFloor = target.GetComponent<AgentMovement>().GetFloor();
        }
        if (targetFloor == currentAgent.GetComponent<AgentMovement>().GetFloor())
        {
            distance = Vector2.Distance(currentAgent.transform.position, target.transform.position);
        }
        else
        {
            if (targetFloor > agentFloor)
            {
                if (targetFloor == agentFloor + 1)
                {
                    //target is one floor above player
                    if (agentFloor == 0)
                    {
                        distance = Vector2.Distance(currentAgent.transform.position, firstStairsLower.position);
                        distance += Vector2.Distance(firstStairsLower.position, firstStairsUpper.position);
                        distance += Vector2.Distance(firstStairsUpper.position, target.transform.position);
                    }
                    else if (agentFloor == 1)
                    {
                        distance = Vector2.Distance(currentAgent.transform.position, secondStairsLower.position);
                        distance += Vector2.Distance(secondStairsLower.position, secondStairsUpper.position);
                        distance += Vector2.Distance(secondStairsUpper.position, target.transform.position);
                    }

                }
                else if (targetFloor == agentFloor + 2)
                {
                    //target is two floors above player
                    distance = Vector2.Distance(currentAgent.transform.position, firstStairsLower.position);
                    distance += Vector2.Distance(firstStairsLower.position, firstStairsUpper.position);
                    distance += Vector2.Distance(firstStairsUpper.position, secondStairsLower.position);
                    distance += Vector2.Distance(secondStairsLower.position, secondStairsUpper.position);
                    distance += Vector2.Distance(secondStairsUpper.position, target.transform.position);
                }
            }
            else
            {
                if (targetFloor == agentFloor - 1)
                {
                    //target is one floor below player
                    if (agentFloor == 1)
                    {
                        distance = Vector2.Distance(currentAgent.transform.position, firstStairsUpper.position);
                        distance += Vector2.Distance(firstStairsUpper.position, firstStairsLower.position);
                        distance += Vector2.Distance(firstStairsLower.position, target.transform.position);
                    }
                    else if (agentFloor == 2)
                    {
                        distance = Vector2.Distance(currentAgent.transform.position, secondStairsUpper.position);
                        distance += Vector2.Distance(secondStairsUpper.position, secondStairsLower.position);
                        distance += Vector2.Distance(secondStairsLower.position, target.transform.position);
                    }

                }
                else if (targetFloor == agentFloor - 2)
                {
                    //target is two floors below player
                    distance = Vector2.Distance(currentAgent.transform.position, secondStairsUpper.position);
                    distance += Vector2.Distance(secondStairsUpper.position, secondStairsLower.position);
                    distance += Vector2.Distance(secondStairsLower.position, firstStairsUpper.position);
                    distance += Vector2.Distance(firstStairsUpper.position, firstStairsLower.position);
                    distance += Vector2.Distance(firstStairsLower.position, target.transform.position);
                }
            }
        }
        time = distance / currentAgent.GetComponent<AgentMovement>().GetMoveSpeed();
        return time;
    }
}
