using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{

    [SerializeField]
    private Image hunger, sleep, toilet, fun, hygene;

    [SerializeField]
    private float currentHunger, currentSleep, currentToilet, currentFun, currentHygene;

    [SerializeField]
    private float hungerChange, sleepChange, toiletChange, funChange, hygeneChange;

    [SerializeField]
    private float hungry, sleepy, needsToilet, needsFun, needsHygene;

    private GameObject manager;
    private bool askedForAction;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("ActionQueue");
        StartCoroutine(NeedChange());
    }

    void Update()
    {
        AdjustNeedBars();
        CheckNeedStates();        
    }

    private void AdjustNeedBars()
    {
        hunger.fillAmount = 1 - currentHunger;
        sleep.fillAmount = 1 - currentSleep;
        toilet.fillAmount = 1 - currentToilet;
        fun.fillAmount = 1 - currentFun;
        hygene.fillAmount = 1 - currentHygene;
    }

    private void CheckNeedStates()
    {
        if (askedForAction) return;

        if (currentHunger >= hungry)
        {
            WorldState.state.ChangeState(WorldState.myStates.playerHasEaten, false);
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, WorldState.myStates.playerHasEaten, true);
        }
        if (currentSleep >= sleepy)
        {
            askedForAction = true;
            WorldState.state.ChangeState(WorldState.myStates.playerIsTired, true);
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, WorldState.myStates.playerIsTired, false);
        }
        if (currentToilet >= needsToilet)
        {
            askedForAction = true;
            WorldState.state.ChangeState(WorldState.myStates.playerNeedsToilet, true);
            WorldState.state.ChangeState(WorldState.myStates.playerWasOnToilet, false);
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, WorldState.myStates.playerWasOnToilet, true);
        }
        if (currentFun >= needsFun)
        {
            askedForAction = true;
            WorldState.state.ChangeState(WorldState.myStates.playerHasNothingToDo, true);
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, WorldState.myStates.playerHasNothingToDo, false);
        }
        if (currentHygene >= needsHygene)
        {
            askedForAction = true;
            WorldState.state.ChangeState(WorldState.myStates.playerIsClean, false);
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, WorldState.myStates.playerIsClean, true);
        }
    }

    public void ActionPlanned()
    {
        askedForAction = false;
    }

    private void LateUpdate()
    {
        if (currentHunger < 0) currentHunger = 0;
        if (currentHunger > 1) currentHunger = 1;
        if (currentSleep < 0) currentSleep = 0;
        if (currentSleep > 1) currentSleep = 1;
        if (currentToilet < 0) currentToilet = 0;
        if (currentToilet > 1) currentToilet = 1;
        if (currentFun < 0) currentFun = 0;
        if (currentFun > 1) currentFun = 1;
        if (currentHygene < 0) currentHygene = 0;
        if (currentHygene > 1) currentHygene = 1;

    }

    private IEnumerator NeedChange()
    {
        while (true)
        {
            currentHunger += hungerChange * Time.deltaTime;
            currentSleep += sleepChange * Time.deltaTime;
            currentToilet += toiletChange * Time.deltaTime;
            currentFun += funChange * Time.deltaTime;
            currentHygene += hygeneChange * Time.deltaTime;

            yield return null;
        }
    }

    public void ManipulateNeedChange(Action action)
    {
        if (action.GetTime() != 0)
        {
            Debug.Log("Starting coroutine now. Action: " + action.GetName());
            StartCoroutine(NeedChangeForATime(action));
        }
    }

    private IEnumerator NeedChangeForATime(Action action)
    {
        float[,] temp = action.GetStats();
        float time = -1;
        bool needWasChanged = false;
        float change = 0;

        for (int i = 0; i < 5; i++)
        {
            change = temp[i, 0];
            if (temp[i, 1] != 0)
            {
                time = temp[i, 1];
                Debug.Log("Change: " + change + " action: " + action.GetName() + " Time: " + time);
            }
            if (change != 0)
            {
                needWasChanged = true;

                hungerChange += temp[0, 0];
                sleepChange += temp[1, 0];
                toiletChange += temp[2, 0];
                funChange += temp[3, 0];
                hygeneChange += temp[4, 0];
            }
        }
        yield return new WaitForSecondsRealtime(time);

        for (int i = 0; i < 5; i++)
        {
            change = temp[i, 0];

            if (change != 0)
            {
                hungerChange -= temp[0, 0];
                sleepChange -= temp[1, 0];
                toiletChange -= temp[2, 0];
                funChange -= temp[3, 0];
                hygeneChange -= temp[4, 0];
            }
        }
        if (needWasChanged) manager.GetComponent<ActionQueue>().FinishedAction(true);
        else manager.GetComponent<ActionQueue>().FinishedAction(false);
    }

    public float GetNeedState(int index)
    {
        switch (index)
        {
            case 0:
                return currentHunger;
            case 1:
                return currentSleep;
            case 2:
                return currentToilet;
            case 3:
                return currentFun;
            case 4:
                return currentHygene;
            default:
                return -1;
        }
    }

    public float GetNeedChange(int index)
    {
        switch (index)
        {
            case 0:
                return hungerChange;
            case 1:
                return sleepChange;
            case 2:
                return toiletChange;
            case 3:
                return funChange;
            case 4:
                return hygeneChange;
            default:
                return -1;
        }
    }

    public float GetCriticalValue(int index)
    {
        switch (index)
        {
            case 0:
                return hungry;
            case 1:
                return sleepy;
            case 2:
                return needsToilet;
            case 3:
                return needsFun;
            case 4:
                return needsHygene;
            default:
                return -1;
        }
    }
}
