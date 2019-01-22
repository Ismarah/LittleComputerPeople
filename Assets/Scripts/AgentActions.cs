﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActions : MonoBehaviour
{

    protected Action[] myActions;
    protected float[,] actionEffects;
    protected Dictionary<WorldState.myStates, bool> conditions;
    protected Dictionary<WorldState.myStates, bool> effects;
    protected Action newAction;
    protected GameObject player;

    protected GameObject fridge;
    protected GameObject bed;
    protected GameObject toilet;
    protected GameObject computer;
    protected GameObject shower;
    protected GameObject door;
    protected GameObject petFood;
    protected GameObject pet;
    protected GameObject drawer;
    protected GameObject couch;
    protected GameObject sink;

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

    public Action[] GetAllActions()
    {
        return myActions;
    }

    public Action GetAction(int i)
    {
        return myActions[i];
    }

    public Action GetAction(string name)
    {
        for (int i = 0; i < myActions.Length; i++)
        {
            if(myActions[i].GetName() == name)
            {
                return myActions[i];
            }
        }
        return new Action();
    }

}
