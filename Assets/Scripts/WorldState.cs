using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * states[0] = foodInfridge
 * states[1] = toiletIsClean
 */

public class WorldState : MonoBehaviour {

    public static WorldState state = null;
    private bool[] states;

	void Start ()
    {
        state = this;
        
        states = new bool[2];
        states[0] = true;
        states[1] = true;
	}
	
	public void ChangeState(int i, bool newState)
    {
        states[i] = newState;
    }

    public bool GetState(int i)
    {
        return states[i];
    }
}
