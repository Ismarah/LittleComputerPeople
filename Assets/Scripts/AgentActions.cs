using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActions : MonoBehaviour
{

    protected Action[] myActions;
    protected float[,] actionEffects;
    protected Dictionary<int, bool> conditions;
    protected Dictionary<int, bool> effects;
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
        drawer = GameObject.FindGameObjectWithTag("Couch");
        drawer = GameObject.FindGameObjectWithTag("BathSink");
    }

    public Action[] GetAllActions()
    {
        return myActions;
    }

    public Action GetAction(int i)
    {
        return myActions[i];
    }

}
