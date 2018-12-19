using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerNeeds : MonoBehaviour
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
    private float currentHunger;
    [SerializeField]
    private float currentSleep;
    [SerializeField]
    private float currentToilet;
    [SerializeField]
    private float currentFun;

    [SerializeField]
    private float hungerChange;
    [SerializeField]
    private float sleepChange;
    [SerializeField]
    private float toiletChange;
    [SerializeField]
    private float funChange;

    [SerializeField]
    private float hungry;
    [SerializeField]
    private float sleepy;
    [SerializeField]
    private float needsToilet;
    [SerializeField]
    private float needsFun;

    private GameObject manager;
    public bool askedForAction;

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
        if (currentHunger >= hungry)
        {
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().EatSomething();
        }
        if (currentSleep >= sleepy)
        {
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().GoToBed();
        }
        if (currentToilet >= needsToilet)
        {
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().UseToilet();
        }
        if (currentFun >= needsFun)
        {
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().HaveFun();
        }
    }

    public void ActionFinished()
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

    }

    private IEnumerator NeedChange()
    {
        while (true)
        {
            currentHunger += hungerChange * Time.deltaTime;
            currentSleep += sleepChange * Time.deltaTime;
            currentToilet += toiletChange * Time.deltaTime;
            currentFun += funChange * Time.deltaTime;

            yield return null;
        }
    }

    public void ManipulateNeedChange(int index, float change, float time)
    {
        StartCoroutine(NeedChangeForATime(index, change, time));
    }

    private IEnumerator NeedChangeForATime(int index, float change, float time)
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
            default:
                break;
        }

        yield return new WaitForSecondsRealtime(time);

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
            default:
                break;
        }

        manager.GetComponent<ActionQueue>().FinishedAction();
    }

    public float GetHunger()
    {
        return currentHunger;
    }

    public float GetSleep()
    {
        return currentSleep;
    }

    public float GetToilet()
    {
        return currentToilet;
    }

    public float GetFun()
    {
        return currentFun;
    }
}
