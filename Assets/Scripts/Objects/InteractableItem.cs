using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    protected int myFloor;
    protected GameObject player;
    protected GameObject pet;
    protected int index;
    protected float change, time;
    [SerializeField]
    protected Action[] nextActions;
    [SerializeField]
    protected int useCount;
    [SerializeField]
    protected string myAnimation;

    protected void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pet = GameObject.FindGameObjectWithTag("Pet");
        nextActions = new Action[10];
    }

    public bool HasAnimation()
    {
        if (myAnimation != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetFloor()
    {
        return myFloor;
    }

    public virtual void AgentArrivedAtMyPosition(GameObject agent)
    {
        if (agent != player) agent.GetComponent<PetState>().ManipulateNeedChange(nextActions[0]);
        else
        {
            agent.GetComponent<PlayerState>().ManipulateNeedChange(nextActions[0]);

            if (nextActions[0].HasAnimation())
            {
                player.GetComponent<PlayerVisuals>().ChangeDirection(nextActions[0].GetAnimation().Value);
                player.GetComponent<PlayerVisuals>().SetAnimationState(nextActions[0].GetAnimation().Key, true);
            }
            player.GetComponent<PlayerVisuals>().ChangeTextColor(true);
        }
        nextActions[0] = null;

        for (int i = 0; i < nextActions.Length; i++)
        {
            if (nextActions[i] != null)
            {
                nextActions[i - 1] = nextActions[i];
                nextActions[i] = null;
            }
        }
    }

    public void PlanAction(Action a)
    {
        for (int i = 0; i < nextActions.Length; i++)
        {
            if (nextActions[i] == null)
            {
                nextActions[i] = a;
                break;
            }
        }
    }

    public void ReverseAnimation()
    {
        GetComponent<Animator>().SetBool(myAnimation, false);
    }
}
