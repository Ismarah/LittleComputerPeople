using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public string name;
    public int favoriteAction;

    void Start()
    {

    }

    void Update()
    {

    }

    public int GetFavoriteAction()
    {
        return favoriteAction;
    }

    public string GetName()
    {
        return name;
    }
}
