using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActions : MonoBehaviour
{
    protected List<Action> myActions;
    protected float[] actionEffects;
    protected Dictionary<WorldState.myStates, bool> conditions, effects;
    protected Action newAction;
    protected GameObject player;
    protected float time;
    protected GameObject fridge, bed, toilet, computer, shower, door, petFood, pet, drawer, couch, sink;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fridge = GameObject.FindGameObjectWithTag("Fridge");
        toilet = GameObject.FindGameObjectWithTag("Toilet");
        bed = GameObject.FindGameObjectWithTag("Bed");
        computer = GameObject.FindGameObjectWithTag("Computer");
        door = GameObject.FindGameObjectWithTag("Door");
        petFood = GameObject.FindGameObjectWithTag("PetFood");
        pet = GameObject.FindGameObjectWithTag("Pet");
        drawer = GameObject.FindGameObjectWithTag("Drawer");
        couch = GameObject.FindGameObjectWithTag("Couch");
        sink = GameObject.FindGameObjectWithTag("BathSink");
        shower = GameObject.FindGameObjectWithTag("Shower");
    }

    public List<Action> GetAllActions()
    {
        return myActions;
    }

    public Action GetAction(int i)
    {
        return myActions[i];
    }

    public Action GetAction(string name)
    {
        for (int i = 0; i < myActions.Count; i++)
        {
            if (myActions[i].GetName() == name)
            {
                return myActions[i];
            }
        }
        return new Action();
    }

}
