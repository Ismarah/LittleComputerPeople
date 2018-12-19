using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : InteractableItem
{
    Action meal;
    Action snack;

    void Start()
    {
        actionCount = 2;
        myFloor = 0;

        Init();

        //Possible action 1
        float[,] mealEffects = new float[4, 2];
        mealEffects[0, 0] = -0.3f;
        mealEffects[0, 1] = 4;
        meal = new Action(mealEffects);
        myActions[0] = meal;

        //Possible action 2
        float[,] snackEffects = new float[4, 2];
        snackEffects[0, 0] = -0.2f;
        snackEffects[0, 1] = 2;
        snack = new Action(snackEffects);
        myActions[1] = snack;
    }
}
