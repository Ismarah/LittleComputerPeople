using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    public GameObject[] actionQueue;
    private int[] actionIndices;
    private GameObject player;
    private bool processingAction;

    void Start()
    {
        actionQueue = new GameObject[10];
        actionIndices = new int[10];
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator ActionsQueued()
    {
        while (actionQueue[0] != null)
        {
            if (!processingAction)
            {
                player.GetComponent<NewPlayerMovement>().NewTarget(actionQueue[0]);
                actionQueue[0].GetComponent<InteractableItem>().PlanAction(actionIndices[0]);
                processingAction = true;
            }
            yield return null;
        }
    }

    public void AddToQueue(GameObject newAction, int actionIndex)
    {
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] == null)
            {
                GameObject newIcon = Instantiate(newAction.GetComponent<InteractableItem>().GetMyIcon(), transform, false);
                actionQueue[i] = newAction;
                actionIndices[i] = actionIndex;
                Debug.Log("Enqueue action at index: " + actionIndex);
                if (i != 0)
                {
                    newAction.transform.localPosition = actionQueue[i - 1].GetComponent<InteractableItem>().GetMyIcon().transform.localPosition + new Vector3(100, 0, 0);
                }
                else
                {
                    StartCoroutine(ActionsQueued());
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
                actionQueue[i].GetComponent<InteractableItem>().GetMyIcon().transform.localPosition -= new Vector3(100, 0, 0);
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;

                actionIndices[i - 1] = actionIndices[i];
                actionIndices[i] = -1;

            }
            if (actionQueue[i] == toRemove)
            {
                Destroy(transform.Find(actionQueue[i].name).gameObject.GetComponent<InteractableItem>().GetMyIcon());
                actionQueue[i] = null;
                actionIndices[i] = -1;
                foundAction = true;
            }
        }
    }

    public void FinishedAction()
    {
        Destroy(transform.GetChild(0).gameObject);
        actionQueue[0] = null;
        actionIndices[0] = -1;

        for (int i = 1; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
            {
                actionQueue[i].GetComponent<InteractableItem>().GetMyIcon().transform.localPosition -= new Vector3(100, 0, 0);
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;
            }
        }

        processingAction = false;
        player.GetComponent<NewPlayerNeeds>().ActionFinished();
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
