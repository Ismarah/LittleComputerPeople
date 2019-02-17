using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : AgentActions
{
    void Start()
    {
        Init();
        CreateEatingActions();
        CreateSleepActions();
        CreateFunActions();
        CreateToiletActions();
        CreateHygeneActions();
        CreatePetActions();
    }

    private void CreateEatingActions()
    {
        //Eat a meal-------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[0] = -0.2f;
        time = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.FoodCooked, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.FoodCooked, false);
        effects.Add(WorldState.myStates.playerHasEaten, true);

        newAction = new Action("Eat a meal", time, actionEffects, conditions, effects, fridge);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Eat a snack------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[0] = -0.2f;
        time = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.snackInFridge, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.snackInFridge, false);
        effects.Add(WorldState.myStates.playerHasEaten, true);

        newAction = new Action("Eat a snack", time, actionEffects, conditions, effects, fridge);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Refill snack------------------------------------------------------------
        actionEffects = new float[5];
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.snackInFridge, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.snackInFridge, true);

        newAction = new Action("Refill snacks", time, actionEffects, conditions, effects, fridge);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Cook a meal------------------------------------------------------------
        actionEffects = new float[5];
        time = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.IngredientsInFridge, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.IngredientsInFridge, false);
        effects.Add(WorldState.myStates.FoodCooked, true);

        newAction = new Action("Cook a meal", time, actionEffects, conditions, effects, fridge);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Refill ingredients-----------------------------------------------------
        actionEffects = new float[5];
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.hasMoney, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.IngredientsInFridge, true);

        newAction = new Action("Refill ingredients", time, actionEffects, conditions, effects, fridge);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Order a pizza----------------------------------------------------------
        actionEffects = new float[5];
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.hasMoney, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.pizzaOnTheWay, true);

        newAction = new Action("Order a pizza", time, actionEffects, conditions, effects, player);
        //myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Wait for pizza----------------------------------------------------------
        actionEffects = new float[5];
        time = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.pizzaOnTheWay, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.doorBellRang, true);
        effects.Add(WorldState.myStates.pizzaOnTheWay, false);

        newAction = new Action("Wait for pizza", time, actionEffects, conditions, effects, player);
       // myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Fetch pizza from door--------------------------------------------------
        actionEffects = new float[5];
        time = 1;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.doorBellRang, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.pizzaIsAvailable, true);

        newAction = new Action("Fetch pizza", time, actionEffects, conditions, effects, door);
        //myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Eat a pizza------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[0] = -0.1f;
        time = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.pizzaIsAvailable, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerHasEaten, true);
        effects.Add(WorldState.myStates.pizzaIsAvailable, false);

        newAction = new Action("Eat a pizza", time, actionEffects, conditions, effects, player);
        //myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

    private void CreateSleepActions()
    {
        //Sleep through night----------------------------------------------------
        actionEffects = new float[5];
        actionEffects[0] = -0.004f;
        actionEffects[1] = -0.3f;
        actionEffects[2] = -0.004f;
        actionEffects[3] = -0.004f;
        actionEffects[4] = -0.004f;
        time = 5;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsWearingStreetClothes, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerSleptThroughNight, true);
        effects.Add(WorldState.myStates.playerIsTired, false);

        newAction = new Action("Sleep", time, actionEffects, conditions, effects, bed);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Take a nap-------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[1] = -0.2f;
        time = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsTired, true);
        conditions.Add(WorldState.myStates.daytime, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsTired, false);

        newAction = new Action("Take a nap", time, actionEffects, conditions, effects, bed);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Put pyjamas on---------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[1] = 0;
        time = 0.5f;
        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.daytime, false);
        conditions.Add(WorldState.myStates.playerIsWearingStreetClothes, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsWearingStreetClothes, false);

        newAction = new Action("Put on pyjamas", time, actionEffects, conditions, effects, drawer);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Put streetclothes on---------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[1] = 0;
        time = 0.5f;
        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsWearingStreetClothes, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsWearingStreetClothes, true);

        newAction = new Action("Put on street clothes", time, actionEffects, conditions, effects, drawer);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

    private void CreateFunActions()
    {
        //Play video games-------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[3] = -0.12f;
        time = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerHasNothingToDo, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerHasNothingToDo, false);

        newAction = new Action("Play video games", time, actionEffects, conditions, effects, computer);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Watch TV---------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[3] = -0.35f;
        time = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerHasNothingToDo, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerHasNothingToDo, false);

        newAction = new Action("Watch TV", time, actionEffects, conditions, effects, couch);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

    private void CreateToiletActions()
    {
        //Use Toilet-------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[2] = -0.3f;
        actionEffects[4] = 0.08f;
        time = 3;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerNeedsToilet, true);
        conditions.Add(WorldState.myStates.toiletIsClean, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerNeedsToilet, false);
        effects.Add(WorldState.myStates.playerWasOnToilet, true);

        newAction = new Action("Use toilet", time, actionEffects, conditions, effects, toilet);
        newAction.AddAnimation("sitOnToilet", true);
        myActions.Add(newAction);

        //-----------------------------------------------------------------------

        //Clean Toilet---------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[3] = 0.05f;
        actionEffects[4] = 0.1f;
        time = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.toiletIsClean, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.toiletIsClean, true);

        newAction = new Action("Clean toilet", time, actionEffects, conditions, effects, toilet);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

    private void CreateHygeneActions()
    {
        //Shower-----------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[4] = -0.2f;
        time = 4;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerIsClean, false);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerIsClean, true);

        newAction = new Action("Take a shower", time, actionEffects, conditions, effects, shower);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------

        //Wash hands---------------------------------------------------------------
        actionEffects = new float[5];
        actionEffects[4] = -0.15f;
        time = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.playerWasOnToilet, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.playerWasOnToilet, false);

        newAction = new Action("Wash hands", time, actionEffects, conditions, effects, sink);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }

    private void CreatePetActions()
    {
        //Feed pet-----------------------------------------------------------------
        actionEffects = new float[5];
        time = 2;

        conditions = new Dictionary<WorldState.myStates, bool>();
        conditions.Add(WorldState.myStates.petAskedForFood, true);

        effects = new Dictionary<WorldState.myStates, bool>();
        effects.Add(WorldState.myStates.petAskedForFood, false);
        effects.Add(WorldState.myStates.foodInBowl, true);

        newAction = new Action("Feed pet", time, actionEffects, conditions, effects, petFood);
        myActions.Add(newAction);
        //-----------------------------------------------------------------------
    }
}
