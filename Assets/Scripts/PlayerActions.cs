using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : AgentActions
{
    void Start()
    {
        Init();
        myActions = new Action[17];

        player = this.gameObject;

        CreateEatingActions();
        CreateSleepActions();
        CreateFunActions();
        CreateToiletActions();
        CreateHygeneActions();
    }

    private void CreateEatingActions()
    {
        //Eat a meal-------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.FoodCooked, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.FoodCooked, false);
        effects.Add(WorldState.myStates.playerHasEaten, true);

        newAction = new Action("Eat a meal", actionEffects, conditions, effects, fridge);
        myActions[0] = newAction;
        //-----------------------------------------------------------------------

        //Eat a snack------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.snackInFridge, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.snackInFridge, false);
        effects.Add(WorldState.myStates.playerHasEaten, true);

        newAction = new Action("Eat a snack", actionEffects, conditions, effects, fridge);
        myActions[1] = newAction;
        //-----------------------------------------------------------------------

        //Cook a meal------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.IngredientsInFridge, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.IngredientsInFridge, false);
        effects.Add(WorldState.myStates.FoodCooked, true);

        newAction = new Action("Cook a meal", actionEffects, conditions, effects, fridge);
        myActions[2] = newAction;
        //-----------------------------------------------------------------------

        //Refill ingredients-----------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.hasMoney, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.IngredientsInFridge, true);

        newAction = new Action("Refill ingredients", actionEffects, conditions, effects, fridge);
        myActions[3] = newAction;
        //-----------------------------------------------------------------------

        //Order a pizza----------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.hasMoney, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.pizzaOnTheWay, true);

        newAction = new Action("Order a pizza", actionEffects, conditions, effects, player);
        myActions[4] = newAction;
        //-----------------------------------------------------------------------

        //Wait for pizza----------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.pizzaOnTheWay, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.doorBellRang, true);
        effects.Add(WorldState.myStates.pizzaOnTheWay, false);

        newAction = new Action("Wait for pizza", actionEffects, conditions, effects, player);
        myActions[5] = newAction;
        //-----------------------------------------------------------------------

        //Fetch pizza from door--------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 1] = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.doorBellRang, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.pizzaIsAvailable, true);

        newAction = new Action("Fetch pizza", actionEffects, conditions, effects, door);
        myActions[6] = newAction;
        //-----------------------------------------------------------------------

        //Eat a pizza------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[0, 0] = -0.1f;
        actionEffects[0, 1] = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.pizzaIsAvailable, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerHasEaten, true);
        effects.Add(WorldState.myStates.pizzaIsAvailable, false);

        newAction = new Action("Eat a pizza",actionEffects, conditions, effects, player);
        myActions[7] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateSleepActions()
    {
        //Sleep through night----------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[1, 0] = -0.1f;
        actionEffects[1, 1] = 8;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsWearingStreetClothes, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerSleptThroughNight, true);
        effects.Add(WorldState.myStates.playerIsTired, false);

        newAction = new Action("Sleep", actionEffects, conditions, effects, bed);
        myActions[8] = newAction;
        //-----------------------------------------------------------------------

        //Take a nap-------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[1, 0] = -0.2f;
        actionEffects[1, 1] = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsTired, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsTired, false);

        newAction = new Action("Take a nap", actionEffects, conditions, effects, bed);
        myActions[9] = newAction;
        //-----------------------------------------------------------------------

        //Put pyjamas on---------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[1, 0] = 0;
        actionEffects[1, 1] = 0.5f;
        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.daytime, false);
        conditions.Add(WorldState.myStates.playerIsWearingStreetClothes, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsWearingStreetClothes, false);

        newAction = new Action("Put on pyjamas", actionEffects, conditions, effects, drawer);
        myActions[10] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateFunActions()
    {
        //Play video games-------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[3, 0] = -0.1f;
        actionEffects[3, 1] = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerHasNothingToDo, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerHasNothingToDo, false);

        newAction = new Action("Play video games", actionEffects, conditions, effects, computer);
        myActions[11] = newAction;
        //-----------------------------------------------------------------------

        //Watch TV---------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[3, 0] = -0.1f;
        actionEffects[3, 1] = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerHasNothingToDo, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerHasNothingToDo, false);

        newAction = new Action("Watch TV", actionEffects, conditions, effects, couch);
        myActions[12] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateToiletActions()
    {
        //Use Toilet-------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[2, 0] = -0.2f;
        actionEffects[2, 1] = 2;
        actionEffects[4, 0] = 0.15f;
        actionEffects[4, 1] = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerNeedsToilet, true);
        conditions.Add(WorldState.myStates.toiletIsClean, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerNeedsToilet, false);
        effects.Add(WorldState.myStates.playerWasOnToilet, true);

        newAction = new Action("Use toilet", actionEffects, conditions, effects, toilet);
        myActions[13] = newAction;
        //-----------------------------------------------------------------------

        //Clean Toilet---------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[3, 0] = 0.1f;
        actionEffects[3, 1] = 3;
        actionEffects[4, 0] = 0.2f;
        actionEffects[4, 1] = 3;
        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.toiletIsClean, false);
        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.toiletIsClean, true);

        newAction = new Action("Clean toilet", actionEffects, conditions, effects, toilet);
        myActions[14] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateHygeneActions()
    {
        //Shower-----------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[4, 0] = -0.2f;
        actionEffects[4, 1] = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsClean, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsClean, true);

        newAction = new Action("Take a shower", actionEffects, conditions, effects, shower);
        myActions[15] = newAction;
        //-----------------------------------------------------------------------

        //Wash hands---------------------------------------------------------------
        actionEffects = new float[5, 2];
        actionEffects[4, 0] = -0.15f;
        actionEffects[4, 1] = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerWasOnToilet, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsClean, true);
        effects.Add(WorldState.myStates.playerWasOnToilet, false);

        newAction = new Action("Wash hands", actionEffects, conditions, effects, sink);
        myActions[16] = newAction;
        //-----------------------------------------------------------------------
    }

}
