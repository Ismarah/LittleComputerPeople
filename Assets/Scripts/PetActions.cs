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
        actionEffects = new float[2];
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.petIsHungry, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.petAskedForFood, true);

        newAction = new Action("Food\nplease", time, actionEffects, conditions, effects, player);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Eat food
        actionEffects = new float[2];
        actionEffects[0] = -0.2f;
        time = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.foodInBowl, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.foodInBowl, false);
        effects.Add(WorldState.myStates.petHasEaten, true);
        effects.Add(WorldState.myStates.petIsHungry, false);

        newAction = new Action("Eat", time, actionEffects, conditions, effects, petFood);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

}
