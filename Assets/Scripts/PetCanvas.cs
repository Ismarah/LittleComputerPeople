using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCanvas : MonoBehaviour
{
    private GameObject pet;
    private Vector3 offset;

    void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet");
        offset = transform.position - pet.transform.position;
    }

    void LateUpdate()
    {
        transform.position = pet.transform.position + offset;
    }
}
