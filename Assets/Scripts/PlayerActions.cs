using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    private Action[] myActions;

    float[,] actionEffects;
    Dictionary<int, bool> conditions;
    Dictionary<int, bool> effects;
    Action newAction;

    [SerializeField]
    private GameObject fridge;
    [SerializeField]
    private GameObject bed;
    [SerializeField]
    private GameObject toilet;
    [SerializeField]
    private GameObject computer;
    [SerializeField]
    private GameObject shower;
    private GameObject player;
    [SerializeField]
    private GameObject door;


    void Start()
    {
        myActions = new Action[10];

        player = this.gameObject;

        CreateEatingActions();
        CreateSleepActions();
    }

    private void CreateEatingActions()
    {
        //Eat a meal-------------------------------------------------------------
        actionEffects = new float[4, 2];
        actionEffects[0, 0] = -0.3f;
        actionEffects[0, 1] = 4;

        conditions = new Dictionary<int, bool>();
        conditions.Add(1, true);

        effects = new Dictionary<int, bool>();
        effects.Add(2, false);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[0] = newAction;
        //-----------------------------------------------------------------------

        //Eat a snack------------------------------------------------------------
        actionEffects = new float[4, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 2;

        conditions = new Dictionary<int, bool>();
        conditions.Add(0, true);

        effects = new Dictionary<int, bool>();
        effects.Add(0, false);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[1] = newAction;
        //-----------------------------------------------------------------------

        //Cook a meal------------------------------------------------------------
        actionEffects = new float[4, 2];
        conditions = new Dictionary<int, bool>();
        conditions.Add(1, true);
        effects = new Dictionary<int, bool>();
        effects.Add(1, false);
        effects.Add(2, true);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[2] = newAction;
        //-----------------------------------------------------------------------

        //Order a pizza----------------------------------------------------------
        actionEffects = new float[4, 2];
        conditions = new Dictionary<int, bool>();
        conditions.Add(5, true);
        effects = new Dictionary<int, bool>();
        effects.Add(7, true);

        newAction = new Action(actionEffects, conditions, effects, player);
        myActions[3] = newAction;
        //-----------------------------------------------------------------------

        //Eat a pizza----------------------------------------------------------
        actionEffects = new float[4, 2];
        actionEffects[0, 0] = -0.2f;
        actionEffects[0, 1] = 3;
        conditions = new Dictionary<int, bool>();
        conditions.Add(7, true);
        effects = new Dictionary<int, bool>();
        effects.Add(6, true);

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[3] = newAction;
        //-----------------------------------------------------------------------
    }

    private void CreateSleepActions()
    {
        //Sleep through night----------------------------------------------------
        actionEffects = new float[4, 2];
        actionEffects[1, 0] = -0.2f;
        actionEffects[1, 1] = 10;
        conditions = new Dictionary<int, bool>();
        conditions.Add(4, true);
        effects = new Dictionary<int, bool>();

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[2] = newAction;
        //-----------------------------------------------------------------------

        //Take a nap-------------------------------------------------------------
        actionEffects = new float[4, 2];
        actionEffects[1, 0] = -0.2f;
        actionEffects[1, 1] = 2;

        conditions = new Dictionary<int, bool>();

        effects = new Dictionary<int, bool>();

        newAction = new Action(actionEffects, conditions, effects, fridge);
        myActions[1] = newAction;
        //-----------------------------------------------------------------------
    }


}
