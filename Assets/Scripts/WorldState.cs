using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState state = null;
    private GameObject player;
    private Dictionary<myStates, bool> stateList;

    public enum myStates
    {
        snackInFridge,
        IngredientsInFridge,
        FoodCooked,
        toiletIsClean,
        daytime,
        hasMoney,
        playerHasEaten,
        pizzaIsAvailable,
        pizzaOnTheWay,
        doorBellRang,
        playerSleptThroughNight,
        petIsHungry,
        petAskedForFood,
        petHasEaten,
        playerWasOnToilet,
        playerHasFun,
        playerIsClean,
        playerIsTired,
        playerIsWearingStreetClothes,
        playerHasNothingToDo,
        playerNeedsToilet,
        favoritePlayerAction,
        petIsTired,
        foodInBowl, 
        petIsBored,
        playerHasBook
    }

    void Start()
    {
        state = this;
        player = GameObject.FindGameObjectWithTag("Player");
        stateList = new Dictionary<myStates, bool>();

        stateList.Add(myStates.snackInFridge, true);
        stateList.Add(myStates.IngredientsInFridge, true);
        stateList.Add(myStates.FoodCooked, false);
        stateList.Add(myStates.toiletIsClean, true);
        stateList.Add(myStates.daytime, false);
        stateList.Add(myStates.hasMoney, true);
        stateList.Add(myStates.playerHasEaten, false);
        stateList.Add(myStates.pizzaIsAvailable, false);
        stateList.Add(myStates.pizzaOnTheWay, false);
        stateList.Add(myStates.doorBellRang, false);
        stateList.Add(myStates.playerSleptThroughNight, false);
        stateList.Add(myStates.petIsHungry, false);
        stateList.Add(myStates.petAskedForFood, false);
        stateList.Add(myStates.petHasEaten, false);
        stateList.Add(myStates.playerWasOnToilet, false);
        stateList.Add(myStates.playerHasFun, false);
        stateList.Add(myStates.playerIsClean, true);
        stateList.Add(myStates.playerIsTired, false);
        stateList.Add(myStates.playerIsWearingStreetClothes, true);
        stateList.Add(myStates.playerHasNothingToDo, false);
        stateList.Add(myStates.playerNeedsToilet, false);
        stateList.Add(myStates.favoritePlayerAction, false);
        stateList.Add(myStates.petIsTired, false);
        stateList.Add(myStates.foodInBowl, false);
        stateList.Add(myStates.petIsBored, false);
        stateList.Add(myStates.playerHasBook, false);
    }

    public void ChangeState(myStates state, bool newState)
    {
        stateList[state] = newState;      
    }

    public int GetNumberOfStates()
    {
        return stateList.Count;
    }

    public bool GetState(myStates state)
    {
        return stateList[state];
    }

}
