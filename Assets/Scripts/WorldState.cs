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
 */

public class WorldState : MonoBehaviour {

    public static WorldState state = null;
    private bool[] states;

	void Start ()
    {
        state = this;
        
        states = new bool[4];
        states[0] = false;
        states[1] = true;
        states[2] = true;
        states[3] = true;
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
