using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    int index = 0;
    string favoriteAction = "";
    string myName;
    int material;

    public void CharacterSelection()
    {

        if (name == "Alex")
        {
            favoriteAction = "Play video games";
        }
        else if (name == "Chris")
        {
            index = 1;
            favoriteAction = "Read a book";
        }
        else if (name == "Elliot")
        {
            index = 2;
            favoriteAction = "Eat a snack";
        }
        else if (name == "Jesse")
        {
            index = 3;
            favoriteAction = "Take a nap";
        }
        myName = name;
        transform.root.GetComponent<StartGame>().PlayerSelected(favoriteAction, index, myName);
        transform.root.GetChild(2).GetComponent<Text>().text = "Please choose a pet";
        transform.root.GetChild(transform.root.childCount - 1).gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void PetSelection()
    {
        if (name == "BrownDark")
            material = 0;
        else if (name == "BrownLight")
            material = 1;
        else
            material = 2;

        transform.root.GetComponent<StartGame>().PetSelected(material);

        transform.root.GetComponent<StartGame>().StartTheGame();
    }


}
