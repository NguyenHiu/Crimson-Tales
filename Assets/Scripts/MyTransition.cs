using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransition : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ActiveEndTransition()
    {
        animator.SetTrigger("End");
    }
}
