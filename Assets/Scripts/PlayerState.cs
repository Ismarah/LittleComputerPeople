using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : AgentState
{
    [SerializeField]
    private Image hunger, sleep, toilet, fun, hygene;
    [SerializeField]
    private float hungerChange, sleepChange, toiletChange, funChange, hygeneChange;
    [SerializeField]
    private float hungry, sleepy, needsToilet, needsFun, needsHygene;

    void Start()
    {
        base.Init();

        currentNeeds = new float[5];

        needChanges = new float[5];
        needChanges[0] = hungerChange;
        needChanges[1] = sleepChange;
        needChanges[2] = toiletChange;
        needChanges[3] = funChange;
        needChanges[4] = hygeneChange;

        criticalValues = new float[5];
        criticalValues[0] = hungry;
        criticalValues[1] = sleepy;
        criticalValues[2] = needsToilet;
        criticalValues[3] = needsFun;
        criticalValues[4] = needsHygene;

        stateChanges = new List<Dictionary<WorldState.myStates, bool>>();

        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.playerHasEaten, false } });
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.playerIsTired, true } });
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.playerNeedsToilet, true } });
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.playerHasNothingToDo, true } });
        stateChanges.Add(new Dictionary<WorldState.myStates, bool>() { { WorldState.myStates.playerIsClean, false } });

        goals = new List<KeyValuePair<WorldState.myStates, bool>>();
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.playerHasEaten, true));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.playerIsTired, false));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.playerWasOnToilet, true));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.playerHasNothingToDo, false));
        goals.Add(new KeyValuePair<WorldState.myStates, bool>(WorldState.myStates.playerIsClean, true));

        StartCoroutine(NeedChange());
    }

    void Update()
    {
        AdjustNeedBars();
        CheckNeedStates();
    }

    private void AdjustNeedBars()
    {
        hunger.fillAmount = 1 - currentNeeds[0];
        sleep.fillAmount = 1 - currentNeeds[1];
        toilet.fillAmount = 1 - currentNeeds[2];
        fun.fillAmount = 1 - currentNeeds[3];
        hygene.fillAmount = 1 - currentNeeds[4];
    }

    public void ActionIsPlanned()
    {
        askedForAction = true;
    }

    public void ActionFinished()
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

    public virtual void PetArrivedAtMyPosition()
    {
        manager.GetComponent<PlayerQueue>().InsertAtStartOfQueue(GetComponent<PlayerActions>().GetAction("Feed pet"));
        manager.GetComponent<PetQueue>().FeedingNow();
    }

    public void ManipulateNeedChange(Action action)
    {
        if (action.GetTime() != 0) StartCoroutine(NeedChangeForATime(action));
    }

    protected override IEnumerator NeedChangeForATime(Action action)
    {
        yield return StartCoroutine(base.NeedChangeForATime(action));

        if (needWasChanged) manager.GetComponent<PlayerQueue>().FinishedAction(true);
        else manager.GetComponent<PlayerQueue>().FinishedAction(false);
    }
}
