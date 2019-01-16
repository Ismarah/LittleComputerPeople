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

        conditions = new Dictionary<int, bool>();
        conditions.Add(11, true);

        effects = new Dictionary<int, bool>();
        effects.Add(12, true);

        newAction = new Action("Food please", actionEffects, conditions, effects, player);
        myActions[0] = newAction;
        //-----------------------------------------------------------------------
    }

}
