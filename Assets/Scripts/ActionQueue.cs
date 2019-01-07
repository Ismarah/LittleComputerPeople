using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    public Action[] playerActionQueue;
    public Action[] petActionQueue;
    public GameObject[] icons;
    private GameObject player;
    private GameObject pet;
    private bool processingActionforPlayer;
    private bool processingActionforPet;

    void Start()
    {
        playerActionQueue = new Action[10];
        icons = new GameObject[10];
        player = GameObject.FindGameObjectWithTag("Player");
        pet = GameObject.FindGameObjectWithTag("Pet");
    }

    private void PlayerActionQueue()
    {
        if (!processingActionforPlayer)
        {
            playerActionQueue[0].GetObject().GetComponent<InteractableItem>().PlanAction(playerActionQueue[0]);
            player.GetComponent<AgentMovement>().NewTarget(playerActionQueue[0].GetObject());
            processingActionforPlayer = true;
        }
    }

    private void PetActionQueue()
    {
        if (!processingActionforPet)
        {
            petActionQueue[0].GetObject().GetComponent<InteractableItem>().PlanAction(petActionQueue[0]);
            pet.GetComponent<AgentMovement>().NewTarget(petActionQueue[0].GetObject());
            processingActionforPet = true;
        }
    }

    public void AddToQueue(Action newAction, GameObject agent)
    {
        if (agent == player)
        {
            for (int i = 0; i < playerActionQueue.Length; i++)
            {
                if (playerActionQueue[i] == null)
                {
                    GameObject newIcon = Instantiate(newAction.GetObject().GetComponent<InteractableItem>().GetMyIcon(), transform.position, Quaternion.identity, transform);
                    playerActionQueue[i] = newAction;
                    icons[i] = newIcon;
                    if (i != 0)
                    {
                        icons[i].transform.localPosition = icons[i - 1].transform.localPosition + new Vector3(1.4f, 0, 0);
                    }
                    else
                    {
                        PlayerActionQueue();
                    }
                    break;
                }
            }
            player.GetComponent<PlayerState>().ActionPlanned();
        }
        else
        {
            for (int i = 0; i < petActionQueue.Length; i++)
            {
                if (petActionQueue[i] == null)
                {
                    petActionQueue[i] = newAction;

                    if (i != 0)
                    {
                        PetActionQueue();
                    }
                }
            }
            pet.GetComponent<PetState>().ActionPlanned();
        }
    }

    public void RemoveFromQueue(GameObject toRemove)
    {
        bool foundAction = false;
        for (int i = 0; i < playerActionQueue.Length; i++)
        {
            if (foundAction && playerActionQueue[i] != null)
            {
                playerActionQueue[i].GetObject().GetComponent<InteractableItem>().GetMyIcon().transform.localPosition -= new Vector3(100, 0, 0);
                playerActionQueue[i - 1] = playerActionQueue[i];
                playerActionQueue[i] = null;

            }
            if (playerActionQueue[i].GetObject() == toRemove)
            {
                Destroy(transform.Find(playerActionQueue[i].GetObject().name).gameObject.GetComponent<InteractableItem>().GetMyIcon());
                playerActionQueue[i] = null;
                foundAction = true;
            }
        }
    }

    public void FinishedAction()
    {
        Destroy(icons[0]);
        playerActionQueue[0] = null;
        icons[0] = null;

        for (int i = 1; i < playerActionQueue.Length; i++)
        {
            if (playerActionQueue[i] != null)
            {
                icons[i].transform.localPosition -= new Vector3(1.4f, 0, 0);
                playerActionQueue[i - 1] = playerActionQueue[i];
                playerActionQueue[i] = null;

                icons[i - 1] = icons[i];
                icons[i] = null;
            }
        }

        processingActionforPlayer = false;

        if (playerActionQueue[0] != null)
        {
            PlayerActionQueue();
        }
    }

    public bool IsEnqueued(Action action)
    {
        bool foundAction = false;
        for (int i = 0; i < playerActionQueue.Length; i++)
        {
            if (playerActionQueue[i] != null && playerActionQueue[i] == action)
            {
                foundAction = true;
            }
        }
        return foundAction;
    }

}
