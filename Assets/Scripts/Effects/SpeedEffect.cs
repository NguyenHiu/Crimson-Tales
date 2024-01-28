using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffect : Effect
{
    float speed;
    public override void Affect()
    {
        hero.AddSpeed(speed);
    }

    public override void ClearEffect()
    {
        hero.SlowDown(speed);
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
}
