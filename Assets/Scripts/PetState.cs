using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetState : MonoBehaviour
{

    [SerializeField]
    private float currentHunger;
    [SerializeField]
    private float currentSleep;
    [SerializeField]
    private float currentToilet;
    [SerializeField]
    private float currentFun;
    [SerializeField]
    private float currentAttention;

    [SerializeField]
    private float hungerChange;
    [SerializeField]
    private float sleepChange;
    [SerializeField]
    private float toiletChange;
    [SerializeField]
    private float funChange;
    [SerializeField]
    private float attentionChange;

    [SerializeField]
    private float hungry;
    [SerializeField]
    private float sleepy;
    [SerializeField]
    private float needsToilet;
    [SerializeField]
    private float needsFun;
    [SerializeField]
    private float needsAttention;

    private bool askedForAction;
    private GameObject manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("ActionQueue");
        StartCoroutine(NeedChange());
    }

    private void Update()
    {
        if (askedForAction) return;

        if (currentHunger >= hungry)
        {
            WorldState.state.ChangeState(13, false);
            WorldState.state.ChangeState(11, true);
            askedForAction = true;
            manager.GetComponent<GOAPplanner>().SetGoal(this.gameObject, 13, true);
        }
        if (currentSleep >= sleepy)
        {
            askedForAction = true;
            //manager.GetComponent<GOAPplanner>().GoToBed();
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
        if (currentAttention >= needsAttention)
        {
            askedForAction = true;
            //manager.GetComponent<GOAPplanner>
        }
    }

    public void ActionPlanned()
    {
        askedForAction = false;
    }

    private IEnumerator NeedChange()
    {
        while (true)
        {
            currentHunger += hungerChange * Time.deltaTime;
            currentSleep += sleepChange * Time.deltaTime;
            currentToilet += toiletChange * Time.deltaTime;
            currentFun += funChange * Time.deltaTime;
            currentAttention += attentionChange * Time.deltaTime;

            yield return null;
        }
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
        if (currentAttention < 0) currentAttention = 0;
        if (currentAttention > 1) currentAttention = 1;

    }

}
