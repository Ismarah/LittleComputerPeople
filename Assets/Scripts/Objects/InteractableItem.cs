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
    protected Action nextAction;
    protected Action[] myActions;
    protected int actionCount;
    protected int useCount;

    protected void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myActions = new Action[actionCount];
    }

    public int GetFloor()
    {
        return myFloor;
    }

    public GameObject GetMyIcon()
    {
        return myIcon;
    }

    public virtual void PlayerArrivedAtMyPosition()
    {
        float[,] temp = nextAction.GetStats();
        for (int i = 0; i < 4; i++)
        {
            if (temp[i, 0] != 0)
            {
                index = i;
                change = temp[i, 0];
                time = temp[i, 1];
                player.GetComponent<NewPlayerNeeds>().ManipulateNeedChange(index, change, time);
            }
        }
    }

    public void PlanAction(int i)
    {
        nextAction = myActions[i];
    }

    public float[] GetActionCosts()
    {
        float[] costs = new float[actionCount];
        for (int i = 0; i < actionCount; i++)
        {
            costs[i] = myActions[i].GetCost();
        }
        return costs;
    }

    public virtual void UseMe()
    {
    }
}
