using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransition : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void ActiveEndTransition()
    {
        animator.SetTrigger("End");
    }

    public void ActiveStartTransition()
    {
        animator.SetTrigger("Start");
    }
}
