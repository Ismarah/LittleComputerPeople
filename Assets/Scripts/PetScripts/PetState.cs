using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetState : AgentState
{
    [SerializeField]
    private float hungerChange, sleepChange, funChange;
    [SerializeField]
    private float hungry, sleepy, bored;

    private void Start()
    {
        base.Init();

        currentNeeds = new float[3];
        needChanges = new float[3];
        needChanges[0] = hungerChange;
        needChanges[1] = sleepChange;
        needChanges[2] = funChange;

        criticalValues = new float[3];
        criticalValues[0] = hungry;
        criticalValues[1] = sleepy;
        criticalValues[2] = bored;

        stateChanges = new List<Dictionary<WorldState.myStates, bool>>();
        Dictionary<WorldState.myStates, bool> dict = new Dictionary<WorldState.myStates, bool>();
        dict.Add(WorldState.myStates.petHasEaten, false);
        dict.Add(WorldState.myStates.petIsHungry, true);
        stateChanges.Add(dict);
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.petIsTired, true } });
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.petIsBored, true } });

        goals = new List<KeyValuePair<WorldState.myStates, bool>>();
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.petAskedForFood, true));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.petIsTired, false));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.petIsBored, false));

        StartCoroutine(NeedChange());
    }

    private void Update()
    {
        if (!askedForAction)
            CheckNeedStates();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < currentNeeds.Length; i++)
        {
            if (currentNeeds[i] < 0) currentNeeds[i] = 0;
            else if (currentNeeds[i] > 1) currentNeeds[i] = 1;
        }
    }

    public void ManipulateNeedChange(Action action)
    {
        if (action.GetTime() != 0) StartCoroutine(NeedChangeForATime(action));
    }

    protected override IEnumerator NeedChangeForATime(Action action)
    {
        yield return StartCoroutine(base.NeedChangeForATime(action));

        if (needWasChanged) manager.GetComponent<PetQueue>().FinishedAction(true);
        else manager.GetComponent<PetQueue>().FinishedAction(false);
    }

    public override void SatisfyMostUrgentNeed(int mostUrgent)
    {
        StartCoroutine(manager.GetComponent<PetGOAP>().SetGoal(this.gameObject, goals[mostUrgent].Key, goals[mostUrgent].Value, mostUrgent));
    }

    protected override void CheckNeedStates()
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
            StartCoroutine(manager.GetComponent<PetGOAP>().SetGoal(this.gameObject, goals[index].Key, goals[index].Value, index));
        }
    }

}
