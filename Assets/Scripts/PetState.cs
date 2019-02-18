using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetState : AgentState
{
    [SerializeField]
    private float hungerChange, sleepChange;
    [SerializeField]
    private float hungry, sleepy;

    private void Start()
    {
        base.Init();

        currentNeeds = new float[2];
        needChanges = new float[2];
        needChanges[0] = hungerChange;
        needChanges[1] = sleepChange;

        criticalValues = new float[2];
        criticalValues[0] = hungry;
        criticalValues[1] = sleepy;

        stateChanges = new List<Dictionary<WorldState.myStates, bool>>();
        Dictionary<WorldState.myStates, bool> dict = new Dictionary<WorldState.myStates, bool>();
        dict.Add(WorldState.myStates.petHasEaten, false);
        dict.Add(WorldState.myStates.petIsHungry, true);
        stateChanges.Add(dict);
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.petIsTired, true } });

        goals = new List<KeyValuePair<WorldState.myStates, bool>>();
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.petAskedForFood, true));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.petIsTired, false));

        StartCoroutine(NeedChange());
    }

    private void Update()
    {
        if (!askedForAction)
            CheckNeedStates();
    }

    public void ActionIsPlanned()
    {
        askedForAction = true;
    }

    public void ActionFinished()
    {
        askedForAction = false;
    }

    public void ActionPlanned()
    {
        askedForAction = false;
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

}
