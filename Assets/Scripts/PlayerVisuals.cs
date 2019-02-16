using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField]
    private Material yellow, green, blue, red, orange, violet;
    private Material[] materials;
    private Text actionText;

    private void Start()
    {
        materials = new Material[] { yellow, green, blue, red, orange, violet };
        actionText = GetComponentInChildren<Canvas>().transform.GetChild(0).GetChild(0).GetComponent<Text>();
        actionText.text = "Hello, my name is " + GetComponent<PlayerCharacter>().GetName();
        actionText.color = Color.blue;
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

    public void ReturnToYellow()
    {
        Renderer[] temp = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].sharedMaterial = materials[0];
        }
    }

    private int GetRandomNumber()
    {
        int rand = Random.Range(0, materials.Length);
        return rand;
    }

    public void ChangeTextColor(bool arrived)
    {
        if (arrived) actionText.color = new Color(0, 220, 30);
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

    public void ChangeDirection(bool left)
    {
        if (left && transform.localEulerAngles.y != 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 180);
            actionText.transform.parent.RotateAround(transform.position, Vector3.up, 180);
        }
        else if (!left && transform.localEulerAngles.y == 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 180);
            actionText.transform.parent.RotateAround(transform.position, Vector3.up, 180);
        }
    }

    public void SetAnimationState(string animation, bool state)
    {
        Animator anim = GetComponent<Animator>();

        anim.SetBool(animation, state);
    }
}
