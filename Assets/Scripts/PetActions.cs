using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetActions : AgentActions
{
    void Start()
    {
        Init();
        CreateActions();
    }

    private void CreateActions()
    {
        //Ask player for food
        actionEffects = new float[3];
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.petIsHungry, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.petAskedForFood, true);

        newAction = new Action(gameObject, "Food\nplease", time, actionEffects, conditions, effects, player);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Eat food
        actionEffects = new float[3];
        actionEffects[0] = -0.2f;
        time = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.foodInBowl, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.foodInBowl, false);
        effects.Add(WorldState.myStates.petHasEaten, true);
        effects.Add(WorldState.myStates.petIsHungry, false);
        effects.Add(WorldState.myStates.petAskedForFood, false);

        newAction = new Action(gameObject, "Eat", time, actionEffects, conditions, effects, petFood);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Run around
        actionEffects = new float[3];
        actionEffects[2] = -0.1f;
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.petIsBored, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.petIsBored, false);

        newAction = new Action(gameObject, "Run around", time, actionEffects, conditions, effects, pet);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

}
