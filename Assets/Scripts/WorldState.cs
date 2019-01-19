using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * states[0] = snackInFridge
 * states[1] = IngredientsInFridge
 * states[2] = FoodCooked
 * states[3] = toiletIsClean
 * states[4] = daytime
 * states[5] = hasMoney
 * states[6] = playerHasEaten
 * states[7] = pizzaIsAvailable
 * states[8] = pizzaOnTheWay
 * states[9] = doorBellRang
 * states[10] = playerSleptThroughNight
 * states[11] = petIsHungry
 * states[12] = petAskedForFood
 * states[13] = petHasEaten
 * states[14] = playerWasOnToilet;
 * states[15] = playerHasFun;
 * states[16] = playerIsClean;
 * states[17] = playerIsTired;
 * states[18] = playerIsWearingStreetClothes;
 * states[19] = playerHasNothingToDo;
 * states[20] = playerNeedsToilet;
 */

public class WorldState : MonoBehaviour
{
    public static WorldState state = null;
    public bool[] states;
    private GameObject player;
    private bool doOnce;

    void Start()
    {
        state = this;
        player = GameObject.FindGameObjectWithTag("Player");

        states = new bool[21];

        states[0] = true;
        states[1] = true;
        states[2] = false;
        states[3] = true;
        states[4] = false;
        states[5] = true;
        states[6] = false;
        states[7] = false;
        states[8] = false;
        states[9] = false;
        states[10] = true;
        states[11] = false;
        states[12] = false;
        states[13] = false;
        states[14] = false;
        states[15] = false;
        states[16] = true;
        states[17] = false;
        states[18] = true;
        states[19] = true;
        states[20] = false;
    }

    public void ChangeState(int i, bool newState)
    {
        states[i] = newState;
    }

    public int GetNumberOfStates()
    {
        return states.Length;
    }

    public void SwitchState(int index)
    {
        if (states[index] == true)
        {
            states[index] = false;
        }
        else
        {
            states[index] = true;
        }
    }

    public bool GetState(int i)
    {
        return states[i];
    }

}



//public enum SnackInFridge { yes, no }
//public enum IngredientsInFridge { yes, no }
//public enum FoodCooked { yes, no }
//public enum ToiletIsClean { yes, no }
//public enum Daytime { yes, no }
//public enum HasMoney { yes, no }
//public enum PlayerHasEaten { yes, no }
//public enum PizzaIsAvailable { yes, no }
//public enum PizzaOnTheWay { yes, no }
//public enum DoorBellRang { yes, no }
//public enum PlayerSleptThroughNight { yes, no }
//public enum PetIsHungry { yes, no }
//public enum PetAskedForFood { yes, no }
//public enum PetHasEaten { yes, no }

//public SnackInFridge snackInFridge = SnackInFridge.yes;
//public IngredientsInFridge ingredientsInFridge = IngredientsInFridge.yes;
//public FoodCooked foodCooked = FoodCooked.yes;
//public ToiletIsClean toiletIsClean = ToiletIsClean.yes;
//public Daytime daytime = Daytime.yes;
//public HasMoney hasMoney = HasMoney.yes;
//public PlayerHasEaten playerHasEaten = PlayerHasEaten.yes;
//public PizzaIsAvailable pizzaIsAvailable = PizzaIsAvailable.yes;
//public PizzaOnTheWay pizzaOnTheWay = PizzaOnTheWay.yes;
//public DoorBellRang doorBellRang = DoorBellRang.yes;
//public PlayerSleptThroughNight playerSleptThroughNight = PlayerSleptThroughNight.yes;
//public PetIsHungry petIsHungry = PetIsHungry.yes;
//public PetAskedForFood petAskedForFood = PetAskedForFood.yes;
//public PetHasEaten petHasEaten = PetHasEaten.yes;

