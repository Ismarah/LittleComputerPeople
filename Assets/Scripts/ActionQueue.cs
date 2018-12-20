﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    public GameObject[] actionQueue;
    private int[] actionIndices;
    public GameObject[] icons;
    private GameObject player;
    private bool processingAction;

    void Start()
    {
        actionQueue = new GameObject[10];
        actionIndices = new int[10];
        icons = new GameObject[10];
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void ActionsQueued()
    {
        if (!processingAction)
        {
            actionQueue[0].GetComponent<InteractableItem>().PlanAction(actionIndices[0]);
            player.GetComponent<NewPlayerMovement>().NewTarget(actionQueue[0]);
            processingAction = true;
        }
    }

    public void AddToQueue(GameObject newAction, int actionIndex)
    {
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] == null)
            {
                GameObject newIcon = Instantiate(newAction.GetComponent<InteractableItem>().GetMyIcon(), transform.position, Quaternion.identity, transform);
                actionQueue[i] = newAction;
                actionIndices[i] = actionIndex;
                icons[i] = newIcon;
                //Debug.Log("Enqueue action at index: " + actionIndex);
                if (i != 0)
                {
                    icons[i].transform.localPosition = icons[i - 1].transform.localPosition + new Vector3(1.4f, 0, 0);
                }
                else
                {
                    ActionsQueued();
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
        Destroy(icons[0]);
        actionQueue[0] = null;
        actionIndices[0] = -1;
        icons[0] = null;

        for (int i = 1; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
            {
                icons[i].transform.localPosition -= new Vector3(1.4f, 0, 0);
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;

                actionIndices[i - 1] = actionIndices[i];
                actionIndices[i] = -1;

                icons[i - 1] = icons[i];
                icons[i] = null;
            }
        }

        processingAction = false;
        player.GetComponent<NewPlayerNeeds>().ActionFinished();

        if(actionQueue[0] != null)
        {
            ActionsQueued();
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