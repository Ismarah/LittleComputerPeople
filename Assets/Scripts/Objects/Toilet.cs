using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : InteractableItem
{
    Action useToilet;
    Action cleanToilet;

	void Start ()
	{
        actionCount = 2;
		myFloor = 0;

		Init ();

        //Possible action 1
        float[,] useToiletEffect = new float[4, 2];
        useToiletEffect[2, 0] = -1f;
        useToiletEffect[2, 1] = 2;
        useToilet = new Action(useToiletEffect);
        myActions[0] = useToilet;

        //Possible action 2
        float[,] cleanToiletEffect = new float[4, 2];
        cleanToiletEffect[3, 0] = 0.3f;
        cleanToiletEffect[3, 1] = 3;
        cleanToilet = new Action(cleanToiletEffect);
        myActions[0] = cleanToilet;
    }
}
