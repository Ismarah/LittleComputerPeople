using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    public GameObject[] actionQueue;
    public static ActionQueue instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        actionQueue = new GameObject[10];
    }

    public void AddToQueue(GameObject newAction)
    {
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] == null)
            {
                actionQueue[i] = newAction;
                if (i != 0)
                {
                    newAction.transform.localPosition = actionQueue[i - 1].transform.localPosition + new Vector3(100, 0, 0);
                }
                break;
            }
        }
    }

    public void RemoveFromQueue(GameObject toRemove)
    {
        bool foundAction = false;
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (foundAction && actionQueue[i] != null)
            {
                actionQueue[i].transform.localPosition -= new Vector3(100, 0, 0);
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;
            }
            if (actionQueue[i] = toRemove)
            {
                Destroy(transform.Find(actionQueue[i].name).gameObject);
                actionQueue[i] = null;
                foundAction = true;
            }
        }
    }

    public void FinishedAction()
    {
        Destroy(transform.Find(actionQueue[0].name).gameObject);
        actionQueue[0] = null;

        for (int i = 1; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
            {
                actionQueue[i].transform.localPosition -= new Vector3(100, 0, 0);
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;
            }
        }
    }

    public bool IsEnqueued(string action)
    {
        bool foundAction = false;
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null && actionQueue[i].tag == action)
            {
                foundAction = true;
            }
        }
        return foundAction;
    }

}
