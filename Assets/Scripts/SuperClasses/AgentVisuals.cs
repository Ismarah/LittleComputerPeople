using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVisuals : MonoBehaviour
{

    public virtual void SetAnimationState(string animation, bool state)
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool(animation, state);
    }
}
