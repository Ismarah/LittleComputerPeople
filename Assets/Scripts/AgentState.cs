using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentState : MonoBehaviour
{
    protected bool askedForAction;
    protected GameObject manager;
    [SerializeField]
    protected float[] currentNeeds;
    [SerializeField]
    protected float[] needChanges;
    [SerializeField]
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

    protected void CheckNeedStates()
    {
        for (int i = 0; i < currentNeeds.Length; i++)
        {
            if (currentNeeds[i] >= criticalValues[i])
            {
                foreach (KeyValuePair<WorldState.myStates, bool> pair in stateChanges[i])
                {
                    WorldState.state.ChangeState(pair.Key, pair.Value);
                }
                if (!askedForAction)
                {
                    askedForAction = true;
                    StartCoroutine(manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, goals[i].Key, goals[i].Value));
                }
            }
        }
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
        yield return new WaitForSecondsRealtime(time);

        for (int i = 0; i < needChanges.Length; i++)
        {
            needChanges[i] -= temp[i];
        }
    }

    public int GetActionCount()
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
