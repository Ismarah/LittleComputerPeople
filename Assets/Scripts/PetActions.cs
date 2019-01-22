using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetActions : AgentActions
{
    void Start()
    {
        Init();
        myActions = new Action[1];

        CreateActions();
    }

    private void CreateActions()
    {
        //Ask player for food
        actionEffects = new float[4, 2];

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.petIsHungry, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.petAskedForFood, true);

        newAction = new Action("Food please", actionEffects, conditions, effects, player);
        myActions[0] = newAction;
        //-----------------------------------------------------------------------
    }

}
