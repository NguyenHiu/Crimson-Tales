using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneAnimation : MonoBehaviour
{
    public Animator transition;

    public void StartTransition()
    {
        transition.SetTrigger("Start");
    }
}