using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueue : MonoBehaviour
{
    protected GameObject player;
    protected GameObject pet;
    //protected bool processingAction;
    protected bool bored;
    [SerializeField]
    protected string[] actionNames;
    protected Action[] actionQueue;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pet = GameObject.FindGameObjectWithTag("Pet");
        actionNames = new string[10];
        actionQueue = new Action[10];
    }

    public virtual void Queue()
    {

    }

    public void AddToQueue(Action newAction)
    {
        StopAllCoroutines();

        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] == null)
            {
                actionQueue[i] = newAction;
                if (i == 0)
                {
                    Queue();
                }
                break;
            }
        }
    }

    public virtual void FinishedAction(bool finished)
    {
        
    }

    public bool IsEnqueued(Action action)
    {
        bool foundAction = false;
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null && actionQueue[i] == action)
            {
                foundAction = true;
            }
        }
        return foundAction;
    }

}
