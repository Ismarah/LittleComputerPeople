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
    protected Action[] nextPlayerActions;
    [SerializeField]
    protected Action[] nextPetActions;
    [SerializeField]
    protected int useCount;
    [SerializeField]
    protected string myAnimation;

    protected void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pet = GameObject.FindGameObjectWithTag("Pet");
        nextPlayerActions = new Action[10];
        nextPetActions = new Action[10];
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
        if (agent != player)
        {
            agent.GetComponent<PetState>().ManipulateNeedChange(nextPetActions[0]);
            nextPetActions[0] = null;

            for (int i = 0; i < nextPetActions.Length; i++)
            {
                if (nextPetActions[i] != null)
                {
                    nextPetActions[i - 1] = nextPetActions[i];
                    nextPetActions[i] = null;
                }
            }
        }
        else
        {
            agent.GetComponent<PlayerState>().ManipulateNeedChange(nextPlayerActions[0]);

            if (nextPlayerActions[0].HasAnimation())
            {
                player.GetComponent<PlayerVisuals>().ChangeDirection(nextPlayerActions[0].GetAnimation().Value);
                player.GetComponent<PlayerVisuals>().SetAnimationState(nextPlayerActions[0].GetAnimation().Key, true);
            }
            player.GetComponent<PlayerVisuals>().ChangeTextColor(true);
            nextPlayerActions[0] = null;
            for (int i = 0; i < nextPlayerActions.Length; i++)
            {
                if (nextPlayerActions[i] != null)
                {
                    nextPlayerActions[i - 1] = nextPlayerActions[i];
                    nextPlayerActions[i] = null;
                }
            }
        }

    }

    public void PlanAction(Action a)
    {
        if (a.GetAgent() == player)
        {
            for (int i = 0; i < nextPlayerActions.Length; i++)
            {
                if (nextPlayerActions[i] == null)
                {
                    nextPlayerActions[i] = a;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < nextPetActions.Length; i++)
            {
                if (nextPetActions[i] == null)
                {
                    nextPetActions[i] = a;
                    break;
                }
            }
        }
    }

    public void ReverseAnimation()
    {
        GetComponent<Animator>().SetBool(myAnimation, false);
    }
}
