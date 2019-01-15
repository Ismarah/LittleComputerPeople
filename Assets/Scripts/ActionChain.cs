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

    public float GetChainStateChange()
    {
        float chainCost = 0;
        for (int j = 0; j < myList.Count; j++)
        {
            chainCost += myList[j].GetStateChange();
        }
        return chainCost;
    }

    public float GetChainDuration()
    {
        float chainDuration = 0;

        for (int i = 0; i < myList.Count; i++)
        {
            chainDuration += myList[i].GetTime();
        }

        return chainDuration;
    }
}
