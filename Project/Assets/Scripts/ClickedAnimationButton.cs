using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Numerics;
using Vector4 = UnityEngine.Vector4;

public class ClickedAnimationButton : MonoBehaviour
{
    [SerializeField] MyKeyCode keycode;
    Image image;

    void Start()
    {
        image = transform.Find("Background").GetComponent<Image>();
    }

    void Clicked()
    {
        image.color = new Vector4(0, 0, 0, 0.3f);
    }

    void UnClick()
    {
        image.color = new Vector4(255, 255, 255, 255);
    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode)keycode))
            Clicked();
        if (Input.GetKeyUp((KeyCode)keycode))
            UnClick();

    }
}

public enum MyKeyCode
{
    A = KeyCode.A,
    D = KeyCode.D,
    S = KeyCode.S,
    W = KeyCode.W,
    Q = KeyCode.Q,
    E = KeyCode.E,
    F = KeyCode.F,
    A1 = KeyCode.Alpha1,
    A2 = KeyCode.Alpha2,
    A3 = KeyCode.Alpha3,
    A4 = KeyCode.Alpha4,
    A5 = KeyCode.Alpha5,
    mouse0 = KeyCode.Mouse0,
    mouse1 = KeyCode.Mouse1,
}