using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public string myName;
    public int favoriteAction;

    public int GetFavoriteAction()
    {
        return favoriteAction;
    }

    public string GetName()
    {
        return myName;
    }
}
