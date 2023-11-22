using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip music;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = music;
        source.loop = true;
        source.Play();
    }
}
