using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisuals : AgentVisuals
{
    [SerializeField]
    private Text actionText;

    public void Init()
    {
        actionText.text = "Hello, my name is " + GetComponent<PlayerCharacter>().GetName();
        actionText.color = Color.blue;
        ChangeColor(GetComponent<PlayerCharacter>().GetClothes());
    }

    public void ChangeClothes()
    {
        Renderer[] temp = GetComponentsInChildren<Renderer>();
        Material current = temp[0].material;
        int rand = GetRandomNumber();

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].sharedMaterial = materials[rand];
        }
        if (temp[0].sharedMaterial == current) ChangeClothes();
    }

    public void ReturnToDefaultClothes()
    {
        ChangeColor(GetComponent<PlayerCharacter>().GetClothes());
    }

    private int GetRandomNumber()
    {
        int rand = Random.Range(0, materials.Length);
        return rand;
    }

    public void ChangeTextColor(bool arrived)
    {
        if (arrived) actionText.color = new Color(0, 255, 1);
        else actionText.color = Color.blue;
    }

    public void ChangeActionText(string newText)
    {
        actionText.text = newText;
    }

    public void ShowActionText(bool show)
    {
        if (!show)
        {
            actionText.transform.parent.GetComponent<Image>().enabled = false;
            actionText.GetComponent<Text>().enabled = false;
        }
        else
        {
            actionText.transform.parent.GetComponent<Image>().enabled = true;
            actionText.GetComponent<Text>().enabled = true;
        }
    }
}
