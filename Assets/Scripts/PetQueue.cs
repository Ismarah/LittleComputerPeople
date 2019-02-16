using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetQueue : ActionQueue
{
    [SerializeField]
    private GameObject canvas;
    private Text actionText;
    private bool foodInBowl;

    void Start()
    {
        base.Init();
        actionText = canvas.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
                actionNames[i] = actionQueue[i].GetName();
            else actionNames[i] = "null";
        }
        if (actionQueue[0] == null) canvas.SetActive(false);
        else canvas.SetActive(true);
    }

    public override void Queue()
    {
        Debug.Log("Pet queue " + actionQueue[0].GetName());
        if (!processingAction)
        {
            if (actionQueue[0].GetObject().GetComponent<InteractableItem>() != null)
            {
                actionQueue[0].GetObject().GetComponent<InteractableItem>().PlanAction(actionQueue[0]);
                //player.GetComponent<PlayerVisuals>().ChangeTextColor(false);
            }
            else if(actionQueue[0].GetObject().GetComponent<PetState>() != null)
            {
                pet.GetComponent<PetState>().ManipulateNeedChange(actionQueue[0]);
            }
            else if(actionQueue[0].GetObject().GetComponent<PlayerState>() != null)
            {
                pet.GetComponent<PetState>().ManipulateNeedChange(actionQueue[0]);
            }
            actionText.text = actionQueue[0].GetName();
            pet.GetComponent<AgentMovement>().NewTarget(actionQueue[0].GetObject());
            processingAction = true;
        }
    }

    public void FeedingNow()
    {
        StartCoroutine(WaitForFood());
    }

    private IEnumerator WaitForFood()
    {
        while (!foodInBowl)
        {
            yield return null;
        }

        AddToQueue(pet.GetComponent<PetActions>().GetAction("Eat"));
        foodInBowl = false;
    }

    public void FinishedFeeding()
    {
        foodInBowl = true; 
    }

    public override void FinishedAction(bool finished)
    {
        Dictionary<WorldState.myStates, bool> temp = actionQueue[0].GetEffects();
        foreach (KeyValuePair<WorldState.myStates, bool> pair in temp)
        {
            WorldState.state.ChangeState(pair.Key, pair.Value);
        }

        pet.GetComponent<Animator>().SetBool("tail", true);

        actionQueue[0] = null;

        for (int i = 1; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
            {
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;
            }
        }
        if (finished) pet.GetComponent<PetState>().ActionFinished();
        processingAction = false;
        bored = false;

        if (actionQueue[0] != null) Queue();
    }
}
