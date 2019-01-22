using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{ 
    [SerializeField]
    private Material yellow, green, blue, red, orange, violet;
    private Material[] materials;

    private void Start()
    {
        materials = new Material[] { yellow, green, blue, red, orange, violet };
    }

    public void ChangeClothes()
    {
        Renderer[] temp = GetComponentsInChildren<Renderer>();
        int rand = Random.Range(0, materials.Length);

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].sharedMaterial = materials[rand];
        }
    }
}
