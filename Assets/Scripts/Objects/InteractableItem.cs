using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    [SerializeField]
    protected GameObject myUI;
    [SerializeField]
    protected GameObject myIcon;
    protected int myFloor;
    protected GameObject player;
    protected int index;
    protected float change;
    protected float time;
    [SerializeField]
    protected Action[] nextActions;
    protected Action[] myActions;
    protected int actionCount;
    protected int useCount;

    protected void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myActions = new Action[actionCount];
        nextActions = new Action[10];
    }

    public int GetFloor()
    {
        return myFloor;
    }

    public Action GetAction(int index)
    {
        return myActions[index];
    }

    public GameObject GetMyIcon()
    {
        return myIcon;
    }

    public virtual void PlayerArrivedAtMyPosition()
    {
        float[,] temp = nextActions[0].GetStats();

        nextActions[0] = null;

        for (int i = 0; i < nextActions.Length; i++)
        {
            if (nextActions[i] != null)
            {
                nextActions[i - 1] = nextActions[i];
                nextActions[i] = null;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            index = i;
            change = temp[i, 0];
            time = temp[i, 1];
            player.GetComponent<PlayerState>().ManipulateNeedChange(index, change, time);
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

    public float[] GetActionCosts()
    {
        float[] costs = new float[actionCount];
        for (int i = 0; i < actionCount; i++)
        {
            costs[i] = myActions[i].GetStateChange();
        }
        return costs;
    }

    public virtual void UseMe()
    {
    }
}
