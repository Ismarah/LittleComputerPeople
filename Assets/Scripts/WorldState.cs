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
 */

public class WorldState : MonoBehaviour {

    public static WorldState state = null;
    private bool[] states;

	void Start ()
    {
        state = this;
        
        states = new bool[14];
        states[0] = false;
        states[1] = true;
        states[2] = true;
        states[3] = true;
        states[4] = true;
        states[5] = true;
        states[6] = true;
        states[7] = true;
        states[8] = true;
        states[9] = true;
        states[10] = true;
        states[11] = true;
        states[12] = true;
        states[13] = true;
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
        if(states[index] == true)
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
