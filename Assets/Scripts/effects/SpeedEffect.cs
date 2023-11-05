using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffect : Effect
{
    float speed;
    float maxSpeed = .2f;
    public override void Affect()
    {
        hero.speed += speed;
        if (hero.speed > maxSpeed) {
            hero.speed = maxSpeed;
        }
    }

    public override void ClearEffect()
    {
        if (hero.speed < speed)
            hero.speed = 0;
        else
            hero.speed -= speed;
    }

    public void SetSpeed(float _speed) {
        speed = _speed;
    }
}
