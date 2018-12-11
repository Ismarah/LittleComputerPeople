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
    protected GameObject actionQueue;

	protected void Init ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
        actionQueue = GameObject.FindGameObjectWithTag("ActionQueue");
	}

	public void OnClick ()
	{
		myUI.SetActive (true);
	}

    public void UseMe()
    {
        player.GetComponent<Player>().SetTarget(this.gameObject);
        myUI.SetActive(false);
        GameObject icon = Instantiate(myIcon, actionQueue.transform);
        ActionQueue.instance.GetComponent<ActionQueue>().AddToQueue(icon);
    }

	public int GetFloor ()
	{
		return myFloor;
	}

    public GameObject GetMyUIObject()
    {
        return myUI;
    }

    public void PlayerMightBeNear()
    {
        if(Vector3.Distance(player.transform.position, transform.position) <= 0.5f)
        {
            Debug.Log("player has actually arrived at my position");
            PlayerArrivedAtMyPosition();
        }
    }

    public virtual void PlayerArrivedAtMyPosition()
    {
        //what happens when player arrives here?
    }
}
