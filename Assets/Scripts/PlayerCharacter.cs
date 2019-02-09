using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public string myName;
    public string favoriteAction;

    public string GetFavoriteAction()
    {
        return favoriteAction;
    }

    public string GetName()
    {
        return myName;
    }
}
