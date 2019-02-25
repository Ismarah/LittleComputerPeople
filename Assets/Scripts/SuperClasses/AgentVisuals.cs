using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVisuals : MonoBehaviour
{
    [SerializeField]
    protected Material[] materials;

    public virtual void SetAnimationState(string animation, bool state)
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool(animation, state);
    }

    public void ChangeColor(Material material)
    {
        Renderer[] temp = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].sharedMaterial = material;
        }
    }

    public Material[] GetAllClothes()
    {
        return materials;
    }
}
