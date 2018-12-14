using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPplanner : MonoBehaviour
{
    private GameObject player;
    private GameObject fridge;
    private GameObject toilet;
    private GameObject bed;
    private GameObject computer;
    private GameObject actionQueue;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fridge = GameObject.FindGameObjectWithTag("Fridge");
        toilet = GameObject.FindGameObjectWithTag("Toilet");
        bed = GameObject.FindGameObjectWithTag("Bed");
        computer = GameObject.FindGameObjectWithTag("Computer");
        actionQueue = GameObject.FindGameObjectWithTag("ActionQueue");

        EventManager.StartListening("playerNeedsFood", EatSomething);
        EventManager.StartListening("playerNeedsToPee", UseToilet);
        EventManager.StartListening("playerIsTired", GoToBed);
        EventManager.StartListening("playerIsBored", HaveFun);
    }

    void HaveFun()
    {
        if (!actionQueue.GetComponent<ActionQueue>().IsEnqueued("Computer"))
        {
            computer.GetComponent<Computer>().UseMe();
        }
    }

    void EatSomething()
    {
        if (!actionQueue.GetComponent<ActionQueue>().IsEnqueued("Fridge"))
        {
            if (WorldState.state.GetState(0))
            {
                //food is available in the fridge
                fridge.GetComponent<Fridge>().UseMe();
            }
        }
    }

    void UseToilet()
    {
        if (!actionQueue.GetComponent<ActionQueue>().IsEnqueued("Toilet"))
        {
            if (WorldState.state.GetState(1))
            {
                //the toilet is clean
                toilet.GetComponent<Toilet>().UseMe();
            }
        }
    }

    void GoToBed()
    {
        if (!actionQueue.GetComponent<ActionQueue>().IsEnqueued("Bed"))
        {
            bed.GetComponent<Bed>().UseMe();
        }
    }
}
