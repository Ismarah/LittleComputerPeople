using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueue : MonoBehaviour
{
    public Action[] playerActionQueue;
    public Action[] petActionQueue;
    //public GameObject[] icons;
    private GameObject player;
    private GameObject pet;
    private bool processingActionforPlayer;
    private bool processingActionforPet;
    public int actionQueueCount;
    public Text actionText;

    void Start()
    {
        playerActionQueue = new Action[10];
        //icons = new GameObject[10];
        player = GameObject.FindGameObjectWithTag("Player");
        pet = GameObject.FindGameObjectWithTag("Pet");
    }

    private void Update()
    {
        if (playerActionQueue[0] == null)
        {
            WorldState.state.ChangeState(19, true);
            StartCoroutine(GettingBored());
        }
    }

    private IEnumerator GettingBored()
    {
        yield return new WaitForSeconds(2);

        if(playerActionQueue[0] == null)
        {
            int action = player.GetComponent<PlayerCharacter>().GetFavoriteAction();
            AddToQueue(player.GetComponent<PlayerActions>().GetAction(action), player);
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
                actionText.text = playerActionQueue[0].GetName();
            }
            else if (playerActionQueue[0].GetObject().GetComponent<PlayerState>() != null)
            {
                Debug.Log("Queue action at players position");
                float[,] temp = playerActionQueue[0].GetStats();
                for (int i = 0; i < 5; i++)
                {
                    if (temp[i, 0] != 0)
                    {
                        int index = i;
                        float change = temp[i, 0];
                        float time = temp[i, 1];
                        player.GetComponent<PlayerState>().ManipulateNeedChange(index, change, time);
                    }
                }
            }
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
                    //GameObject newIcon = Instantiate(newAction.GetObject().GetComponent<InteractableItem>().GetMyIcon(), transform.position, Quaternion.identity, transform);
                    playerActionQueue[i] = newAction;
                    //icons[i] = newIcon;
                    if (i != 0)
                    {
                        //icons[i].transform.localPosition = icons[i - 1].transform.localPosition + new Vector3(1.4f, 0, 0);
                    }
                    else
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

    public void FinishedAction(bool finished)
    {
        //Destroy(icons[0]);

        if (playerActionQueue[0] != null)
        {
            Dictionary<int, bool> temp = playerActionQueue[0].GetEffects();
            foreach (KeyValuePair<int, bool> pair in temp)
            {
                WorldState.state.ChangeState(pair.Key, pair.Value);
            }
        }


        playerActionQueue[0] = null;
        //icons[0] = null;

        for (int i = 1; i < playerActionQueue.Length; i++)
        {
            if (playerActionQueue[i] != null)
            {
                //icons[i].transform.localPosition -= new Vector3(1.4f, 0, 0);
                playerActionQueue[i - 1] = playerActionQueue[i];
                playerActionQueue[i] = null;

                //icons[i - 1] = icons[i];
                //icons[i] = null;
            }
        }
        if (finished)
        {
            player.GetComponent<PlayerState>().ActionPlanned();
            Debug.Log("Tell player that action has been planned");
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
