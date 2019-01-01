using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : InteractableItem {

    Action playGames;

    void Start()
    {
        actionCount = 1;
        myFloor = 1;

        Init();

        ////Possible action 1
        //float[,] playGamesEffect = new float[4, 2];
        //playGamesEffect[3, 0] = -0.15f;
        //playGamesEffect[3, 1] = 5;
        //playGames = new Action(playGamesEffect);
        //myActions[0] = playGames;
    }
}
