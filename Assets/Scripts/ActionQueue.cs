using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueue : MonoBehaviour
{
    public Action[] playerActionQueue;
    public Action[] petActionQueue;
    private GameObject player;
    private GameObject pet;
    private bool processingActionforPlayer;
    private bool processingActionforPet;
    public Text actionText;
    bool bored;
    public string[] actionNames;

    void Start()
    {
        playerActionQueue = new Action[10];
        player = GameObject.FindGameObjectWithTag("Player");
        pet = GameObject.FindGameObjectWithTag("Pet");
        actionNames = new string[10];
    }

    private void Update()
    {
        for (int i = 0; i < playerActionQueue.Length; i++)
        {
            if (playerActionQueue[i] != null)
                actionNames[i] = playerActionQueue[i].GetName();
            else actionNames[i] = "null";
        }
        //string name = "";
        //if (playerActionQueue[0] != null) name = playerActionQueue[0].GetName();
        //else name = "null";
        //Debug.Log(name);
        if (playerActionQueue[0] == null && !bored)
        {
            Debug.Log("boooored");
            bored = true;
            WorldState.state.ChangeState(19, true);
            StartCoroutine(GettingBored());
        }
    }

    private IEnumerator GettingBored()
    {
        yield return new WaitForSeconds(2);

        if (playerActionQueue[0] == null)
        {
            Debug.Log("still bored");
            int action = player.GetComponent<PlayerCharacter>().GetFavoriteAction();
            Action favoriteAction = player.GetComponent<PlayerActions>().GetAction(action);
            if (!IsEnqueued(favoriteAction))
                AddToQueue(favoriteAction, player);
            bored = false;
        }
    }

    private void PlayerActionQueue()
    {
        if (!processingActionforPlayer)
        {
            if (playerActionQueue[0].GetObject().GetComponent<InteractableItem>() != null)
            {
                Debug.Log("Queue action with an interactable item: " + playerActionQueue[0].GetObject().name);
                playerActionQueue[0].GetObject().GetComponent<InteractableItem>().PlanAction(playerActionQueue[0]);
            }
            else if (playerActionQueue[0].GetObject().GetComponent<PlayerState>() != null)
            {
                Debug.Log("Queue action " + playerActionQueue[0] + " at players position");
                player.GetComponent<PlayerState>().ManipulateNeedChange(playerActionQueue[0]);

            }
            actionText.text = playerActionQueue[0].GetName();
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
                    playerActionQueue[i] = newAction;
                    if (i == 0)
                    {
                        PlayerActionQueue();
                    }
                    break;
                }
            }
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

    public void InsertActionAtStartOfQueue(Action newAction, GameObject agent)
    {
        if (agent == player)
        {
            for (int i = playerActionQueue.Length - 1; i >= 0; i--)
            {
                string name = "";
                if (playerActionQueue[i] != null)
                {
                    name = playerActionQueue[i].GetName();
                }
                else
                {
                    name = "null";
                }
                Debug.Log("Action at index " + i + " is called " + name);
                if (i == 0)
                {
                    playerActionQueue[i] = newAction;
                    PlayerActionQueue();
                }
                else
                {
                    playerActionQueue[i] = playerActionQueue[i - 1];
                    playerActionQueue[i] = null;
                }
            }
        }

        for (int i = 0; i < playerActionQueue.Length; i++)
        {
            string name = "";
            if (playerActionQueue[i] != null)
            {
                name = playerActionQueue[i].GetName();
            }
            else
            {
                name = "null";
            }
            Debug.Log("Action at index " + i + " is called " + name);
        }
    }

    public void FinishedAction(bool finished)
    {
        Debug.Log("Finished action " + playerActionQueue[0].GetName());

        Dictionary<int, bool> temp = playerActionQueue[0].GetEffects();
        foreach (KeyValuePair<int, bool> pair in temp)
        {
            WorldState.state.ChangeState(pair.Key, pair.Value);
        }

        playerActionQueue[0] = null;

        for (int i = 1; i < playerActionQueue.Length; i++)
        {
            if (playerActionQueue[i] != null)
            {
                playerActionQueue[i - 1] = playerActionQueue[i];
                playerActionQueue[i] = null;
            }
        }
        if (finished)
        {
            player.GetComponent<PlayerState>().ActionPlanned();
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
