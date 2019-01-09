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

    public float GetChainCost()
    {
        float chainCost = 0;
        for (int j = 0; j < myList.Count; j++)
        {
            chainCost += myList[j].GetCost();
        }
        return chainCost;
    }
}
