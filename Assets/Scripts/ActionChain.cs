using System.Collections;
using System.Collections.Generic;

public class ActionChain
{
    List<Action> myList;

    public ActionChain()
    {
        myList = new List<Action>();
    }

    public ActionChain(Action newAction)
    {
        myList = new List<Action>();
        myList.Add(newAction);
    }

    public void Add(Action newAction)
    {
        myList.Add(newAction);
    }

    public List<Action> GetActions()
    {
        return myList;
    }

    public float GetChainDuration()
    {
        float duration = 0;

        for (int i = 0; i < myList.Count; i++)
        {
            duration += myList[i].GetCost();
        }

        return duration;
    }

    public float GetNeedChange()
    {
        float change = 0;

        for (int i = 0; i < myList.Count; i++)
        {
            float[,] effects = myList[i].GetStats();
            for (int j = 0; j < 4; j++)
            {
                change += effects[j, 0];
            }
        }

        return change;
    }
}
