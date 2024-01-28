using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DetectPlayerToFollow : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        Scan();
    }

    // void Update()
    // {

    // }

    void Scan()
    {
        if (!cinemachineVirtualCamera.Follow)
        {
            HeroController hero = FindAnyObjectByType<HeroController>();
            if (hero)
            {
                cinemachineVirtualCamera.Follow = hero.transform;
                print("detect player");
            }
        }
    }
}
