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
    private InteractableItem[] interactables;
    private List<GameObject> objects;
    private bool startToGetBored;

    void Start()
    {
        base.Init();
        actionText = canvas.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        interactables = FindObjectsOfType<InteractableItem>();
        objects = new List<GameObject>();
        foreach (InteractableItem i in interactables)
        {
            objects.Add(i.gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
                actionNames[i] = actionQueue[i].GetName();
            else actionNames[i] = "null";
        }
        if (actionQueue[0] == null)
        {
            canvas.SetActive(false);

            if (!startToGetBored && WorldState.state.GetState(WorldState.myStates.petAskedForFood) == false && WorldState.state.GetState(WorldState.myStates.petIsHungry) == false)
            {
                //pet has nothing to do and has not asked for food
                startToGetBored = true;
                StartCoroutine(StartToRunAround());
            }
        }
        else
        {
            if (actionQueue[0].GetName() != "Run around")
            {
                canvas.SetActive(true);
                actionText.text = actionQueue[0].GetName();
            }
        }
        foodInBowl = WorldState.state.GetState(WorldState.myStates.foodInBowl);
    }

    private IEnumerator StartToRunAround()
    {
        yield return new WaitForSeconds(3 / GetComponent<TimeManager>().GetGameSpeed());

        if (WorldState.state.GetState(WorldState.myStates.petAskedForFood) == false && WorldState.state.GetState(WorldState.myStates.petIsHungry) == false)
        {
            AddToQueue(pet.GetComponent<PetActions>().GetAction("Run around"));
            pet.GetComponent<PetState>().ActionIsPlanned();
        }

    }

    public override void Queue()
    {
        if (actionQueue[0].GetObject().GetComponent<InteractableItem>() != null)
        {
            actionQueue[0].GetObject().GetComponent<InteractableItem>().PlanAction(actionQueue[0]);
            pet.GetComponent<AgentMovement>().NewTarget(actionQueue[0].GetObject());
        }
        else if (actionQueue[0].GetObject().GetComponent<PetState>() != null)
        {
            int rand = Random.Range(0, objects.Count);
            objects[rand].GetComponent<InteractableItem>().PlanAction(actionQueue[0]);
            pet.GetComponent<AgentMovement>().NewTarget(objects[rand]);
        }
        else if (actionQueue[0].GetObject().GetComponent<PlayerState>() != null)
        {
            pet.GetComponent<AgentMovement>().NewTarget(actionQueue[0].GetObject());
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
    }

    public override void FinishedAction(bool finished)
    {
        Dictionary<WorldState.myStates, bool> temp = actionQueue[0].GetEffects();
        foreach (KeyValuePair<WorldState.myStates, bool> pair in temp)
        {
            WorldState.state.ChangeState(pair.Key, pair.Value);
        }

        pet.GetComponent<Animator>().SetBool("tail", true);

        if (actionQueue[0].HasAnimation())
            pet.GetComponent<AgentVisuals>().SetAnimationState(actionQueue[0].GetAnimation().Key, false);
        if (actionQueue[0].GetObject().GetComponent<InteractableItem>() != null && actionQueue[0].GetObject().GetComponent<InteractableItem>().HasAnimation())
            actionQueue[0].GetObject().GetComponent<InteractableItem>().ReverseAnimation();

        if (finished) pet.GetComponent<PetState>().ActionFinished();
        if (actionQueue[0] == pet.GetComponent<PetActions>().GetAction("Run around"))
        {
            pet.GetComponent<PetState>().ActionFinished();
        }
        actionQueue[0] = null;

        for (int i = 1; i < actionQueue.Length; i++)
        {
            if (actionQueue[i] != null)
            {
                actionQueue[i - 1] = actionQueue[i];
                actionQueue[i] = null;
            }
        }
        startToGetBored = false;
        if (actionQueue[0] != null) Queue();
    }
}
