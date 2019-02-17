using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GOAPplanner : MonoBehaviour
{
    private GameObject player;
    private GameObject petFood;
    private List<ActionChain> allPossibleChains;
    //private bool goalSet;
    private bool completedChain;
    private ActionChain chain;
    private int currentGoal;
    private GameObject currentAgent;
    private int mostUrgentNeedIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        petFood = GameObject.FindGameObjectWithTag("PetFood");
    }

    public IEnumerator SetGoal(GameObject agent, WorldState.myStates newState, bool state, int index)
    {
        //if (!goalSet)
        //{
        //    goalSet = true;
        currentGoal = index;
        Debug.Log("New goal " + newState + " " + state + "    Goal index: " + currentGoal);
        currentAgent = agent;
        allPossibleChains = new List<ActionChain>();
        chain = new ActionChain();

        List<Action> possibleActions = FindActionsToFulfillCondition(currentAgent, newState, state);
        for (int i = 0; i < possibleActions.Count; i++)
        {
            yield return StartCoroutine(FindActionChain(possibleActions[i]));
        }
        if (newState == WorldState.myStates.playerWasOnToilet)
            Debug.Log("Satisfy toilet: " + allPossibleChains[0].GetName());
        else Debug.Log(allPossibleChains.Count + " chains found");
        FindBestActionChain();
        //}
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

            string name = "";
            for (int i = 0; i < chain.GetActions().Count; i++)
            {
                name += chain.GetActions()[i].GetName();
            }
            chain.SetName(name);
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
            allValues[i] = GetChainStateChange(allPossibleChains[i]);
        }

        Array.Sort(allValues);
        Array.Reverse(allValues);

        float bestValue = -1;
        int bestIndex = -1;

        for (int i = 0; i < allValues.Length; i++)
        {
            if (!CheckForProblematicNeed(allPossibleChains[i].GetChainDuration() / GetComponent<TimeManager>().GetGameSpeed(), i))
            {
                bestValue = allValues[i];
                for (int j = 0; j < allPossibleChains.Count; j++)
                {
                    if (GetChainStateChange(allPossibleChains[j]) == bestValue)
                    {
                        bestIndex = j;
                    }
                }
                break;
            }
        }
        if (bestValue != -1 && bestIndex != -1) AddChosenChainToQueue(bestIndex);
        else
        {
            Debug.Log("No possible chain :( Next most urgent need index: " + mostUrgentNeedIndex);
            //no action can be done because other needs are more important
            currentAgent.GetComponent<AgentState>().SatisfySecondMostUrgentNeed(mostUrgentNeedIndex);
        }
    }

    private float GetActionStateChange(Action action)
    {
        float change = 0;
        for (int k = 0; k < action.GetStats().Length; k++)
        {
            change = action.GetStats()[k] * action.GetTime() / GetComponent<TimeManager>().GetGameSpeed();
        }
        return change;
    }

    private float GetChainStateChange(ActionChain actionChain)
    {
        float change = 0;
        List<Action> temp = actionChain.GetActions();
        for (int j = 0; j < temp.Count; j++)
        {

            change -= GetActionStateChange(temp[j]);
        }
        return change;
    }

    private bool CheckForProblematicNeed(float time, int chosenChain)
    {
        //TODO calculate in chain change
        bool foundAProblem = false;
        for (int index = 0; index < currentAgent.GetComponent<AgentState>().GetNeedCount(); index++)
        {
            if (index != currentGoal)
            {
                float stateAfterChain = currentAgent.GetComponent<AgentState>().GetNeedState(index) + (currentAgent.GetComponent<AgentState>().GetNeedChange(index) * time * (1 / Time.deltaTime) * Time.deltaTime * GetComponent<TimeManager>().GetGameSpeed());
                if (stateAfterChain >= 0.85f)
                {
                    Debug.Log("Need " + index + " would be too low after " + time + " seconds. Chain: " + allPossibleChains[chosenChain].GetName() + "   (" + stateAfterChain + ")");
                    mostUrgentNeedIndex = index;
                    foundAProblem = true;
                }
            }
            else
            {
                float stateAfterAction = currentAgent.GetComponent<AgentState>().GetNeedState(currentGoal) + (currentAgent.GetComponent<AgentState>().GetNeedChange(currentGoal) * time * (1 / Time.deltaTime) * Time.deltaTime * GetComponent<TimeManager>().GetGameSpeed());
                if (stateAfterAction >= 1)
                {
                    Debug.Log("Need " + index + " would be too low after chain " + allPossibleChains[chosenChain].GetName() + "   (" + stateAfterAction + ")");
                    mostUrgentNeedIndex = index;
                    foundAProblem = true;
                }
            }
        }
        Debug.Log("Found a problem: " + foundAProblem);
        return foundAProblem;
    }

    private void AddChosenChainToQueue(int index)
    {
        List<Action> newQueue = allPossibleChains[index].GetActions();
        newQueue.Reverse();
        for (int i = 0; i < newQueue.Count; i++)
        {
            if (!GetComponent<ActionQueue>().IsEnqueued(newQueue[i]))
            {
                Debug.Log("Add chain to queue " + allPossibleChains[index].GetName());
                if (currentAgent == player)
                    GetComponent<PlayerQueue>().AddToQueue(newQueue[i]);
                else
                    GetComponent<PetQueue>().AddToQueue(newQueue[i]);
            }
        }
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
