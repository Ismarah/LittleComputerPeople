using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQueue : ActionQueue
{
    void Start()
    {
        base.Init();
    }

    private void Update()
    {
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
                actionNames[i] = actionQueue[i].GetName();
            else actionNames[i] = "null";
        }

        if (actionQueue[0] == null) player.GetComponent<PlayerVisuals>().ShowActionText(false);
        else player.GetComponent<PlayerVisuals>().ShowActionText(true);

        if (actionQueue[0] == null && actionQueue[1] == null && !bored)
        {
            bored = true;
            WorldState.state.ChangeState(WorldState.myStates.playerHasNothingToDo, true);
            StartCoroutine(GettingBored());
        }
    }

    private IEnumerator GettingBored()
    {
        yield return new WaitForSeconds(3);

        if (actionQueue[0] == null && actionQueue[1] == null)
        {
            yield return StartCoroutine(GetComponent<GOAPplanner>().SetGoal(player, WorldState.myStates.favoritePlayerAction, true, 3));
            player.GetComponent<PlayerState>().ActionIsPlanned();
        }
        bored = false;
    }

    public override void Queue()
    {
        if (actionQueue[0].GetObject().GetComponent<InteractableItem>() != null)
        {
            actionQueue[0].GetObject().GetComponent<InteractableItem>().PlanAction(actionQueue[0]);
            player.GetComponent<PlayerVisuals>().ChangeTextColor(false);

            if (actionQueue[0] == player.GetComponent<PlayerActions>().GetAction("Feed pet"))
                GetComponent<PetQueue>().FeedingNow();
        }

        else if (actionQueue[0].GetObject().GetComponent<PlayerState>() != null)
        {
            player.GetComponent<PlayerState>().ManipulateNeedChange(actionQueue[0]);
            player.GetComponent<PlayerVisuals>().ChangeTextColor(true);
        }

        player.GetComponent<PlayerVisuals>().ChangeActionText(actionQueue[0].GetName());
        player.GetComponent<AgentMovement>().NewTarget(actionQueue[0].GetObject());
    }

    public override void FinishedAction(bool finished)
    {
        Dictionary<WorldState.myStates, bool> temp = actionQueue[0].GetEffects();
        foreach (KeyValuePair<WorldState.myStates, bool> pair in temp)
        {
            WorldState.state.ChangeState(pair.Key, pair.Value);
        }

        if (actionQueue[0].HasAnimation())
            player.GetComponent<PlayerVisuals>().SetAnimationState(actionQueue[0].GetAnimation().Key, false);
        if (actionQueue[0].GetObject().GetComponent<InteractableItem>() != null && actionQueue[0].GetObject().GetComponent<InteractableItem>().HasAnimation())
            actionQueue[0].GetObject().GetComponent<InteractableItem>().ReverseAnimation();

        actionQueue[0] = null;

        for (int i = 1; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
            {
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;
            }
        }
        if (finished) player.GetComponent<PlayerState>().ActionFinished();
        bored = false;

        if (actionQueue[0] != null) Queue();
    }
}
