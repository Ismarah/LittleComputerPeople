using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    protected int myFloor;
    protected GameObject player;
    protected int index;
    protected float change, time;
    [SerializeField]
    protected Action[] nextActions;
    [SerializeField]
    protected int useCount;

    protected void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nextActions = new Action[10];
    }

    public int GetFloor()
    {
        return myFloor;
    }

    public virtual void PlayerArrivedAtMyPosition()
    {
        player.GetComponent<PlayerState>().ManipulateNeedChange(nextActions[0]);
        nextActions[0] = null;

        for (int i = 0; i < nextActions.Length; i++)
        {
            if(nextActions[i] != null)
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
}
