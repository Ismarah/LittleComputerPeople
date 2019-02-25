using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentState : MonoBehaviour
{
    protected bool askedForAction;
    protected GameObject manager;
    protected float[] currentNeeds;
    protected float[] needChanges;
    protected float[] criticalValues;
    protected List<Dictionary<WorldState.myStates, bool>> stateChanges;
    protected List<KeyValuePair<WorldState.myStates, bool>> goals;
    protected bool needWasChanged;

    public void Init()
    {
        manager = GameObject.FindGameObjectWithTag("ActionQueue");
    }

    protected IEnumerator NeedChange()
    {
        while (true)
        {
            for (int i = 0; i < needChanges.Length; i++)
            {
                currentNeeds[i] += needChanges[i] * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void ActionIsPlanned()
    {
        askedForAction = true;
    }

    public void ActionFinished()
    {
        askedForAction = false;
    }

    protected void CheckNeedStates()
    {
        int index = -1;
        float value = 0;
        for (int i = 0; i < currentNeeds.Length; i++)
        {
            if (currentNeeds[i] >= criticalValues[i])
            {
                if (currentNeeds[i] > value)
                {
                    value = currentNeeds[i];
                    index = i;
                }
            }
        }
        if (index >= 0)
        {
            foreach (KeyValuePair<WorldState.myStates, bool> pair in stateChanges[index])
            {
                WorldState.state.ChangeState(pair.Key, pair.Value);
            }
            askedForAction = true;
            StartCoroutine(manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, goals[index].Key, goals[index].Value, index));
        }
    }

    public void SatisfySecondMostUrgentNeed(int mostUrgent)
    {
        StartCoroutine(manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, goals[mostUrgent].Key, goals[mostUrgent].Value, mostUrgent));
    }

    protected virtual IEnumerator NeedChangeForATime(Action action)
    {
        float[] temp = action.GetStats();
        float time = -1;
        needWasChanged = false;
        float change = 0;

        if (action.GetTime() != 0) time = action.GetTime();
        for (int i = 0; i < needChanges.Length; i++)
        {
            needChanges[i] += temp[i];
        }

        for (int i = 0; i < temp.Length; i++)
        {
            change = temp[i];
            if (change != 0 && action.GetName() != "Use toilet")
            {
                needWasChanged = true;
                break;
            }
        }
        yield return new WaitForSeconds(time);

        for (int i = 0; i < needChanges.Length; i++)
        {
            needChanges[i] -= temp[i];
        }
    }

    public int GetNeedCount()
    {
        return currentNeeds.Length;
    }

    public float GetNeedState(int index)
    {
        return currentNeeds[index];
    }

    public float GetNeedChange(int index)
    {
        return needChanges[index];
    }

    public float GetCriticalValue(int index)
    {
        return criticalValues[index];
    }
}
