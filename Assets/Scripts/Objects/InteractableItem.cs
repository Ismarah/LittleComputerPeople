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

    protected void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myActions = new Action[actionCount];
    }

    public void OnClick()
    {
        myUI.SetActive(true);
    }

    public int GetFloor()
    {
        return myFloor;
    }

    public GameObject GetMyUIObject()
    {
        return myUI;
    }

    public GameObject GetMyIcon()
    {
        return myIcon;
    }

    public void PlayerMightBeNear()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= 0.5f)
        {
            Debug.Log("player has actually arrived at my position");
            PlayerArrivedAtMyPosition();
        }
    }

    public virtual void PlayerArrivedAtMyPosition()
    {
        float[,] temp = nextAction.GetStats();
        for (int i = 0; i < 4; i++)
        {
            if (temp[i, 0] != 0)
            {
                index = 0;
                change = temp[0, 0];
                time = temp[0, 1];
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
}
