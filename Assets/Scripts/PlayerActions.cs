using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : AgentActions
{
    void Start()
    {
        Init();
        myActions = new Action[13];

        player = this.gameObject;

        CreateEatingActions();
        CreateSleepActions();
        CreateFunActions();
    }

    private void CreateEatingActions()
    {
        //Eat a meal-------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 4;

        conditions = new Dictionary<int, bool>();
        conditions.Add(2, true);

        effects = new Dictionary<int, bool>();
        effects.Add(2, false);
        effects.Add(6, true);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[0] = newAction;
        //-----------------------------------------------------------------------

        //Eat a snack------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 2;

        conditions = new Dictionary<int, bool>();
        conditions.Add(0, true);

        effects = new Dictionary<int, bool>();
        effects.Add(0, false);
        effects.Add(6, true);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[1] = newAction;
        //-----------------------------------------------------------------------

        //Cook a meal------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 2;
        conditions = new Dictionary<int, bool>();
        conditions.Add(1, true);
        effects = new Dictionary<int, bool>();
        effects.Add(1, false);
        effects.Add(2, true);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[2] = newAction;
        //-----------------------------------------------------------------------

        //Refill ingredients-----------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 1;
        conditions = new Dictionary<int, bool>();
        conditions.Add(5, true);
        effects = new Dictionary<int, bool>();
        effects.Add(1, true);
        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[3] = newAction;
        //-----------------------------------------------------------------------

        //Order a pizza----------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 1;
        conditions = new Dictionary<int, bool>();
        conditions.Add(5, true);
        effects = new Dictionary<int, bool>();
        effects.Add(8, true);

        newAction = new Action(actionEffects, conditions, effects, player);
        myActions[4] = newAction;
        //-----------------------------------------------------------------------

        //Wait for pizza----------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 3;
        conditions = new Dictionary<int, bool>();
        conditions.Add(8, true);
        effects = new Dictionary<int, bool>();
        effects.Add(9, true);
        effects.Add(8, false);

        newAction = new Action(actionEffects, conditions, effects, player);
        myActions[5] = newAction;
        //-----------------------------------------------------------------------

        //Fetch pizza from door--------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 1;
        conditions = new Dictionary<int, bool>();
        conditions.Add(9, true);
        effects = new Dictionary<int, bool>();
        effects.Add(7, true);

        newAction = new Action(actionEffects, conditions, effects, door);
        myActions[6] = newAction;
        //-----------------------------------------------------------------------

        //Eat a pizza------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 3;
        conditions = new Dictionary<int, bool>();
        conditions.Add(7, true);
        effects = new Dictionary<int, bool>();
        effects.Add(6, true);
        effects.Add(7, false);

        newAction = new Action(actionEffects, conditions, effects, player);
        myActions[7] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateSleepActions()
    {
        //Sleep through night----------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[1, 0] = -0.1f;
        actionEffects[1, 1] = 8;
        conditions = new Dictionary<int, bool>();
        conditions.Add(18, false);

        effects = new Dictionary<int, bool>();
        effects.Add(10, true);
        effects.Add(17, false);

        newAction = new Action(actionEffects, conditions, effects, bed);
        myActions[8] = newAction;
        //-----------------------------------------------------------------------

        //Take a nap-------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[1, 0] = -0.2f;
        actionEffects[1, 1] = 2;

        conditions = new Dictionary<int, bool>();
        conditions.Add(17, true);

        effects = new Dictionary<int, bool>();
        effects.Add(17, false);

        newAction = new Action(actionEffects, conditions, effects, bed);
        myActions[9] = newAction;
        //-----------------------------------------------------------------------

        //Put pyjamas on---------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[1, 0] = 0;
        actionEffects[1, 1] = 0.5f;
        conditions = new Dictionary<int, bool>();
        conditions.Add(4, false);
        conditions.Add(18, true);

        effects = new Dictionary<int, bool>();
        effects.Add(18, false);

        newAction = new Action(actionEffects, conditions, effects, drawer);
        myActions[10] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateToiletActions()
    {

    }

    private void CreateFunActions()
    {
        //Play video games-------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[3, 0] = -0.1f;
        actionEffects[3, 1] = 3;
        conditions = new Dictionary<int, bool>();
        conditions.Add(19, true);
        effects = new Dictionary<int, bool>();
        effects.Add(19, false);

        newAction = new Action(actionEffects, conditions, effects, computer);
        myActions[11] = newAction;
        //-----------------------------------------------------------------------

        //Watch TV---------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[3, 0] = -0.1f;
        actionEffects[3, 1] = 4;
        conditions = new Dictionary<int, bool>();
        conditions.Add(19, true);
        effects = new Dictionary<int, bool>();
        effects.Add(19, false);

        newAction = new Action(actionEffects, conditions, effects, couch);
        myActions[12] = newAction;
        //-----------------------------------------------------------------------
    }


}
