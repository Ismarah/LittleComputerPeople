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

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Action[] GetAllActions()
    {
        return myActions;
    }

}
