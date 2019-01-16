using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{

    [SerializeField]
    private Image hunger;
    [SerializeField]
    private Image sleep;
    [SerializeField]
    private Image toilet;
    [SerializeField]
    private Image fun;
    [SerializeField]
    private Image hygene;

    [SerializeField]
    private float currentHunger;
    [SerializeField]
    private float currentSleep;
    [SerializeField]
    private float currentToilet;
    [SerializeField]
    private float currentFun;
    [SerializeField]
    private float currentHygene;

    [SerializeField]
    private float hungerChange;
    [SerializeField]
    private float sleepChange;
    [SerializeField]
    private float toiletChange;
    [SerializeField]
    private float funChange;
    [SerializeField]
    private float hygeneChange;

    [SerializeField]
    private float hungry;
    [SerializeField]
    private float sleepy;
    [SerializeField]
    private float needsToilet;
    [SerializeField]
    private float needsFun;
    [SerializeField]
    private float needsHygene;

    private GameObject manager;
    private bool askedForAction;
    private int needCount = 5;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("ActionQueue");
        StartCoroutine(NeedChange());
    }

    void Update()
    {
        hunger.fillAmount = 1 - currentHunger;
        sleep.fillAmount = 1 - currentSleep;
        toilet.fillAmount = 1 - currentToilet;
        fun.fillAmount = 1 - currentFun;
        hygene.fillAmount = 1 - currentHygene;

        if (askedForAction) return;

        if (currentHunger >= hungry)
        {
            WorldState.state.ChangeState(6, false);
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, 6, true);
        }
        if (currentSleep >= sleepy)
        {
            askedForAction = true;
            WorldState.state.ChangeState(17, true);
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, 17, false);
        }
        if (currentToilet >= needsToilet)
        {
            askedForAction = true;
            //manager.GetComponent<GOAPplanner>().UseToilet();
        }
        if (currentFun >= needsFun)
        {
            askedForAction = true;
            //manager.GetComponent<GOAPplanner>().HaveFun();
        }
        if (currentHygene >= needsHygene)
        {
            askedForAction = true;
            //manager.GetComponent<GOAPplanner>
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

    public void ManipulateNeedChange(int index, float change, float time)
    {
        if (time != 0)
        {
            StartCoroutine(NeedChangeForATime(index, change, time));
            Debug.Log("Manipulate need " + index + " for " + time + " seconds by " + change);
        }
    }

    private IEnumerator NeedChangeForATime(int index, float change, float time)
    {
        if (change != 0)
        {
            switch (index)
            {
                case 0:
                    hungerChange += change;
                    break;
                case 1:
                    sleepChange += change;
                    break;
                case 2:
                    toiletChange += change;
                    break;
                case 3:
                    funChange += change;
                    break;
                case 4:
                    hygeneChange += change;
                    break;
                default:
                    break;
            }
        }
        Debug.Log("Wait now for " + time + " seconds");
        yield return new WaitForSecondsRealtime(time);

        if (change != 0)
        {
            switch (index)
            {
                case 0:
                    hungerChange -= change;
                    break;
                case 1:
                    sleepChange -= change;
                    break;
                case 2:
                    toiletChange -= change;
                    break;
                case 3:
                    funChange -= change;
                    break;
                case 4:
                    hygeneChange -= change;
                    break;
                default:
                    break;
            }
        }
        if (change != 0)
        {
            manager.GetComponent<ActionQueue>().FinishedAction(true);
        }
        else
        {
            manager.GetComponent<ActionQueue>().FinishedAction(false);
        }
        Debug.Log("Finished waiting now " + time);
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
